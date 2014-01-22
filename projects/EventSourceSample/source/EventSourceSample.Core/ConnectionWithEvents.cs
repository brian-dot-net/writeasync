//-----------------------------------------------------------------------
// <copyright file="ConnectionWithEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Threading.Tasks;

    public class ConnectionWithEvents<TProxy> : IConnection<TProxy>
    {
        private readonly IConnection<TProxy> inner;
        private readonly ClientEventSource eventSource;

        public ConnectionWithEvents(IConnection<TProxy> inner, ClientEventSource eventSource)
        {
            this.inner = inner;
            this.eventSource = eventSource;
        }

        public TProxy Instance
        {
            get { return this.inner.Instance; }
        }

        public async Task OpenAsync()
        {
            try
            {
                this.eventSource.ConnectionOpening();
                await this.inner.OpenAsync();
                this.eventSource.ConnectionOpened();
            }
            catch (Exception e)
            {
                this.eventSource.ConnectionError(e.GetType().FullName, e.Message);
                throw;
            }
        }

        public void Abort()
        {
            this.eventSource.ConnectionAborting();
            this.inner.Abort();
        }
    }
}
