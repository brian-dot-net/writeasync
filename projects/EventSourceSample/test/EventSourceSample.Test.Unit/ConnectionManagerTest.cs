//-----------------------------------------------------------------------
// <copyright file="ConnectionManagerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ConnectionManagerTest
    {
        public ConnectionManagerTest()
        {
        }

        private interface IMyChannel
        {
            void Stub();
        }

        [Fact]
        public void Connect_creates_and_opens_new_connection()
        {
            FactoryStub factoryStub = new FactoryStub();
            ConnectionManager<IMyChannel> manager = new ConnectionManager<IMyChannel>(factoryStub);
            ConnectionStub connectionStub = new ConnectionStub();
            factoryStub.Channels.Enqueue(connectionStub);

            Task task = manager.ConnectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, connectionStub.OpenCount);
        }

        [Fact]
        public void Connect_when_already_connected_is_idempotent()
        {
            FactoryStub factoryStub = new FactoryStub();
            ConnectionManager<IMyChannel> manager = new ConnectionManager<IMyChannel>(factoryStub);
            ConnectionStub connectionStub = new ConnectionStub();
            factoryStub.Channels.Enqueue(connectionStub);

            Task task = manager.ConnectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, connectionStub.OpenCount);

            task = manager.ConnectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, connectionStub.OpenCount);
        }

        [Fact]
        public void Invalidate_aborts_and_allows_subsequent_reconnection()
        {
            FactoryStub factoryStub = new FactoryStub();
            ConnectionManager<IMyChannel> manager = new ConnectionManager<IMyChannel>(factoryStub);
            ConnectionStub connectionStub = new ConnectionStub();
            factoryStub.Channels.Enqueue(connectionStub);

            Task task = manager.ConnectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, connectionStub.OpenCount);

            manager.Invalidate();

            Assert.Equal(1, connectionStub.AbortCount);

            ConnectionStub connectionStub2 = new ConnectionStub();
            factoryStub.Channels.Enqueue(connectionStub2);
            task = manager.ConnectAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, connectionStub2.OpenCount);
        }

        [Fact]
        public void Invalidate_before_connect_does_nothing()
        {
            FactoryStub factoryStub = new FactoryStub();
            ConnectionManager<IMyChannel> manager = new ConnectionManager<IMyChannel>(factoryStub);

            manager.Invalidate();
        }

        [Fact]
        public void Get_proxy_before_connect_throws_InvalidOperation()
        {
            FactoryStub factoryStub = new FactoryStub();
            ConnectionManager<IMyChannel> manager = new ConnectionManager<IMyChannel>(factoryStub);

            Assert.Throws<InvalidOperationException>(() => manager.Proxy);
        }

        private sealed class ConnectionStub : IConnection<IMyChannel>
        {
            public ConnectionStub()
            {
            }

            public int OpenCount { get; private set; }

            public int AbortCount { get; private set; }

            public IMyChannel Instance
            {
                get { throw new NotImplementedException(); }
            }

            public Task OpenAsync()
            {
                ++this.OpenCount;
                return Task.FromResult(false);
            }

            public void Abort()
            {
                ++this.AbortCount;
            }
        }

        private sealed class FactoryStub : IFactory<IConnection<IMyChannel>>
        {
            public FactoryStub()
            {
                this.Channels = new Queue<IConnection<IMyChannel>>();
            }

            public Queue<IConnection<IMyChannel>> Channels { get; private set; }

            public IConnection<IMyChannel> Create()
            {
                return this.Channels.Dequeue();
            }
        }
    }
}
