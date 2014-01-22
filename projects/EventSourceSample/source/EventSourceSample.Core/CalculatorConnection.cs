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
        public CalculatorConnection(ICalculatorClientAsync proxy, ClientEventSource eventSource)
        {
            ICalculatorClientAsync middle = new CalculatorClientWithEvents(proxy, eventSource);
            this.Instance = new CalculatorClientWithActivity(middle, eventSource, Guid.NewGuid());
        }

        public ICalculatorClientAsync Instance { get; private set; }

        private ICommunicationObject Channel
        {
            get { return (ICommunicationObject)this.Instance; }
        }

        public Task OpenAsync()
        {
            return this.Channel.OpenAsync();
        }

        public void Abort()
        {
            this.Channel.Abort();
        }
    }
}
