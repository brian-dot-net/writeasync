//-----------------------------------------------------------------------
// <copyright file="CalculatorChannelFactory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;

    public class CalculatorChannelFactory : IFactory<IConnection<ICalculatorClientAsync>>
    {
        private readonly ChannelFactory<ICalculatorClientAsync> factory;
        private readonly ClientEventSource eventSource;

        public CalculatorChannelFactory(Binding binding, EndpointAddress address, ClientEventSource eventSource)
        {
            this.factory = new ChannelFactory<ICalculatorClientAsync>(binding, address);
            this.eventSource = eventSource;
        }

        public Task OpenAsync()
        {
            return this.factory.OpenAsync();
        }

        public IConnection<ICalculatorClientAsync> Create()
        {
            return new CalculatorConnection(this.factory.CreateChannel(), this.eventSource);
        }

        public Task CloseAsync()
        {
            return this.factory.CloseAsync();
        }
    }
}
