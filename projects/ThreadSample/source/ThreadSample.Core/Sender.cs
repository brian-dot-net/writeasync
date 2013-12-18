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
        private readonly IChannelFactory<IDuplexSessionChannel> factory;
        private readonly bool useDedicatedThread;
        private readonly TimeSpan delay;

        private IDuplexSessionChannel channel;

        public Sender(string name, bool useDedicatedThread, TimeSpan delay)
        {
            this.name = name;
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            this.factory = binding.BuildChannelFactory<IDuplexSessionChannel>();
            this.useDedicatedThread = useDedicatedThread;
            this.delay = delay;
        }

        public event EventHandler DataSent;

        public async Task OpenAsync()
        {
            await OpenAsync(this.factory);
            
            this.channel = this.factory.CreateChannel(new EndpointAddress("net.pipe://localhost/" + this.name));

            await OpenAsync(this.channel);
        }

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

        public async Task CloseAsync()
        {
            if (this.channel != null)
            {
                await CloseAsync(this.channel);
                this.channel = null;
            }
        
            await CloseAsync(this.factory);
        }

        private static Task OpenAsync(ICommunicationObject commObj)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ICommunicationObject)s).BeginOpen(c, s),
                r => ((ICommunicationObject)r.AsyncState).EndOpen(r),
                commObj);
        }

        private static async Task CloseAsync(ICommunicationObject commObj)
        {
            try
            {
                await Task.Factory.FromAsync(
                    (c, s) => ((ICommunicationObject)s).BeginClose(c, s),
                    r => ((ICommunicationObject)r.AsyncState).EndClose(r),
                    commObj);
                commObj = null;
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {
            }

            if (commObj != null)
            {
                commObj.Abort();
            }
        }

        private void SendInner(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    using (Message message = Message.CreateMessage(MessageVersion.Default, "http://tempuri.org"))
                    {
                        this.channel.Send(message);
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
