//-----------------------------------------------------------------------
// <copyright file="AsyncOperation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnum35Sample
{
    using System;
    using System.Collections.Generic;

    public abstract class AsyncOperation<TResult>
    {
        private AsyncResult<TResult> result;
        private IEnumerator<Step> steps;
        private AsyncCallback moveNext;
        private Step currentStep;
        private bool anyStepCompletedAsync;

        protected AsyncOperation()
        {
        }

        protected interface IExceptionHandler
        {
            bool Handle(Exception exception);
        }

        protected TResult Result { get; set; }

        public static TResult End(IAsyncResult result)
        {
            return ((AsyncResult<TResult>)result).EndInvoke();
        }

        public IAsyncResult Start(AsyncCallback callback, object state)
        {
            this.result = new AsyncResult<TResult>(callback, state);
            this.steps = this.Steps();
            this.moveNext = MoveNext;
            this.MoveNextInner(null);
            return this.result;
        }

        protected abstract IEnumerator<Step> Steps();

        private static void MoveNext(IAsyncResult result)
        {
            if (!result.CompletedSynchronously)
            {
                ((AsyncOperation<TResult>)result.AsyncState).MoveNextInner(result);
            }
        }

        private void MoveNextInner(IAsyncResult result)
        {
            try
            {
                if (result != null)
                {
                    this.anyStepCompletedAsync = true;
                    this.currentStep.EndInvoke(result);
                }

                while (this.steps.MoveNext())
                {
                    this.currentStep = this.steps.Current;
                    IAsyncResult nextResult = this.currentStep.BeginInvoke(this.moveNext, this);
                    if (nextResult == null)
                    {
                        // No-op; exception was caught and handled.
                    }
                    else if (nextResult.CompletedSynchronously)
                    {
                        this.currentStep.EndInvoke(nextResult);
                    }
                    else
                    {
                        return;
                    }
                }

                this.result.SetAsCompleted(this.Result, !this.anyStepCompletedAsync);
            }
            catch (Exception e)
            {
                using (this.steps)
                {
                    if (result == null)
                    {
                        throw;
                    }
                    else
                    {
                        this.result.SetAsCompleted(e, !this.anyStepCompletedAsync);
                    }
                }
            }
        }

        protected struct Step
        {
            private readonly Func<AsyncCallback, object, IAsyncResult> begin;
            private readonly Action<IAsyncResult> end;

            private Step(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end)
            {
                this.begin = begin;
                this.end = end;
            }

            public static Step Await<TState>(TState state, Func<TState, AsyncCallback, object, IAsyncResult> begin, Action<TState, IAsyncResult> end)
            {
                return Await(state, begin, end, null);
            }

            public static Step Await<TState>(TState state, Func<TState, AsyncCallback, object, IAsyncResult> begin, Action<TState, IAsyncResult> end, IExceptionHandler handler)
            {
                return new AsyncCall<TState>(state, begin, end, handler).Step;
            }

            public IAsyncResult BeginInvoke(AsyncCallback callback, object state)
            {
                return this.begin(callback, state);
            }

            public void EndInvoke(IAsyncResult result)
            {
                this.end(result);
            }

            private sealed class AsyncCall<TState>
            {
                private readonly TState state;
                private readonly Func<TState, AsyncCallback, object, IAsyncResult> begin;
                private readonly Action<TState, IAsyncResult> end;
                private readonly IExceptionHandler handler;

                public AsyncCall(TState state, Func<TState, AsyncCallback, object, IAsyncResult> begin, Action<TState, IAsyncResult> end, IExceptionHandler handler)
                {
                    this.state = state;
                    this.begin = begin;
                    this.end = end;
                    this.handler = handler;
                }

                public Step Step
                {
                    get { return new Step(this.BeginInvoke, this.EndInvoke); }
                }

                public IAsyncResult BeginInvoke(AsyncCallback callback, object state)
                {
                    try
                    {
                        return this.begin(this.state, callback, state);
                    }
                    catch (Exception e)
                    {
                        if ((this.handler != null) && this.handler.Handle(e))
                        {
                            return null;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                public void EndInvoke(IAsyncResult result)
                {
                    this.end(this.state, result);
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
