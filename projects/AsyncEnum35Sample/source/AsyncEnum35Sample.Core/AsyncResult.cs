//-----------------------------------------------------------------------
// <copyright file="AsyncResult.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnum35Sample
{
    using System;
    using System.Threading;

    public class AsyncResult<TResult> : IAsyncResult
    {
        private const int StatePending = 0;
        private const int StateCompletedSynchronously = 1;
        private const int StateCompletedAsynchronously = 2;

        private readonly AsyncCallback callback;
        private readonly object state;

        private int completedState;
        private ManualResetEvent waitHandle;
        private Exception exception;
        private TResult result;

        public AsyncResult(AsyncCallback callback, object state)
        {
            this.completedState = StatePending;
            this.callback = callback;
            this.state = state;
        }

        public object AsyncState
        {
            get { return this.state; }
        }

        public bool CompletedSynchronously
        {
            get { return Thread.VolatileRead(ref this.completedState) == StateCompletedSynchronously; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (this.waitHandle == null)
                {
                    bool done = this.IsCompleted;
                    ManualResetEvent mre = new ManualResetEvent(done);
                    if (Interlocked.CompareExchange(ref this.waitHandle, mre, null) != null)
                    {
                        mre.Close();
                    }
                    else
                    {
                        if (!done && this.IsCompleted)
                        {
                            this.waitHandle.Set();
                        }
                    }
                }

                return this.waitHandle;
            }
        }

        public bool IsCompleted
        {
            get { return Thread.VolatileRead(ref this.completedState) != StatePending; }
        }

        public void SetAsCompleted(TResult result, bool completedSynchronously)
        {
            this.result = result;
            this.Complete(null, completedSynchronously);
        }

        public void SetAsCompleted(Exception exception, bool completedSynchronously)
        {
            this.Complete(exception, completedSynchronously);
        }

        public TResult EndInvoke()
        {
            if (!this.IsCompleted)
            {
                this.AsyncWaitHandle.WaitOne();
                this.AsyncWaitHandle.Close();
                this.waitHandle = null;
            }

            if (this.exception != null)
            {
                throw this.exception;
            }

            return this.result;
        }

        private void Complete(Exception exception, bool completedSynchronously)
        {
            this.exception = exception;
            int prevState = Interlocked.Exchange(ref this.completedState, completedSynchronously ? StateCompletedSynchronously : StateCompletedAsynchronously);
            if (prevState != StatePending)
            {
                throw new InvalidOperationException("Result is already completed.");
            }

            if (this.waitHandle != null)
            {
                this.waitHandle.Set();
            }

            if (this.callback != null)
            {
                this.callback(this);
            }
        }
    }
}
