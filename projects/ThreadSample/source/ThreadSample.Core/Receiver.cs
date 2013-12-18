//-----------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Brian Rogers">
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

    public class Receiver
    {
        private readonly string name;
        private readonly bool useDedicatedThread;

        public Receiver(string name, bool useDedicatedThread)
        {
            this.name = name;
            this.useDedicatedThread = useDedicatedThread;
        }

        public event EventHandler DataReceived;

        public Task ReceiveAsync(CancellationToken token)
        {
            if (this.useDedicatedThread)
            {
                return Task.Factory.StartNew(() => this.ReceiveInner(token), TaskCreationOptions.LongRunning);
            }
            else
            {
                return this.ReceiveInnerAsync(token);
            }
        }

        private void ReceiveInner(CancellationToken token)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            IChannelListener<IDuplexSessionChannel> listener = binding.BuildChannelListener<IDuplexSessionChannel>(new Uri("net.pipe://localhost/" + this.name));
            listener.Open();
            IDuplexSessionChannel channel = null;
            token.Register(l => ((ICommunicationObject)l).Abort(), listener);
            try
            {
                channel = listener.AcceptChannel();
                channel.Open();
                while (true)
                {
                    using (Message message = channel.Receive())
                    {
                        this.OnReceived();
                    }
                }
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {
            }

            if (channel != null)
            {
                channel.Abort();
            }
        }

        private void OnReceived()
        {
            EventHandler handler = this.DataReceived;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private async Task ReceiveInnerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                await Task.Delay(1000);
            }
        }
    }
}