//-----------------------------------------------------------------------
// <copyright file="ProcessTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample.Test.Unit
{
    using System;
    using System.Threading;
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

            Task task = process.WaitForExitAsync(CancellationToken.None);
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

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void Race_with_exit_and_subscribe_does_not_cause_errors()
        {
            ProcessStub inner = new ProcessStub();
            inner.Subscribed += delegate(object sender, EventArgs e)
            {
                inner.RaiseExited();
                inner.HasExited = true;
            };

            ProcessEx process = new ProcessEx(inner);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_is_canceled_if_token_requests_cancellation()
        {
            ProcessStub inner = new ProcessStub();
            ProcessEx process = new ProcessEx(inner);

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task task = process.WaitForExitAsync(cts.Token);
                Assert.False(task.IsCompleted);

                cts.Cancel();

                Assert.True(task.IsCanceled);
            }
        }

        private sealed class ProcessStub : IProcess
        {
            public ProcessStub()
            {
            }

            public event EventHandler Subscribed;

            public event EventHandler Exited
            {
                add
                {
                    this.ExitedCore += value;
                    EventHandler handler = this.Subscribed;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }

                remove
                {
                    this.ExitedCore -= value;
                }
            }

            private event EventHandler ExitedCore;

            public bool HasExited { get; set; }

            public bool EnableRaisingEvents { get; set; }

            public int ExitedSubscriberCount
            {
                get
                {
                    int count = 0;
                    if (this.ExitedCore != null)
                    {
                        count = this.ExitedCore.GetInvocationList().Length;
                    }

                    return count;
                }
            }

            public void RaiseExited()
            {
                this.ExitedCore(this, EventArgs.Empty);
            }
        }
    }
}
