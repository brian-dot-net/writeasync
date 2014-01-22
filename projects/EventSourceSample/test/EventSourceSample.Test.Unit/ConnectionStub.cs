//-----------------------------------------------------------------------
// <copyright file="ConnectionStub.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Threading.Tasks;

    internal sealed class ConnectionStub<TProxy> : IConnection<TProxy>
    {
        private readonly bool deferCompletion;

        public ConnectionStub()
            : this(false)
        {
        }

        public ConnectionStub(bool deferCompletion)
        {
            this.deferCompletion = deferCompletion;
        }

        public int OpenCount { get; private set; }

        public int AbortCount { get; private set; }

        public TProxy Instance { get; set; }

        public TaskCompletionSource<bool> OpenCall { get; private set; }

        public Task OpenAsync()
        {
            ++this.OpenCount;
            this.OpenCall = new TaskCompletionSource<bool>();
            if (!this.deferCompletion)
            {
                this.OpenCall.SetResult(false);
            }

            return this.OpenCall.Task;
        }

        public void Abort()
        {
            ++this.AbortCount;
        }
    }
}
