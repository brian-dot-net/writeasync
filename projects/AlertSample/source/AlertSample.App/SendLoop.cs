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
        private readonly Sender sender;

        private CancellationTokenSource cts;
        private Task task;

        public SendLoop(Logger logger, string receiverName)
        {
            this.logger = logger;
            this.receiverName = receiverName;
            this.sender = new Sender(receiverName);
        }

        public TimeSpan SendInterval
        {
            get { return this.sender.SendInterval; }
            set { this.sender.SendInterval = value; }
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
            await this.sender.OpenAsync();

            Task runTask = this.sender.RunAsync(token);

            int previous = 0;
            int current = 0;
            while (!token.IsCancellationRequested)
            {
                current = this.sender.MessagesSent;
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

            await this.sender.CloseAsync();
        }
    }
}
