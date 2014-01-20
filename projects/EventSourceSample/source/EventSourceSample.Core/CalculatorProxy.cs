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
            return OpenAsync(this.factory);
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
            return Task.Factory.FromAsync(
                (c, s) => ((ICommunicationObject)s).BeginClose(c, s),
                r => ((ICommunicationObject)r.AsyncState).EndClose(r),
                this.factory);
        }

        private static Task OpenAsync(ICommunicationObject commObj)
        {
            return Task.Factory.FromAsync(
                (c, s) => ((ICommunicationObject)s).BeginOpen(c, s),
                r => ((ICommunicationObject)r.AsyncState).EndOpen(r),
                commObj);
        }

        private async Task ConnectAsync()
        {
            if (this.coreProxy == null)
            {
                this.coreProxy = this.factory.CreateChannel();
                await OpenAsync((ICommunicationObject)this.coreProxy);
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
