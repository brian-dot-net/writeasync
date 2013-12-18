//-----------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System.Threading;
    using System.Threading.Tasks;

    public class Receiver
    {
        private readonly bool useDedicatedThread;

        public Receiver(bool useDedicatedThread)
        {
            this.useDedicatedThread = useDedicatedThread;
        }

        public Task ReceiveAsync(CancellationToken token)
        {
            if (this.useDedicatedThread)
            {
                return Task.Factory.StartNew(() => this.ReceiveInner(token), TaskCreationOptions.LongRunning);
            }
            else
            {
                return this.ReceiveInnerAsync(token);
            }
        }

        private void ReceiveInner(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                Thread.Sleep(1000);
            }
        }

        private async Task ReceiveInnerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                await Task.Delay(1000);
            }
        }
    }
}
