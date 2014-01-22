//-----------------------------------------------------------------------
// <copyright file="ConnectionManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Threading.Tasks;

    public class ConnectionManager<TProxy>
    {
        private readonly IFactory<Uri, IConnection<TProxy>> factory;

        public ConnectionManager(IFactory<Uri, IConnection<TProxy>> factory)
        {
            this.factory = factory;
        }

        public Task ConnectAsync(Uri address)
        {
            IConnection<TProxy> connection = this.factory.Create(address);
            return connection.OpenAsync();
        }
    }
}
