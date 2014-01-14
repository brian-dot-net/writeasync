//-----------------------------------------------------------------------
// <copyright file="ProcessExitWatcher.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class ProcessExitWatcher : IDisposable
    {
        private readonly TaskCompletionSource<bool> exited;
        private readonly IProcessExit exit;

        private bool? savedEnableRaisingEvents;

        public ProcessExitWatcher(IProcessExit exit)
        {
            this.exited = new TaskCompletionSource<bool>();
            this.exit = exit;
            this.savedEnableRaisingEvents = this.exit.EnableRaisingEvents;
            this.exit.EnableRaisingEvents = true;
            this.exit.Exited += this.OnProcessExited;
            if (this.exit.HasExited)
            {
                this.exited.TrySetResult(false);
            }
        }

        public IProcessExit Inner
        {
            get { return this.exit; }
        }

        private bool IsDisposed
        {
            get { return this.savedEnableRaisingEvents == null; }
        }

        public Task WaitForExitAsync(CancellationToken token)
        {
            if (this.IsDisposed)
            {
                throw GetDisposedException();
            }

            Task task = this.exited.Task;
            if (token.CanBeCanceled)
            {
                task = this.WaitForExitInnerAsync(token);
            }

            return task;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static Exception GetDisposedException()
        {
            return new ObjectDisposedException("ProcessExitWatcher");
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.savedEnableRaisingEvents.HasValue)
                {
                    this.exit.EnableRaisingEvents = this.savedEnableRaisingEvents.Value;
                    this.exit.Exited -= this.OnProcessExited;
                    this.exited.TrySetException(GetDisposedException());
                    this.savedEnableRaisingEvents = null;
                }
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            this.exited.TrySetResult(false);
        }

        private async Task WaitForExitInnerAsync(CancellationToken token)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            using (token.Register(o => ((TaskCompletionSource<bool>)o).SetResult(false), tcs))
            {
                Task completed = await Task.WhenAny(this.exited.Task, tcs.Task);
                if (completed == this.exited.Task)
                {
                    await this.exited.Task;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }
            }
        }
    }
}
