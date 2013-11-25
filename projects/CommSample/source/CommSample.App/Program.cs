﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Logger logger = new Logger();
            MemoryChannel channel = new MemoryChannel();

            Receiver receiver = new Receiver(channel, logger);
            Sender sender = new Sender(channel, logger);

            Task receiverTask = receiver.RunAsync();
            Task senderTask = sender.RunAsync();

            Task.WaitAll(receiverTask, senderTask);

            channel.Dispose();

            logger.WriteLine("Done.");
        }
    }
}
