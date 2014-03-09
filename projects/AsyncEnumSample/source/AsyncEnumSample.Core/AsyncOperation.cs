//-----------------------------------------------------------------------
// <copyright file="AsyncOperation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnumSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class AsyncOperation<TResult>
    {
        private TaskCompletionSource<TResult> tcs;
        private IEnumerator<Step> steps;
        private Action<Task> moveNext;

        protected AsyncOperation()
        {
        }

        protected event EventHandler SchedulingMoveNext;

        protected interface IExceptionHandler
        {
            bool Handle(Exception exception);
        }

        protected TResult Result { get; set; }

        protected bool RunMoveNextSynchronously { get; set; }

        public Task<TResult> Start()
        {
            this.tcs = new TaskCompletionSource<TResult>();
            this.steps = this.Steps();
            this.moveNext = this.MoveNext;
            this.MoveNext(null);
            return this.tcs.Task;
        }

        protected abstract IEnumerator<Step> Steps();

        private void MoveNext(Task task)
        {
            try
            {
                if ((task != null) && (task.Exception != null))
                {
                    throw task.Exception.Flatten();
                }

                while (this.steps.MoveNext())
                {
                    Task nextTask = this.steps.Current.Invoke();
                    switch (nextTask.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            // no-op
                            break;
                        case TaskStatus.Faulted:
                            throw nextTask.Exception.Flatten();
                        default:
                            this.ScheduleMoveNext(nextTask);
                            return;
                    }
                }

                this.tcs.SetResult(this.Result);
            }
            catch (Exception e)
            {
                using (this.steps)
                {
                    if (task == null)
                    {
                        throw;
                    }

                    AggregateException ae = e as AggregateException;
                    if (ae != null)
                    {
                        this.tcs.SetException(ae.InnerExceptions);
                    }
                    else
                    {
                        this.tcs.SetException(e);
                    }
                }
            }
        }

        private void ScheduleMoveNext(Task nextTask)
        {
            EventHandler handler = this.SchedulingMoveNext;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            TaskContinuationOptions options = this.RunMoveNextSynchronously ? TaskContinuationOptions.ExecuteSynchronously : TaskContinuationOptions.None;
            nextTask.ContinueWith(this.moveNext, options);
        }

        protected struct Step
        {
            private readonly Func<Task> doAsync;

            private Step(Func<Task> doAsync)
            {
                this.doAsync = doAsync;
            }

            public static Task<T> TaskFromResult<T>(T result)
            {
                TaskCompletionSource<T> resultTask = new TaskCompletionSource<T>();
                resultTask.SetResult(result);
                return resultTask.Task;
            }

            public static Task TaskFromException(Exception exception)
            {
                return TaskFromException<bool>(exception);
            }

            public static Task<T> TaskFromException<T>(Exception exception)
            {
                TaskCompletionSource<T> exceptionTask = new TaskCompletionSource<T>();
                exceptionTask.SetException(exception);
                return exceptionTask.Task;
            }

            public static Step Await<TState>(TState state, Func<TState, AsyncCallback, object, IAsyncResult> begin, Action<TState, IAsyncResult> end)
            {
                return Await(new LegacyState<TState>(state, begin, end), l => Task.Factory.FromAsync((c, s) => LegacyBegin<TState>(c, s), r => LegacyEnd<TState>(r), l));
            }

            public static Step Await<TState>(TState state, Func<TState, Task> doAsync, params IExceptionHandler[] handlers)
            {
                return Await(Tuple.Create(state, doAsync), t => DoWithFakeReturnAsync(t.Item1, t.Item2), (t, r) => { }, handlers);
            }

            public static Step Await<TState, TCallResult>(TState state, Func<TState, Task<TCallResult>> doAsync, Action<TState, TCallResult> afterCall, params IExceptionHandler[] handlers)
            {
                return new AsyncCall<TState, TCallResult>(state, doAsync, afterCall, handlers).Step;
            }

            public Task Invoke()
            {
                return this.doAsync();
            }

            private static IAsyncResult LegacyBegin<TState>(AsyncCallback callback, object state)
            {
                LegacyState<TState> legacy = (LegacyState<TState>)state;
                return legacy.Begin(callback, state);
            }

            private static void LegacyEnd<TState>(IAsyncResult result)
            {
                LegacyState<TState> legacy = (LegacyState<TState>)result.AsyncState;
                legacy.End(result);
            }

            private static Task<bool> DoWithFakeReturnAsync<TState>(TState state, Func<TState, Task> doAsync)
            {
                return doAsync(state).ContinueWith(t => ObserveException(t), TaskContinuationOptions.ExecuteSynchronously);
            }

            private static bool ObserveException(Task task)
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                return false;
            }

            private sealed class LegacyState<TState>
            {
                private readonly TState state;
                private readonly Func<TState, AsyncCallback, object, IAsyncResult> begin;
                private readonly Action<TState, IAsyncResult> end;

                public LegacyState(TState state, Func<TState, AsyncCallback, object, IAsyncResult> begin, Action<TState, IAsyncResult> end)
                {
                    this.state = state;
                    this.begin = begin;
                    this.end = end;
                }

                public IAsyncResult Begin(AsyncCallback callback, object state)
                {
                    return this.begin(this.state, callback, state);
                }

                public void End(IAsyncResult result)
                {
                    this.end(this.state, result);
                }
            }

            private sealed class AsyncCall<TState, TCallResult>
            {
                private static readonly Task CompletedTask = TaskFromResult(false);

                private readonly TState state;
                private readonly Func<TState, Task<TCallResult>> doAsync;
                private readonly Action<TState, TCallResult> afterCall;
                private readonly IExceptionHandler[] handlers;

                public AsyncCall(TState state, Func<TState, Task<TCallResult>> doAsync, Action<TState, TCallResult> afterCall, IExceptionHandler[] handlers)
                {
                    this.state = state;
                    this.doAsync = doAsync;
                    this.afterCall = afterCall;
                    this.handlers = handlers;
                }

                public Step Step
                {
                    get { return new Step(this.DoAsync); }
                }

                private static Exception Unpack(AggregateException ae)
                {
                    if (ae == null)
                    {
                        return null;
                    }

                    ae = ae.Flatten();
                    if (ae.InnerExceptions.Count == 1)
                    {
                        return ae.InnerExceptions[0];
                    }

                    return ae;
                }

                private Task DoAsync()
                {
                    try
                    {
                        return this.doAsync(this.state).ContinueWith<TCallResult>(this.AfterCall, TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception e)
                    {
                        bool handled = this.Handle(e);
                        if (handled)
                        {
                            return CompletedTask;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                private bool Handle(Exception exception)
                {
                    bool handled = false;
                    if (exception != null)
                    {
                        foreach (IExceptionHandler handler in this.handlers)
                        {
                            if (handler.Handle(exception))
                            {
                                handled = true;
                                break;
                            }
                        }
                    }

                    return handled;
                }

                private TCallResult AfterCall(Task<TCallResult> task)
                {
                    TCallResult result;
                    if (this.Handle(Unpack(task.Exception)))
                    {
                        result = default(TCallResult);
                    }
                    else
                    {
                        result = task.Result;
                        this.afterCall(this.state, result);
                    }

                    return result;
                }
            }
        }

        protected static class Catch<TException> where TException : Exception
        {
            public static IExceptionHandler AndHandle<TState>(TState state, Func<TState, TException, bool> handler)
            {
                return new ExceptionHandler<TState>(state, handler);
            }

            private sealed class ExceptionHandler<TState> : IExceptionHandler
            {
                private readonly TState state;
                private readonly Func<TState, TException, bool> handler;

                public ExceptionHandler(TState state, Func<TState, TException, bool> handler)
                {
                    this.state = state;
                    this.handler = handler;
                }

                public bool Handle(Exception exception)
                {
                    bool handled = false;
                    TException e = exception as TException;
                    if (e != null)
                    {
                        handled = this.handler(this.state, e);
                    }

                    return handled;
                }
            }
        }
    }
}
