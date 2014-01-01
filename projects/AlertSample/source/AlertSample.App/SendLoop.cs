//-----------------------------------------------------------------------
// <copyright file="SendLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class SendLoop
    {
        private readonly Logger logger;
        private readonly string receiverName;

        private CancellationTokenSource cts;
        private Task task;

        public SendLoop(Logger logger, string receiverName)
        {
            this.logger = logger;
            this.receiverName = receiverName;
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
            Sender sender = new Sender(this.receiverName);
            await sender.OpenAsync();

            Task runTask = sender.RunAsync(token);

            int previous = 0;
            int current = 0;
            while (!token.IsCancellationRequested)
            {
                current = sender.MessagesSent;
                this.logger.WriteInfo("Sender sent {0} messages to '{1}' (~{2} messages/sec).", current, this.receiverName, current - previous);
                previous = current;
                try
                {
                    await Task.Delay(1000, token);
                }
                catch (OperationCanceledException)
                {
                }
            }

            await runTask;

            await sender.CloseAsync();
        }
    }
}
