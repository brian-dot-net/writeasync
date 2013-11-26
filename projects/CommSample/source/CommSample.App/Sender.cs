//-----------------------------------------------------------------------
// <copyright file="Sender.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Sender
    {
        private readonly MemoryChannel channel;
        private readonly Logger logger;
        private readonly int bufferSize;
        private readonly Delay delay;
        private readonly byte fill;

        public Sender(MemoryChannel channel, Logger logger, int bufferSize, byte fill, Delay delay)
        {
            this.channel = channel;
            this.logger = logger;
            this.bufferSize = bufferSize;
            this.fill = fill;
            this.delay = delay;
        }

        public async Task<long> RunAsync(CancellationToken token)
        {
            this.logger.WriteLine("Sender B={0}/F=0x{1:x} starting...", this.bufferSize, this.fill);
            long totalBytes = 0;
            await Task.Factory.StartNew(() => totalBytes = this.RunInner(token), TaskCreationOptions.LongRunning);
            return totalBytes;
        }

        private long RunInner(CancellationToken token)
        {
            long totalBytes = 0;
            byte[] buffer = new byte[this.bufferSize];
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = this.fill;
            }

            while (!token.IsCancellationRequested)
            {
                this.channel.Send(buffer);
                totalBytes += buffer.Length;
                this.delay.Next();
            }

            this.logger.WriteLine("Sender B={0}/F=0x{1:x} completed. Sent {2} bytes.", this.bufferSize, this.fill, totalBytes);
            return totalBytes;
        }
    }
}
