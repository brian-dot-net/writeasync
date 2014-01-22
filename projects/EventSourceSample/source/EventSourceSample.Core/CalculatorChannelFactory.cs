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

        public CalculatorChannelFactory(Binding binding, EndpointAddress address)
        {
            this.factory = new ChannelFactory<ICalculatorClientAsync>(binding, address);
        }

        public Task OpenAsync()
        {
            return this.factory.OpenAsync();
        }

        public IConnection<ICalculatorClientAsync> Create()
        {
            return new CalculatorConnection(this.factory.CreateChannel());
        }

        public Task CloseAsync()
        {
            return this.factory.CloseAsync();
        }
    }
}
