//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            string pipeName = Guid.NewGuid().ToString("N");
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                StatusInfo info = new StatusInfo();
                Receiver receiver = new Receiver(pipeName, false);
                receiver.DataReceived += (o, e) => info.OnReceived();
                Sender sender = new Sender(pipeName, false, TimeSpan.FromSeconds(0.1d));
                sender.DataSent += (o, e) => info.OnSent();

                Task statusTask = PrintStatusAsync(info, cts.Token);

                Task receiveTask = receiver.ReceiveAsync(cts.Token);
                
                Thread.Sleep(1000);

                sender.OpenAsync().Wait();

                List<Task> sendTasks = new List<Task>();
                for (int i = 0; i < 100000; ++i)
                {
                    Task sendTask = sender.SendAsync(cts.Token);
                    info.OnSenderAdded();
                    sendTasks.Add(sendTask);
                    if (i % 100 == 0)
                    {
                        Thread.Sleep(1);
                    }
                }

                Thread.Sleep(1000);

                cts.Cancel();

                Task.WaitAll(sendTasks.ToArray());
                statusTask.Wait();
                receiveTask.Wait();

                sender.CloseAsync().Wait();

                PrintStatus(info);
            }
        }

        private static void PrintStatus(StatusInfo info)
        {
            Console.WriteLine("[{0:000.000}] Senders: {1:000000} / Sent: {2:00000000} / Received: {3:00000000}", info.Elapsed.TotalSeconds, info.SenderCount, info.BytesSent, info.BytesReceived);
        }

        private static async Task PrintStatusAsync(StatusInfo info, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                PrintStatus(info);
                try
                {
                    await Task.Delay(1000, token);
                }
                catch (OperationCanceledException)
                {
                }
            }
        }
    }
}
