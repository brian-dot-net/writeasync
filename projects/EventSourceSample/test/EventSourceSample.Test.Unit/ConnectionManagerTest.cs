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

            Uri address = new Uri("urn:test");
            Task task = manager.ConnectAsync(address);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, factoryStub.Inputs.Count);
            Assert.Equal(address, factoryStub.Inputs[0]);
            Assert.Equal(1, connectionStub.OpenCount);
        }

        private sealed class ConnectionStub : IConnection<IMyChannel>
        {
            public ConnectionStub()
            {
            }

            public int OpenCount { get; private set; }

            public IMyChannel Instance
            {
                get { throw new NotImplementedException(); }
            }

            public Task OpenAsync()
            {
                ++this.OpenCount;
                return Task.FromResult(false);
            }
        }

        private sealed class FactoryStub : IFactory<Uri, IConnection<IMyChannel>>
        {
            public FactoryStub()
            {
                this.Channels = new Queue<IConnection<IMyChannel>>();
                this.Inputs = new List<Uri>();
            }

            public Queue<IConnection<IMyChannel>> Channels { get; private set; }

            public IList<Uri> Inputs { get; private set; }

            public IConnection<IMyChannel> Create(Uri input)
            {
                this.Inputs.Add(input);
                return this.Channels.Dequeue();
            }
        }
    }
}
