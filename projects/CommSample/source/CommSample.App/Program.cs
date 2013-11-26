//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Logger logger = new Logger();
            TimeSpan duration = TimeSpan.FromSeconds(5.0d);
            int senderCount = 2;
            logger.WriteLine("Receive loop with {0} senders, {1:0.0} sec...", senderCount, duration.TotalSeconds);
            Receive_loop_with_N_senders(logger, senderCount, duration);
            logger.WriteLine("Done.");
        }

        private static void Receive_loop_with_N_senders(Logger logger, int senderCount, TimeSpan duration)
        {
            MemoryChannel channel = new MemoryChannel();
            int[] sentDataSizes = new int[] { 11, 19, 29, 41, 53, 71 };

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                DataOracle oracle = new DataOracle();
                Sender[] senders = new Sender[senderCount];
                for (int i = 0; i < senders.Length; ++i)
                {
                    int bufferSize = sentDataSizes[i % sentDataSizes.Length];
                    byte fill = (byte)(i + 1);
                    senders[i] = new Sender(channel, logger, bufferSize, fill, new Delay(2, 1));
                    oracle.AddPattern(fill, bufferSize);
                }

                Receiver receiver = new Receiver(channel, logger, 16);
                receiver.DataReceived += delegate(object sender, DataEventArgs e)
                {
                };

                Task[] senderTasks = new Task[senders.Length];
                for (int i = 0; i < senderTasks.Length; ++i)
                {
                    senderTasks[i] = senders[i].RunAsync(cts.Token);
                }

                Task receiverTask = receiver.RunAsync();

                Thread.Sleep(duration);

                cts.Cancel();
                Task.WaitAll(senderTasks);

                channel.Dispose();
                receiverTask.Wait();
            }
        }
    }
}
