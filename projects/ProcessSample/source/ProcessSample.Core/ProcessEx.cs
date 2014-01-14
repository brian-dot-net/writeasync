//-----------------------------------------------------------------------
// <copyright file="ProcessEx.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;
    using System.Threading.Tasks;

    public sealed class ProcessEx : IDisposable
    {
        private readonly TaskCompletionSource<bool> exited;
        private readonly IProcess inner;

        public ProcessEx(IProcess inner)
        {
            this.exited = new TaskCompletionSource<bool>();
            this.inner = inner;
            this.inner.EnableRaisingEvents = true;
            this.inner.Exited += this.OnProcessExited;
            if (this.inner.HasExited)
            {
                this.exited.TrySetResult(false);
            }
        }

        public Task WaitForExitAsync()
        {
            return this.exited.Task;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.inner.EnableRaisingEvents = false;
                this.inner.Exited -= this.OnProcessExited;
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            this.exited.TrySetResult(false);
        }
    }
}
