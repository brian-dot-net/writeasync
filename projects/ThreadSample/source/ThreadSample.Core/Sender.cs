//-----------------------------------------------------------------------
// <copyright file="Sender.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System.Threading;
    using System.Threading.Tasks;

    public class Sender
    {
        private readonly bool useDedicatedThread;

        public Sender(bool useDedicatedThread)
        {
            this.useDedicatedThread = useDedicatedThread;
        }

        public Task SendAsync(CancellationToken token)
        {
            if (useDedicatedThread)
            {
                return Task.Factory.StartNew(() => this.SendInner(token), TaskCreationOptions.LongRunning);
            }
            else
            {
                return SendInnerAsync(token);
            }
        }

        private void SendInner(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                Thread.Sleep(1000);
            }
        }

        private async Task SendInnerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                await Task.Delay(1000);
            }
        }
    }
}
