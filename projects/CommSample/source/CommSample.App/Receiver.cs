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

        public Receiver(MemoryChannel channel, Logger logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        public async Task RunAsync()
        {
            this.logger.WriteLine("Receiver starting...");
            byte[] buffer = new byte[16];
            int bytesReceived = await this.channel.ReceiveAsync(buffer);
            this.logger.WriteLine("Receiver completed. Received {0} bytes.", bytesReceived);
        }
    }
}
