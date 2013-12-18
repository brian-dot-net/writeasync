//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System;
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
                Receiver receiver = new Receiver(pipeName, true);
                receiver.DataReceived += (o, e) => info.OnReceived();
                Sender sender = new Sender(pipeName, true);
                sender.DataSent += (o, e) => info.OnSent();

                Task statusTask = PrintStatusAsync(info, cts.Token);

                Task receiveTask = receiver.ReceiveAsync(cts.Token);
                Thread.Sleep(1000);

                Task sendTask = sender.SendAsync(cts.Token);

                Thread.Sleep(5000);

                cts.Cancel();

                Task.WaitAll(receiveTask, sendTask, statusTask);
            }
        }

        private static async Task PrintStatusAsync(StatusInfo info, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("Sent: {0} / Received: {1}", info.BytesSent, info.BytesReceived);
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
