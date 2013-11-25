//-----------------------------------------------------------------------
// <copyright file="Sender.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading.Tasks;

    internal sealed class Sender
    {
        private readonly MemoryChannel channel;
        private readonly Logger logger;

        public Sender(MemoryChannel channel, Logger logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        public Task RunAsync()
        {
            this.logger.WriteLine("Sender starting...");
            return Task.Factory.StartNew(this.RunInner, TaskCreationOptions.LongRunning);
        }

        private void RunInner()
        {
            byte[] buffer = new byte[16];
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)i;
            }

            this.channel.Send(buffer);

            this.logger.WriteLine("Sender completed. Sent {0} bytes.", buffer.Length);
        }
    }
}
