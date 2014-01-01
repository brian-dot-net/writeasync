//-----------------------------------------------------------------------
// <copyright file="Sender.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.ServiceModel;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Sender
    {
        private readonly IClientAsync client;

        private int messagesSent;
        private volatile int sendIntervalMilliseconds;

        public Sender(string receiverName)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            this.client = ChannelFactory<IClientAsync>.CreateChannel(binding, new EndpointAddress("net.pipe://localhost/" + receiverName));
        }

        public TimeSpan SendInterval
        {
            get { return TimeSpan.FromMilliseconds(this.sendIntervalMilliseconds); }
            set { this.sendIntervalMilliseconds = (int)value.TotalMilliseconds; }
        }

        public int MessagesSent
        {
            get { return Thread.VolatileRead(ref this.messagesSent); }
        }

        public Task OpenAsync()
        {
            return Task.Factory.FromAsync(
                (c, s) => ((IClientChannel)s).BeginOpen(c, s),
                r => ((IClientChannel)r.AsyncState).EndOpen(r),
                (IClientChannel)this.client);
        }

        public async Task RunAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await this.client.SendAsync();
                Interlocked.Increment(ref this.messagesSent);
                try
                {
                    await Task.Delay(this.sendIntervalMilliseconds, token);
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

        public Task CloseAsync()
        {
            return Task.Factory.FromAsync(
                (c, s) => ((IClientChannel)s).BeginClose(c, s),
                r => ((IClientChannel)r.AsyncState).EndClose(r),
                (IClientChannel)this.client);
        }
    }
}
