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
            public Task Invoke()
            {
                return null;
            }
        }
    }
}
