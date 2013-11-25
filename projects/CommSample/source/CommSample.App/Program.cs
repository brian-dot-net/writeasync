//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Logger logger = new Logger();
            MemoryChannel channel = new MemoryChannel();

            Receiver receiver = new Receiver(channel, logger, 16);
            Sender sender = new Sender(channel, logger, 16, 1, new Delay(2, 1));

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task receiverTask = receiver.RunAsync();
                Task senderTask = sender.RunAsync(cts.Token);

                cts.Cancel();
                senderTask.Wait();

                channel.Dispose();
                receiverTask.Wait();
            }

            logger.WriteLine("Done.");
        }
    }
}
