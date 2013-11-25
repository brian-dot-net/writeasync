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

        public Receiver(MemoryChannel channel)
        {
            this.channel = channel;
        }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
