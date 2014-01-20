//-----------------------------------------------------------------------
// <copyright file="CalculatorProxy.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;

    public class CalculatorProxy
    {
        private readonly ChannelFactory<ICalculatorClientAsync> factory;
        private readonly Func<ICalculatorClientAsync, ICalculatorClientAsync> wrapClient;

        private ICalculatorClientAsync coreProxy;
        private ICalculatorClientAsync wrappedProxy;

        public CalculatorProxy(Binding binding, EndpointAddress address, Func<ICalculatorClientAsync, ICalculatorClientAsync> wrapClient)
        {
            this.factory = new ChannelFactory<ICalculatorClientAsync>(binding, address);
            this.wrapClient = wrapClient;
        }

        public Task OpenAsync()
        {
            return this.factory.OpenAsync();
        }

        public async Task<TResult> InvokeAsync<TResult>(Func<ICalculatorClientAsync, Task<TResult>> doAsync)
        {
            try
            {
                await this.ConnectAsync();
                TResult result = await doAsync(this.coreProxy);
                return result;
            }
            catch (Exception)
            {
                this.Invalidate();
                throw;
            }
        }

        public Task CloseAsync()
        {
            return this.factory.CloseAsync();
        }

        private async Task ConnectAsync()
        {
            if (this.coreProxy == null)
            {
                this.coreProxy = this.factory.CreateChannel();
                ICommunicationObject commObj = (ICommunicationObject)this.coreProxy;
                await commObj.OpenAsync();
                this.wrappedProxy = this.wrapClient(this.coreProxy);
            }
        }

        private void Invalidate()
        {
            if (this.coreProxy != null)
            {
                ((ICommunicationObject)this.coreProxy).Abort();
                this.coreProxy = null;
            }
        }
    }
}
