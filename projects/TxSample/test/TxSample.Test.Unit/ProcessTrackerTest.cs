//-----------------------------------------------------------------------
// <copyright file="ProcessTrackerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
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

        [Fact]
        public void Process_start_then_stop_raises_ProcessStopped()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);
            List<ProcessData> data = new List<ProcessData>();
            tracker.ProcessStopped += delegate(object sender, ProcessDataEventArgs e)
            {
                Assert.Same(tracker, sender);
                data.Add(e.Data);
            };

            ProcessEventArgs e1 = new ProcessEventArgs()
            {
                Id = 101,
                ImageName = @"\A\B\C\X.exe",
                Timestamp = new DateTime(2000, 1, 2, 3, 4, 5, 6)
            };
            parent.RaiseProcessStarted(e1);
            ProcessEventArgs e2 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2001, 2, 3, 4, 5, 6, 7),
                ExitCode = 202
            };
            parent.RaiseProcessStopped(e2);

            Assert.Equal(1, data.Count);
            Assert.Equal(101, data[0].Id);
            Assert.Equal("X.exe", data[0].Name);
            Assert.Equal(new DateTime(2000, 1, 2, 3, 4, 5, 6), data[0].StartTime);
            Assert.Equal(new DateTime(2001, 2, 3, 4, 5, 6, 7), data[0].ExitTime);
            Assert.Equal(202, data[0].ExitCode);
        }

        [Fact]
        public void Two_process_starts_then_two_stops_raise_ProcessStopped_twice()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);
            List<ProcessData> data = new List<ProcessData>();
            tracker.ProcessStopped += delegate(object sender, ProcessDataEventArgs e)
            {
                Assert.Same(tracker, sender);
                data.Add(e.Data);
            };

            ProcessEventArgs e1 = new ProcessEventArgs()
            {
                Id = 101,
                ImageName = @"\A\B\C\X.exe",
                Timestamp = new DateTime(2000, 1, 2, 3, 4, 5, 6)
            };
            parent.RaiseProcessStarted(e1);

            ProcessEventArgs e2 = new ProcessEventArgs()
            {
                Id = 102,
                ImageName = @"\A\B\C\Y.exe",
                Timestamp = new DateTime(2001, 2, 3, 4, 5, 6, 7)
            };
            parent.RaiseProcessStarted(e2);

            ProcessEventArgs e3 = new ProcessEventArgs()
            {
                Id = 102,
                Timestamp = new DateTime(2002, 3, 4, 5, 6, 7, 8),
                ExitCode = 202
            };
            parent.RaiseProcessStopped(e3);

            ProcessEventArgs e4 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2003, 4, 5, 6, 7, 8, 9),
                ExitCode = 303
            };
            parent.RaiseProcessStopped(e4);

            Assert.Equal(2, data.Count);
            Assert.Equal(102, data[0].Id);
            Assert.Equal("Y.exe", data[0].Name);
            Assert.Equal(new DateTime(2001, 2, 3, 4, 5, 6, 7), data[0].StartTime);
            Assert.Equal(new DateTime(2002, 3, 4, 5, 6, 7, 8), data[0].ExitTime);
            Assert.Equal(202, data[0].ExitCode);
            Assert.Equal(101, data[1].Id);
            Assert.Equal("X.exe", data[1].Name);
            Assert.Equal(new DateTime(2000, 1, 2, 3, 4, 5, 6), data[1].StartTime);
            Assert.Equal(new DateTime(2003, 4, 5, 6, 7, 8, 9), data[1].ExitTime);
            Assert.Equal(303, data[1].ExitCode);
        }

        [Fact]
        public void Two_process_starts_with_same_Id_then_process_stop_raises_ProcessStopped_with_later_info()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);
            List<ProcessData> data = new List<ProcessData>();
            tracker.ProcessStopped += delegate(object sender, ProcessDataEventArgs e)
            {
                Assert.Same(tracker, sender);
                data.Add(e.Data);
            };

            ProcessEventArgs e1 = new ProcessEventArgs()
            {
                Id = 101,
                ImageName = @"\A\B\C\X.exe",
                Timestamp = new DateTime(2000, 1, 2, 3, 4, 5, 6)
            };
            parent.RaiseProcessStarted(e1);

            ProcessEventArgs e2 = new ProcessEventArgs()
            {
                Id = 101,
                ImageName = @"\A\B\C\Y.exe",
                Timestamp = new DateTime(2001, 2, 3, 4, 5, 6, 7)
            };
            parent.RaiseProcessStarted(e2);

            ProcessEventArgs e3 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2003, 4, 5, 6, 7, 8, 9),
                ExitCode = 303
            };
            parent.RaiseProcessStopped(e3);

            Assert.Equal(1, data.Count);
            Assert.Equal(101, data[0].Id);
            Assert.Equal("Y.exe", data[0].Name);
            Assert.Equal(new DateTime(2001, 2, 3, 4, 5, 6, 7), data[0].StartTime);
            Assert.Equal(new DateTime(2003, 4, 5, 6, 7, 8, 9), data[0].ExitTime);
            Assert.Equal(303, data[0].ExitCode);
        }

        [Fact]
        public void Process_start_then_two_stops_with_same_Id_raises_ProcessStopped_once_for_first_entry()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);
            List<ProcessData> data = new List<ProcessData>();
            tracker.ProcessStopped += delegate(object sender, ProcessDataEventArgs e)
            {
                Assert.Same(tracker, sender);
                data.Add(e.Data);
            };

            ProcessEventArgs e1 = new ProcessEventArgs()
            {
                Id = 101,
                ImageName = @"\A\B\C\X.exe",
                Timestamp = new DateTime(2000, 1, 2, 3, 4, 5, 6)
            };
            parent.RaiseProcessStarted(e1);

            ProcessEventArgs e2 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2001, 2, 3, 4, 5, 6, 7),
                ExitCode = 202
            };
            parent.RaiseProcessStopped(e2);

            ProcessEventArgs e3 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2002, 3, 4, 5, 6, 7, 8),
                ExitCode = 303
            };
            parent.RaiseProcessStopped(e3);

            Assert.Equal(1, data.Count);
            Assert.Equal(101, data[0].Id);
            Assert.Equal("X.exe", data[0].Name);
            Assert.Equal(new DateTime(2000, 1, 2, 3, 4, 5, 6), data[0].StartTime);
            Assert.Equal(new DateTime(2001, 2, 3, 4, 5, 6, 7), data[0].ExitTime);
            Assert.Equal(202, data[0].ExitCode);
        }

        [Fact]
        public void Process_stop_without_matching_start_does_nothing()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);
            List<ProcessData> data = new List<ProcessData>();
            tracker.ProcessStopped += delegate(object sender, ProcessDataEventArgs e)
            {
                Assert.Same(tracker, sender);
                data.Add(e.Data);
            };

            ProcessEventArgs e1 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2001, 2, 3, 4, 5, 6, 7),
                ExitCode = 202
            };
            parent.RaiseProcessStopped(e1);

            Assert.Equal(0, data.Count);
        }

        [Fact]
        public void Process_start_then_stop_before_subscribe_does_nothing_but_raises_later_events_after_subscribe()
        {
            ProcessEventsImpl parent = new ProcessEventsImpl();
            ProcessTracker tracker = new ProcessTracker(parent);

            ProcessEventArgs e1 = new ProcessEventArgs()
            {
                Id = 101,
                ImageName = @"\A\B\C\X.exe",
                Timestamp = new DateTime(2000, 1, 2, 3, 4, 5, 6)
            };
            parent.RaiseProcessStarted(e1);
            ProcessEventArgs e2 = new ProcessEventArgs()
            {
                Id = 101,
                Timestamp = new DateTime(2001, 2, 3, 4, 5, 6, 7),
                ExitCode = 202
            };
            parent.RaiseProcessStopped(e2);

            List<ProcessData> data = new List<ProcessData>();
            tracker.ProcessStopped += delegate(object sender, ProcessDataEventArgs e)
            {
                Assert.Same(tracker, sender);
                data.Add(e.Data);
            };

            ProcessEventArgs e3 = new ProcessEventArgs()
            {
                Id = 102,
                ImageName = @"\A\B\C\Y.exe",
                Timestamp = new DateTime(2002, 3, 4, 5, 6, 7, 8)
            };
            parent.RaiseProcessStarted(e3);
            ProcessEventArgs e4 = new ProcessEventArgs()
            {
                Id = 102,
                Timestamp = new DateTime(2003, 4, 5, 6, 7, 8, 9),
                ExitCode = 303
            };
            parent.RaiseProcessStopped(e4);

            Assert.Equal(1, data.Count);
            Assert.Equal(102, data[0].Id);
            Assert.Equal("Y.exe", data[0].Name);
            Assert.Equal(new DateTime(2002, 3, 4, 5, 6, 7, 8), data[0].StartTime);
            Assert.Equal(new DateTime(2003, 4, 5, 6, 7, 8, 9), data[0].ExitTime);
            Assert.Equal(303, data[0].ExitCode);
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

            public void RaiseProcessStarted(ProcessEventArgs e)
            {
                this.ProcessStarted(this, e);
            }

            public void RaiseProcessStopped(ProcessEventArgs e)
            {
                this.ProcessStopped(this, e);
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
