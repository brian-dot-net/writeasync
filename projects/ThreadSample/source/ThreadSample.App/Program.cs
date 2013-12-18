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
                Receiver receiver = new Receiver(pipeName, true);
                Sender sender = new Sender(pipeName, true);

                Task receiveTask = receiver.ReceiveAsync(cts.Token);
                Thread.Sleep(1000);

                Task sendTask = sender.SendAsync(cts.Token);

                Thread.Sleep(2000);

                cts.Cancel();

                Task.WaitAll(receiveTask, sendTask);
            }
        }
    }
}
