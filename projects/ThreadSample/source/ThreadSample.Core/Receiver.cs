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

        private static Task OpenAsync(ICommunicationObject commObj)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ICommunicationObject)s).BeginOpen(c, s),
                r => ((ICommunicationObject)r.AsyncState).EndOpen(r),
                commObj);
        }

        private static Task<IDuplexSessionChannel> AcceptChannelAsync(IChannelListener<IDuplexSessionChannel> listener)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((IChannelListener<IDuplexSessionChannel>)s).BeginAcceptChannel(c, s),
                r => ((IChannelListener<IDuplexSessionChannel>)r.AsyncState).EndAcceptChannel(r),
                listener);
        }

        private static Task<Message> ReceiveAsync(IInputChannel channel)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((IInputChannel)s).BeginReceive(c, s),
                r => ((IInputChannel)r.AsyncState).EndReceive(r),
                channel);
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
                token.Register(c => ((ICommunicationObject)c).Abort(), channel);
                channel.Open();

                bool sessionClosed = false;
                do
                {
                    using (Message message = channel.Receive())
                    {
                        if (message != null)
                        {
                            this.OnReceived();
                        }
                        else
                        {
                            sessionClosed = true;
                        }
                    }
                }
                while (!sessionClosed);
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {
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
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            IChannelListener<IDuplexSessionChannel> listener = binding.BuildChannelListener<IDuplexSessionChannel>(new Uri("net.pipe://localhost/" + this.name));
            await OpenAsync(listener);
            IDuplexSessionChannel channel = null;
            token.Register(l => ((ICommunicationObject)l).Abort(), listener);
            try
            {
                channel = await AcceptChannelAsync(listener);
                token.Register(c => ((ICommunicationObject)c).Abort(), channel);
                await OpenAsync(channel);

                bool sessionClosed = false;
                do
                {
                    using (Message message = await ReceiveAsync(channel))
                    {
                        if (message != null)
                        {
                            this.OnReceived();
                        }
                        else
                        {
                            sessionClosed = true;
                        }
                    }
                }
                while (!sessionClosed);
            }
            catch (CommunicationException)
            {
            }
            catch (TimeoutException)
            {
            }
        }
    }
}