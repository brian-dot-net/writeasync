//-----------------------------------------------------------------------
// <copyright file="CalculatorServiceHost.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;

    public class CalculatorServiceHost
    {
        private readonly TimeSpan delay;
        private readonly Binding binding;
        private readonly Uri address;

        private ServiceHost host;

        public CalculatorServiceHost(TimeSpan delay, Binding binding, Uri address)
        {
            this.delay = delay;
            this.binding = binding;
            this.address = address;
        }

        public Task OpenAsync()
        {
            this.host = new ServiceHost(new CalculatorService(this.delay), this.address);
            this.host.AddServiceEndpoint(typeof(ICalculator), this.binding, string.Empty);
            return this.host.OpenAsync();
        }

        public Task RecycleAsync()
        {
            this.host.Abort();
            return this.OpenAsync();
        }

        public Task CloseAsync()
        {
            return this.host.CloseAsync();
        }
    }
}
