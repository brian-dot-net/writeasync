//-----------------------------------------------------------------------
// <copyright file="ReceiveLoopTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ReceiveLoopTest
    {
        private readonly Logger logger;
        private readonly int senderCount;
        private readonly TimeSpan duration;

        public ReceiveLoopTest(Logger logger, int senderCount, TimeSpan duration)
        {
            this.logger = logger;
            this.senderCount = senderCount;
            this.duration = duration;
        }

        public void Run()
        {
            this.logger.WriteLine("Receive loop with {0} senders, {1:0.0} sec...", this.senderCount, this.duration.TotalSeconds);

            MemoryChannel channel = new MemoryChannel();
            int[] sentDataSizes = new int[] { 11, 19, 29, 41, 53, 71 };

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                DataOracle oracle = new DataOracle();
                Sender[] senders = new Sender[this.senderCount];
                for (int i = 0; i < senders.Length; ++i)
                {
                    int bufferSize = sentDataSizes[i % sentDataSizes.Length];
                    byte fill = (byte)(i + 1);
                    senders[i] = new Sender(channel, this.logger, bufferSize, fill, new Delay(2, 1));
                    oracle.AddPattern(fill, bufferSize);
                }

                Receiver receiver = new Receiver(channel, this.logger, 16);
                byte lastSeen = 0;
                int lastCount = 0;
                receiver.DataReceived += delegate(object sender, DataEventArgs e)
                {
                    for (int i = 0; i < e.BytesRead; ++i)
                    {
                        if (lastSeen != e.Buffer[i])
                        {
                            if (lastSeen != 0)
                            {
                                oracle.VerifyLastSeen(lastSeen, lastCount);
                            }

                            lastSeen = e.Buffer[i];
                            lastCount = 0;
                        }

                        ++lastCount;
                    }
                };

                Task<long>[] senderTasks = new Task<long>[senders.Length];
                for (int i = 0; i < senderTasks.Length; ++i)
                {
                    senderTasks[i] = senders[i].RunAsync(cts.Token);
                }

                Task<long> receiverTask = receiver.RunAsync();

                Thread.Sleep(this.duration);

                cts.Cancel();
                Task.WaitAll(senderTasks);

                channel.Dispose();
                receiverTask.Wait();

                long totalSent = 0;
                foreach (Task<long> senderTask in senderTasks)
                {
                    totalSent += senderTask.Result;
                }

                if (totalSent != receiverTask.Result)
                {
                    throw new InvalidOperationException(string.Format(
                        CultureInfo.InvariantCulture,
                        "Data corruption detected; {0} bytes sent but {1} bytes received.",
                        totalSent,
                        receiverTask.Result));
                }
            }

            this.logger.WriteLine("Done.");
        }
    }
}
