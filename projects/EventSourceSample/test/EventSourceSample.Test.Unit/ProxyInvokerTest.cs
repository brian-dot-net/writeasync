//-----------------------------------------------------------------------
// <copyright file="ProxyInvokerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ProxyInvokerTest
    {
        public ProxyInvokerTest()
        {
        }

        private interface IMyProxy
        {
            Task<object> DoAsync(object input);
        }

        [Fact]
        public void Invoke_connects_and_invokes_method_with_result()
        {
            ConnectionManagerStub connectionManagerStub = new ConnectionManagerStub();
            connectionManagerStub.Proxy = new MyProxyStub();
            ProxyInvoker<IMyProxy> invoker = new ProxyInvoker<IMyProxy>(connectionManagerStub);
            object input = new object();

            Task<object> task = invoker.InvokeAsync(p => p.DoAsync(input));

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Same(input, task.Result);
        }

        private sealed class MyProxyStub : IMyProxy
        {
            public MyProxyStub()
            {
            }

            public Task<object> DoAsync(object input)
            {
                return Task.FromResult(input);
            }
        }

        private sealed class ConnectionManagerStub : IConnectionManager<IMyProxy>
        {
            public ConnectionManagerStub()
            {
            }

            public IMyProxy Proxy { get; set; }

            public int ConnectCount { get; private set; }

            public Task ConnectAsync()
            {
                ++this.ConnectCount;
                return Task.FromResult(false);
            }

            public void Invalidate()
            {
                throw new NotImplementedException();
            }
        }
    }
}
