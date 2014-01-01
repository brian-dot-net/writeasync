//-----------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.ServiceModel;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Receiver
    {
        private readonly ServiceInstance instance;
        private readonly ServiceHost host;

        public Receiver(string name)
        {
            this.instance = new ServiceInstance();
            this.host = new ServiceHost(this.instance, new Uri("net.pipe://localhost/" + name));
            this.host.AddServiceEndpoint(typeof(IClient), new NetNamedPipeBinding(NetNamedPipeSecurityMode.None), string.Empty);
        }

        public int MessagesReceived
        {
            get { return this.instance.MessagesReceived; }
        }

        public Task OpenAsync()
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ServiceHost)s).BeginOpen(c, s),
                r => ((ServiceHost)r.AsyncState).EndOpen(r),
                this.host);
        }

        public Task CloseAsync()
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ServiceHost)s).BeginClose(c, s),
                r => ((ServiceHost)r.AsyncState).EndClose(r),
                this.host);
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        private sealed class ServiceInstance : IClient
        {
            private int messagesReceived;

            public ServiceInstance()
            {
            }

            public int MessagesReceived
            {
                get { return Thread.VolatileRead(ref this.messagesReceived); }
            }

            public void Send()
            {
                Interlocked.Increment(ref this.messagesReceived);
            }
        }
    }
}
