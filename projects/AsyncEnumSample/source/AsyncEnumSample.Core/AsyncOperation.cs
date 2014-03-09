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
                steps.Current.Invoke();
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

            public static Step Await<TState>(TState state, Func<TState, Task> doAsync)
            {
                return new AsyncCall<TState>(state, doAsync).Step;
            }

            public Task Invoke()
            {
                return this.doAsync();
            }

            private sealed class AsyncCall<TState>
            {
                private readonly TState state;
                private readonly Func<TState, Task> doAsync;

                public AsyncCall(TState state, Func<TState, Task> doAsync)
                {
                    this.state = state;
                    this.doAsync = doAsync;
                }

                public Step Step
                {
                    get { return new Step(this.DoAsync); }
                }

                private Task DoAsync()
                {
                    return this.doAsync(this.state);
                }
            }
        }
    }
}
