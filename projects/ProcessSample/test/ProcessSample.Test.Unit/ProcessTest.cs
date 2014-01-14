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
            ProcessExitStub inner = new ProcessExitStub();
            Assert.Equal(0, inner.ExitedSubscriberCount);
            Assert.False(inner.EnableRaisingEvents);

            ProcessExitWatcher process = new ProcessExitWatcher(inner);
            Assert.Same(inner, process.Inner);
            Assert.Equal(1, inner.ExitedSubscriberCount);
            Assert.True(inner.EnableRaisingEvents);
        }

        [Fact]
        public void Unsubscribes_from_Exited_event_on_Dispose()
        {
            ProcessExitStub inner = new ProcessExitStub();

            using (ProcessExitWatcher process = new ProcessExitWatcher(inner))
            {
            }

            Assert.Equal(0, inner.ExitedSubscriberCount);
            Assert.False(inner.EnableRaisingEvents);
        }

        [Fact]
        public void WaitForExit_completes_after_exit()
        {
            ProcessExitStub inner = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(inner);

            Task task = process.WaitForExitAsync(CancellationToken.None);
            Assert.False(task.IsCompleted);

            inner.RaiseExited();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_completes_sync_if_exited_before_subscribed()
        {
            ProcessExitStub inner = new ProcessExitStub();
            inner.HasExited = true;
            ProcessExitWatcher process = new ProcessExitWatcher(inner);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void Race_with_exit_and_subscribe_does_not_cause_errors()
        {
            ProcessExitStub inner = new ProcessExitStub();
            inner.Subscribed += delegate(object sender, EventArgs e)
            {
                inner.RaiseExited();
                inner.HasExited = true;
            };

            ProcessExitWatcher process = new ProcessExitWatcher(inner);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_is_canceled_if_token_requests_cancellation()
        {
            ProcessExitStub inner = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(inner);

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task task = process.WaitForExitAsync(cts.Token);
                Assert.False(task.IsCompleted);

                cts.Cancel();

                Assert.True(task.IsCanceled);
            }
        }

        [Fact]
        public void WaitForExit_is_canceled_immediately_if_token_is_already_canceled()
        {
            ProcessExitStub inner = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(inner);

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                cts.Cancel();
                Task task = process.WaitForExitAsync(cts.Token);

                Assert.True(task.IsCanceled);
            }
        }

        [Fact]
        public void WaitForExit_completes_successfully_if_already_exited_even_if_token_is_already_canceled()
        {
            ProcessExitStub inner = new ProcessExitStub();
            inner.HasExited = true;
            ProcessExitWatcher process = new ProcessExitWatcher(inner);

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                cts.Cancel();
                Task task = process.WaitForExitAsync(cts.Token);

                Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            }
        }

        private sealed class ProcessExitStub : IProcessExit
        {
            public ProcessExitStub()
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
