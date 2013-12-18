//-----------------------------------------------------------------------
// <copyright file="Sender.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using System.Threading.Tasks;

    public class Sender
    {
        private readonly string name;
        private readonly bool useDedicatedThread;
        private readonly TimeSpan delay;

        public Sender(string name, bool useDedicatedThread, TimeSpan delay)
        {
            this.name = name;
            this.useDedicatedThread = useDedicatedThread;
            this.delay = delay;
        }

        public event EventHandler DataSent;

        public Task SendAsync(CancellationToken token)
        {
            if (this.useDedicatedThread)
            {
                return Task.Factory.StartNew(() => this.SendInner(token), TaskCreationOptions.LongRunning);
            }
            else
            {
                return this.SendInnerAsync(token);
            }
        }

        private void SendInner(CancellationToken token)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            IChannelFactory<IDuplexSessionChannel> factory = binding.BuildChannelFactory<IDuplexSessionChannel>();
            factory.Open();
            IDuplexSessionChannel channel = factory.CreateChannel(new EndpointAddress("net.pipe://localhost/" + this.name));
            try
            {
                channel.Open();
                while (!token.IsCancellationRequested)
                {
                    using (Message message = Message.CreateMessage(MessageVersion.Default, "http://tempuri.org"))
                    {
                        channel.Send(message);
                    }

                    this.OnSent();
                }
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {
            }

            channel.Abort();
            factory.Abort();
        }

        private void OnSent()
        {
            EventHandler handler = this.DataSent;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private async Task SendInnerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                await Task.Delay(1000);
            }
        }
    }
}
