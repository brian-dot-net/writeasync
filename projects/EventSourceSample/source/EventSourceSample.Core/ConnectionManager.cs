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
        private readonly IFactory<IConnection<TProxy>> factory;

        public ConnectionManager(IFactory<IConnection<TProxy>> factory)
        {
            this.factory = factory;
        }

        public Task ConnectAsync()
        {
            IConnection<TProxy> connection = this.factory.Create();
            return connection.OpenAsync();
        }
    }
}
