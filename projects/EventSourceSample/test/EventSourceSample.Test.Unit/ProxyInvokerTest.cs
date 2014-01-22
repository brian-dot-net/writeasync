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
            Assert.Equal(1, connectionManagerStub.ConnectCount);
        }

        [Fact]
        public void Invoke_connects_and_invokes_method_with_exception_and_invalidates()
        {
            ConnectionManagerStub connectionManagerStub = new ConnectionManagerStub();
            MyProxyStub proxyStub = new MyProxyStub();
            connectionManagerStub.Proxy = proxyStub;
            ProxyInvoker<IMyProxy> invoker = new ProxyInvoker<IMyProxy>(connectionManagerStub);

            InvalidTimeZoneException expectedException = new InvalidTimeZoneException("Expected.");
            proxyStub.Exception = expectedException;
            Task<object> task = invoker.InvokeAsync(p => p.DoAsync(new object()));

            Assert.Equal(TaskStatus.Faulted, task.Status);
            AggregateException ae = Assert.IsType<AggregateException>(task.Exception);
            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.Same(expectedException, ae.InnerExceptions[0]);
            Assert.Equal(1, connectionManagerStub.InvalidateCount);
        }

        [Fact]
        public void Dispose_invalidates_connection()
        {
            ConnectionManagerStub connectionManagerStub = new ConnectionManagerStub();
            ProxyInvoker<IMyProxy> invoker = new ProxyInvoker<IMyProxy>(connectionManagerStub);

            invoker.Dispose();

            Assert.Equal(1, connectionManagerStub.InvalidateCount);
        }

        private sealed class MyProxyStub : IMyProxy
        {
            public MyProxyStub()
            {
            }

            public Exception Exception { get; set; }

            public Task<object> DoAsync(object input)
            {
                TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
                if (this.Exception == null)
                {
                    tcs.SetResult(input);
                }
                else
                {
                    tcs.SetException(this.Exception);
                }

                return tcs.Task;
            }
        }

        private sealed class ConnectionManagerStub : IConnectionManager<IMyProxy>
        {
            public ConnectionManagerStub()
            {
            }

            public IMyProxy Proxy { get; set; }

            public int ConnectCount { get; private set; }

            public int InvalidateCount { get; private set; }

            public Task ConnectAsync()
            {
                ++this.ConnectCount;
                return Task.FromResult(false);
            }

            public void Invalidate()
            {
                ++this.InvalidateCount;
            }
        }
    }
}
