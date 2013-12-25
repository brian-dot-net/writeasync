//-----------------------------------------------------------------------
// <copyright file="ProcessTrackerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample.Test.Unit
{
    using System;
    using Xunit;

    public class ProcessTrackerTest
    {
        public ProcessTrackerTest()
        {
        }

        [Fact]
        public void Subscribes_on_construction_and_unsubscribes_on_Dispose()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);

            Assert.Equal(1, parent.ProcessStartedSubscriberCount);
            Assert.Equal(1, parent.ProcessStoppedSubscriberCount);

            tracker.Dispose();

            Assert.Equal(0, parent.ProcessStartedSubscriberCount);
            Assert.Equal(0, parent.ProcessStoppedSubscriberCount);
        }

        private sealed class ProcessEventsImpl : IProcessEvents
        {
            public ProcessEventsImpl()
            {
            }

            public event EventHandler<ProcessEventArgs> ProcessStarted;

            public event EventHandler<ProcessEventArgs> ProcessStopped;

            public int ProcessStartedSubscriberCount
            {
                get { return GetSubscriberCount(this.ProcessStarted); }
            }

            public int ProcessStoppedSubscriberCount
            {
                get { return GetSubscriberCount(this.ProcessStopped); }
            }

            private static int GetSubscriberCount<TEventArgs>(EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
            {
                int count = 0;
                if (handler != null)
                {
                    count = handler.GetInvocationList().Length;
                }

                return count;
            }
        }
    }
}
