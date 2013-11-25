//-----------------------------------------------------------------------
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

            Receiver receiver = new Receiver(channel, logger, 16);
            Sender sender = new Sender(channel, logger, 16, 1);

            Task receiverTask = receiver.RunAsync();
            Task senderTask = sender.RunAsync();

            channel.Dispose();

            Task.WaitAll(receiverTask, senderTask);

            logger.WriteLine("Done.");
        }
    }
}
