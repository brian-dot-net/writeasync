//-----------------------------------------------------------------------
// <copyright file="ProxyInvoker.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Threading.Tasks;

    public class ProxyInvoker<TProxy>
    {
        private readonly IConnectionManager<TProxy> connectionManager;

        public ProxyInvoker(IConnectionManager<TProxy> connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        public async Task<TResult> InvokeAsync<TResult>(Func<TProxy, Task<TResult>> doAsync)
        {
            await this.connectionManager.ConnectAsync();
            TResult result = await doAsync(this.connectionManager.Proxy);
            return result;
        }
    }
}
