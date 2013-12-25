//-----------------------------------------------------------------------
// <copyright file="ProcessTracker.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample
{
    using System;
    using System.Threading.Tasks;

    public sealed class ProcessTracker : IDisposable
    {
        private readonly IProcessEvents parent;

        public ProcessTracker(IProcessEvents parent)
        {
            this.parent = parent;
            this.parent.ProcessStarted += this.OnProcessStarted;
            this.parent.ProcessStopped += this.OnProcessStopped;
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
                this.parent.ProcessStarted -= this.OnProcessStarted;
                this.parent.ProcessStopped -= this.OnProcessStopped;
            }
        }

        private void OnProcessStarted(object sender, ProcessEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnProcessStopped(object sender, ProcessEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
