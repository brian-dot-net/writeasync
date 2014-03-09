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

        protected AsyncOperation()
        {
        }

        protected TResult Result { get; set; }

        public Task<TResult> Start()
        {
            this.tcs = new TaskCompletionSource<TResult>();
            IEnumerator<Step> steps = this.Steps();
            while (steps.MoveNext())
            {
                Task task = steps.Current.Invoke();
                if (task.IsFaulted)
                {
                    throw task.Exception.Flatten();
                }
            }

            this.tcs.SetResult(this.Result);
            return this.tcs.Task;
        }

        protected abstract IEnumerator<Step> Steps();

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

            public static Step Await<TState>(TState state, Func<TState, Task> doAsync)
            {
                return Await(Tuple.Create(state, doAsync), t => DoWithFakeReturnAsync(t.Item1, t.Item2), (t, r) => { });
            }

            public static Step Await<TState, TCallResult>(TState state, Func<TState, Task<TCallResult>> doAsync, Action<TState, TCallResult> afterCall)
            {
                return new AsyncCall<TState, TCallResult>(state, doAsync, afterCall).Step;
            }

            public Task Invoke()
            {
                return this.doAsync();
            }

            private static Task<bool> DoWithFakeReturnAsync<TState>(TState state, Func<TState, Task> doAsync)
            {
                return doAsync(state).ContinueWith(t => ObserveException(t), TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.AttachedToParent);
            }

            private static bool ObserveException(Task task)
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                return false;
            }

            private sealed class AsyncCall<TState, TCallResult>
            {
                private readonly TState state;
                private readonly Func<TState, Task<TCallResult>> doAsync;
                private readonly Action<TState, TCallResult> afterCall;

                public AsyncCall(TState state, Func<TState, Task<TCallResult>> doAsync, Action<TState, TCallResult> afterCall)
                {
                    this.state = state;
                    this.doAsync = doAsync;
                    this.afterCall = afterCall;
                }

                public Step Step
                {
                    get { return new Step(this.DoAsync); }
                }

                private Task DoAsync()
                {
                    return this.doAsync(this.state).ContinueWith<TCallResult>(this.AfterCall, TaskContinuationOptions.ExecuteSynchronously);
                }

                private TCallResult AfterCall(Task<TCallResult> task)
                {
                    TCallResult result = task.Result;
                    this.afterCall(this.state, result);
                    return result;
                }
            }
        }
    }
}
