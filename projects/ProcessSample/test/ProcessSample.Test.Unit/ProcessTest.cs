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
        public void Constructor_sets_EnableRaisingEvents_and_subscribes_to_Exited()
        {
            ProcessExitStub subject = new ProcessExitStub();
            Assert.Equal(0, subject.ExitedSubscriberCount);
            Assert.False(subject.EnableRaisingEvents);

            ProcessExitWatcher process = new ProcessExitWatcher(subject);
            Assert.Equal(1, subject.ExitedSubscriberCount);
            Assert.True(subject.EnableRaisingEvents);
        }

        [Fact]
        public void Constructor_throws_ArgumentNull_for_null_subject()
        {
            IProcessExit subject = null;
            ArgumentNullException ane = Assert.Throws<ArgumentNullException>(() => new ProcessExitWatcher(subject));
            Assert.Equal("subject", ane.ParamName);
        }

        [Fact]
        public void Status_provides_access_to_inner_status()
        {
            ProcessExitStub subject = new ProcessExitStub();

            ProcessExitWatcher process = new ProcessExitWatcher(subject);

            subject.ExitCode = 123;
            subject.ExitTime = new DateTime(2000, 1, 2);

            Assert.Equal(123, process.Status.ExitCode);
            Assert.Equal(new DateTime(2000, 1, 2), process.Status.ExitTime);
        }

        [Fact]
        public void Dispose_Unsubscribes_from_Exited_and_resets_EnableRaisingEvents()
        {
            ProcessExitStub subject = new ProcessExitStub();

            using (ProcessExitWatcher process = new ProcessExitWatcher(subject))
            {
            }

            Assert.Equal(0, subject.ExitedSubscriberCount);
            Assert.False(subject.EnableRaisingEvents);
        }

        [Fact]
        public void Dispose_does_not_change_EnableRaisingEvents_if_started_as_true()
        {
            ProcessExitStub subject = new ProcessExitStub();
            subject.EnableRaisingEvents = true;

            using (ProcessExitWatcher process = new ProcessExitWatcher(subject))
            {
            }

            Assert.True(subject.EnableRaisingEvents);
        }

        [Fact]
        public void Dispose_is_idempotent()
        {
            ProcessExitStub subject = new ProcessExitStub();

            ProcessExitWatcher process = new ProcessExitWatcher(subject);
            process.Dispose();

            subject.EnableRaisingEvents = true;
            process.Dispose();

            Assert.True(subject.EnableRaisingEvents);
        }

        [Fact]
        public void WaitForExit_completes_after_exit()
        {
            ProcessExitStub subject = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(subject);

            Task task = process.WaitForExitAsync(CancellationToken.None);
            Assert.False(task.IsCompleted);

            subject.RaiseExited();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_completes_sync_if_exited_before_subscribed()
        {
            ProcessExitStub subject = new ProcessExitStub();
            subject.HasExited = true;
            ProcessExitWatcher process = new ProcessExitWatcher(subject);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_after_Dispose_throws_ObjectDisposed()
        {
            ProcessExitStub subject = new ProcessExitStub();

            ProcessExitWatcher process = new ProcessExitWatcher(subject);
            process.Dispose();

            ObjectDisposedException ode = Assert.Throws<ObjectDisposedException>(() => process.WaitForExitAsync(CancellationToken.None));
            Assert.Equal("ProcessExitWatcher", ode.ObjectName);
        }

        [Fact]
        public void Race_with_exit_and_subscribe_does_not_cause_errors()
        {
            ProcessExitStub subject = new ProcessExitStub();
            subject.Subscribed += delegate(object sender, EventArgs e)
            {
                subject.RaiseExited();
                subject.HasExited = true;
            };

            ProcessExitWatcher process = new ProcessExitWatcher(subject);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_is_canceled_if_token_requests_cancellation()
        {
            ProcessExitStub subject = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(subject);

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
            ProcessExitStub subject = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(subject);

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
            ProcessExitStub subject = new ProcessExitStub();
            subject.HasExited = true;
            ProcessExitWatcher process = new ProcessExitWatcher(subject);

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                cts.Cancel();
                Task task = process.WaitForExitAsync(cts.Token);

                Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            }
        }

        [Fact]
        public void WaitForExit_completes_with_ObjectDisposed_after_Dispose()
        {
            ProcessExitStub subject = new ProcessExitStub();
            Task task;
            using (ProcessExitWatcher process = new ProcessExitWatcher(subject))
            {
                task = process.WaitForExitAsync(CancellationToken.None);
                Assert.False(task.IsCompleted);
            }

            Assert.True(task.IsFaulted);
            AggregateException ae = Assert.IsType<AggregateException>(task.Exception);
            Assert.Equal(1, ae.InnerExceptions.Count);
            ObjectDisposedException ode = Assert.IsType<ObjectDisposedException>(ae.InnerExceptions[0]);
            Assert.Equal("ProcessExitWatcher", ode.ObjectName);
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

            public int ExitCode { get; set; }

            public DateTime ExitTime { get; set; }

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
