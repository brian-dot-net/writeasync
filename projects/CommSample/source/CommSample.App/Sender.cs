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

        public Sender(MemoryChannel channel)
        {
            this.channel = channel;
        }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
