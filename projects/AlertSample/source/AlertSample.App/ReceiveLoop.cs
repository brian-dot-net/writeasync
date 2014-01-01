//-----------------------------------------------------------------------
// <copyright file="ReceiveLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ReceiveLoop
    {
        private readonly Logger logger;
        private readonly string name;

        private CancellationTokenSource cts;
        private Task task;

        public ReceiveLoop(Logger logger, string name)
        {
            this.logger = logger;
            this.name = name;
        }

        public void Start()
        {
            this.cts = new CancellationTokenSource();
            this.task = this.RunAsync(this.cts.Token);
        }

        public void Stop()
        {
            using (this.cts)
            {
                this.cts.Cancel();
                this.task.Wait();
            }
        }

        private async Task RunAsync(CancellationToken token)
        {
            Receiver receiver = new Receiver(this.name);
            await receiver.OpenAsync();

            int previous = 0;
            int current = 0;
            while (!token.IsCancellationRequested)
            {
                current = receiver.MessagesReceived;
                this.logger.WriteInfo("Receiver '{0}' received {1} messages (~{2} messages/sec).", this.name, current, current - previous);
                previous = current;
                try
                {
                    await Task.Delay(1000, token);
                }
                catch (OperationCanceledException)
                {
                }
            }

            await receiver.CloseAsync();
        }
    }
}
