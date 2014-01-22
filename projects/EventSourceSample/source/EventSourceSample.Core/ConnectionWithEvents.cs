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
            get { throw new NotImplementedException(); }
        }

        public Task OpenAsync()
        {
            return this.inner.OpenAsync();
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }
    }
}
