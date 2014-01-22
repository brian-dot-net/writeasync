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
        private readonly Guid id;

        public ConnectionWithEvents(IConnection<TProxy> inner, ClientEventSource eventSource, Guid id)
        {
            this.inner = inner;
            this.eventSource = eventSource;
            this.id = id;
        }

        public TProxy Instance
        {
            get { return this.inner.Instance; }
        }

        public async Task OpenAsync()
        {
            try
            {
                this.eventSource.ConnectionOpening(this.id);
                await this.inner.OpenAsync();
                this.eventSource.ConnectionOpened(this.id);
            }
            catch (Exception e)
            {
                this.eventSource.ConnectionError(this.id, e.GetType().FullName, e.Message);
                throw;
            }
        }

        public void Abort()
        {
            this.eventSource.ConnectionAborting(this.id);
            this.inner.Abort();
        }
    }
}
