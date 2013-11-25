//-----------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading.Tasks;

    internal sealed class Receiver
    {
        private readonly MemoryChannel channel;
        private readonly Logger logger;
        private readonly int bufferSize;

        public Receiver(MemoryChannel channel, Logger logger, int bufferSize)
        {
            this.channel = channel;
            this.logger = logger;
            this.bufferSize = bufferSize;
        }

        public async Task RunAsync()
        {
            this.logger.WriteLine("Receiver starting...");
            byte[] buffer = new byte[this.bufferSize];
            int bytesReceived = await this.channel.ReceiveAsync(buffer);
            this.logger.WriteLine("Receiver completed. Received {0} bytes.", bytesReceived);
        }
    }
}
