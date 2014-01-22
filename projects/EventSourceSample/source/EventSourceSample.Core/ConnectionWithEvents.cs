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

        public ConnectionWithEvents(IConnection<TProxy> inner)
        {
            this.inner = inner;
        }

        public TProxy Instance
        {
            get { return this.inner.Instance; }
        }

        public Task OpenAsync()
        {
            return this.inner.OpenAsync();
        }

        public void Abort()
        {
            this.inner.Abort();
        }
    }
}
