//-----------------------------------------------------------------------
// <copyright file="CalculatorProxy.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Threading.Tasks;

    public sealed class CalculatorProxy : IDisposable
    {
        private readonly ConnectionManager<ICalculatorClientAsync> connectionManager;

        public CalculatorProxy(CalculatorChannelFactory factory)
        {
            this.connectionManager = new ConnectionManager<ICalculatorClientAsync>(factory);
        }

        public async Task<TResult> InvokeAsync<TResult>(Func<ICalculatorClientAsync, Task<TResult>> doAsync)
        {
            try
            {
                await this.connectionManager.ConnectAsync();
                TResult result = await doAsync(this.connectionManager.Proxy);
                return result;
            }
            catch (Exception)
            {
                this.connectionManager.Invalidate();
                throw;
            }
        }

        public void Dispose()
        {
            this.connectionManager.Invalidate();
        }
    }
}
