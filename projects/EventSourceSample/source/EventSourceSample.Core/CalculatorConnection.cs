//-----------------------------------------------------------------------
// <copyright file="CalculatorConnection.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;

    public class CalculatorConnection : IConnection<ICalculatorClientAsync>
    {
        private readonly ICommunicationObject channel;

        public CalculatorConnection(ICalculatorClientAsync proxy, ClientEventSource eventSource, Guid id)
        {
            this.channel = (ICommunicationObject)proxy;
            ICalculatorClientAsync middle = new CalculatorClientWithEvents(proxy, eventSource);
            this.Instance = new CalculatorClientWithActivity(middle, eventSource, id);
        }

        public ICalculatorClientAsync Instance { get; private set; }

        public Task OpenAsync()
        {
            return this.channel.OpenAsync();
        }

        public void Abort()
        {
            this.channel.Abort();
        }
    }
}
