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
            ProcessExitStub exit = new ProcessExitStub();
            Assert.Equal(0, exit.ExitedSubscriberCount);
            Assert.False(exit.EnableRaisingEvents);

            ProcessExitWatcher process = new ProcessExitWatcher(exit);
            Assert.Equal(1, exit.ExitedSubscriberCount);
            Assert.True(exit.EnableRaisingEvents);
        }

        [Fact]
        public void Constructor_throws_ArgumentNull_for_null_exit()
        {
            IProcessExit exit = null;
            ArgumentNullException ane = Assert.Throws<ArgumentNullException>(() => new ProcessExitWatcher(exit));
            Assert.Equal("exit", ane.ParamName);
        }

        [Fact]
        public void Status_provides_access_to_inner_status()
        {
            ProcessExitStub exit = new ProcessExitStub();

            ProcessExitWatcher process = new ProcessExitWatcher(exit);

            exit.ExitCode = 123;
            exit.ExitTime = new DateTime(2000, 1, 2);

            Assert.Equal(123, process.Status.ExitCode);
            Assert.Equal(new DateTime(2000, 1, 2), process.Status.ExitTime);
        }

        [Fact]
        public void Dispose_Unsubscribes_from_Exited_and_resets_EnableRaisingEvents()
        {
            ProcessExitStub exit = new ProcessExitStub();

            using (ProcessExitWatcher process = new ProcessExitWatcher(exit))
            {
            }

            Assert.Equal(0, exit.ExitedSubscriberCount);
            Assert.False(exit.EnableRaisingEvents);
        }

        [Fact]
        public void Dispose_does_not_change_EnableRaisingEvents_if_started_as_true()
        {
            ProcessExitStub exit = new ProcessExitStub();
            exit.EnableRaisingEvents = true;

            using (ProcessExitWatcher process = new ProcessExitWatcher(exit))
            {
            }

            Assert.True(exit.EnableRaisingEvents);
        }

        [Fact]
        public void Dispose_is_idempotent()
        {
            ProcessExitStub exit = new ProcessExitStub();

            ProcessExitWatcher process = new ProcessExitWatcher(exit);
            process.Dispose();

            exit.EnableRaisingEvents = true;
            process.Dispose();

            Assert.True(exit.EnableRaisingEvents);
        }

        [Fact]
        public void WaitForExit_completes_after_exit()
        {
            ProcessExitStub exit = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(exit);

            Task task = process.WaitForExitAsync(CancellationToken.None);
            Assert.False(task.IsCompleted);

            exit.RaiseExited();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_completes_sync_if_exited_before_subscribed()
        {
            ProcessExitStub exit = new ProcessExitStub();
            exit.HasExited = true;
            ProcessExitWatcher process = new ProcessExitWatcher(exit);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_after_Dispose_throws_ObjectDisposed()
        {
            ProcessExitStub exit = new ProcessExitStub();

            ProcessExitWatcher process = new ProcessExitWatcher(exit);
            process.Dispose();

            ObjectDisposedException ode = Assert.Throws<ObjectDisposedException>(() => process.WaitForExitAsync(CancellationToken.None));
            Assert.Equal("ProcessExitWatcher", ode.ObjectName);
        }

        [Fact]
        public void Race_with_exit_and_subscribe_does_not_cause_errors()
        {
            ProcessExitStub exit = new ProcessExitStub();
            exit.Subscribed += delegate(object sender, EventArgs e)
            {
                exit.RaiseExited();
                exit.HasExited = true;
            };

            ProcessExitWatcher process = new ProcessExitWatcher(exit);

            Task task = process.WaitForExitAsync(CancellationToken.None);

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void WaitForExit_is_canceled_if_token_requests_cancellation()
        {
            ProcessExitStub exit = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(exit);

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
            ProcessExitStub exit = new ProcessExitStub();
            ProcessExitWatcher process = new ProcessExitWatcher(exit);

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
            ProcessExitStub exit = new ProcessExitStub();
            exit.HasExited = true;
            ProcessExitWatcher process = new ProcessExitWatcher(exit);

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
            ProcessExitStub exit = new ProcessExitStub();
            Task task;
            using (ProcessExitWatcher process = new ProcessExitWatcher(exit))
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
