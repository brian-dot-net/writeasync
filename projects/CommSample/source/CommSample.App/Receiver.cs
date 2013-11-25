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

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
