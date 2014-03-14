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

        protected AsyncOperation()
        {
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
            while (this.steps.MoveNext())
            {
                Step step = this.steps.Current;
                IAsyncResult result = step.BeginInvoke();
                step.EndInvoke(result);
            }

            this.result.SetAsCompleted(this.Result, true);
            return this.result;
        }

        protected abstract IEnumerator<Step> Steps();

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
                return new AsyncCall<TState>(state, begin, end).Step;
            }

            public IAsyncResult BeginInvoke()
            {
                return this.begin(null, null);
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

                public AsyncCall(TState state, Func<TState, AsyncCallback, object, IAsyncResult> begin, Action<TState, IAsyncResult> end)
                {
                    this.state = state;
                    this.begin = begin;
                    this.end = end;
                }

                public Step Step
                {
                    get { return new Step(this.BeginInvoke, this.EndInvoke); }
                }

                public IAsyncResult BeginInvoke(AsyncCallback callback, object state)
                {
                    return this.begin(this.state, callback, state);
                }

                public void EndInvoke(IAsyncResult result)
                {
                    this.end(this.state, result);
                }
            }
        }
    }
}
