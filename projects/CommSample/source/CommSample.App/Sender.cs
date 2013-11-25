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
            throw new NotImplementedException();
        }
    }
}
