//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Logger logger = new Logger();
            TimeSpan duration = TimeSpan.FromSeconds(5.0d);
            logger.WriteLine("Receive loop with one sender, {0:0.0} sec...", duration.TotalSeconds);
            Receive_loop_with_one_sender(logger, duration);
            logger.WriteLine("Done.");
        }

        private static void Receive_loop_with_one_sender(Logger logger, TimeSpan duration)
        {
            MemoryChannel channel = new MemoryChannel();

            Receiver receiver = new Receiver(channel, logger, 16);
            Sender sender = new Sender(channel, logger, 16, 1, new Delay(2, 1));

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task receiverTask = receiver.RunAsync();
                Task senderTask = sender.RunAsync(cts.Token);

                Thread.Sleep(duration);

                cts.Cancel();
                senderTask.Wait();

                channel.Dispose();
                receiverTask.Wait();
            }
        }
    }
}
