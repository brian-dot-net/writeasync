//-----------------------------------------------------------------------
// <copyright file="ConnectionWithEventsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Threading.Tasks;
    using Xunit;

    public class ConnectionWithEventsTest
    {
        public ConnectionWithEventsTest()
        {
        }

        private interface IMyProxy
        {
            void Stub();
        }

        [Fact]
        public void Open_with_events_calls_inner()
        {
            ConnectionStub<IMyProxy> inner = new ConnectionStub<IMyProxy>();
            ConnectionWithEvents<IMyProxy> outer = new ConnectionWithEvents<IMyProxy>(inner);

            Task task = outer.OpenAsync();
            
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(1, inner.OpenCount);
        }
    }
}
