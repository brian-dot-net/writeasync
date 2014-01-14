//-----------------------------------------------------------------------
// <copyright file="ProcessTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ProcessTest
    {
        public ProcessTest()
        {
        }

        [Fact]
        public void Subscribes_to_Exited_event_on_construction()
        {
            ProcessStub inner = new ProcessStub();
            Assert.Equal(0, inner.ExitedSubscriberCount);
            Assert.False(inner.EnableRaisingEvents);

            ProcessEx process = new ProcessEx(inner);
            Assert.Equal(1, inner.ExitedSubscriberCount);
            Assert.True(inner.EnableRaisingEvents);
        }

        [Fact]
        public void Unsubscribes_from_Exited_event_on_Dispose()
        {
            ProcessStub inner = new ProcessStub();

            using (ProcessEx process = new ProcessEx(inner))
            {
            }

            Assert.Equal(0, inner.ExitedSubscriberCount);
            Assert.False(inner.EnableRaisingEvents);
        }

        [Fact]
        public void WaitForExit_completes_after_exit()
        {
            ProcessStub inner = new ProcessStub();
            ProcessEx process = new ProcessEx(inner);

            Task task = process.WaitForExitAsync();
            Assert.False(task.IsCompleted);

            inner.RaiseExited();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_completes_sync_if_exited_before_subscribed()
        {
            ProcessStub inner = new ProcessStub();
            inner.HasExited = true;
            ProcessEx process = new ProcessEx(inner);

            Task task = process.WaitForExitAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        private sealed class ProcessStub : IProcess
        {
            public ProcessStub()
            {
            }

            public event EventHandler Exited;

            public bool HasExited { get; set; }

            public bool EnableRaisingEvents { get; set; }

            public int ExitedSubscriberCount
            {
                get
                {
                    int count = 0;
                    if (this.Exited != null)
                    {
                        count = this.Exited.GetInvocationList().Length;
                    }

                    return count;
                }
            }

            public void RaiseExited()
            {
                this.Exited(this, EventArgs.Empty);
            }
        }
    }
}
