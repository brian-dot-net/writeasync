//-----------------------------------------------------------------------
// <copyright file="ProcessEx.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;

    public sealed class ProcessEx : IDisposable
    {
        private readonly IProcess inner;

        public ProcessEx(IProcess inner)
        {
            this.inner = inner;
            this.inner.Exited += this.OnProcessExited;
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
                this.inner.Exited -= this.OnProcessExited;
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
        }
    }
}
