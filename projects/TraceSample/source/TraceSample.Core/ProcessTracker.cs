//-----------------------------------------------------------------------
// <copyright file="ProcessTracker.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public sealed class ProcessTracker : IDisposable
    {
        private readonly IProcessEvents parent;

        private ProcessData data;

        public ProcessTracker(IProcessEvents parent)
        {
            this.parent = parent;
            this.parent.ProcessStarted += this.OnProcessStarted;
            this.parent.ProcessStopped += this.OnProcessStopped;
        }

        public event EventHandler<ProcessDataEventArgs> ProcessStopped;

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
            this.data = new ProcessData()
            {
                Id = e.Id,
                Name = Path.GetFileName(e.ImageName),
                StartTime = e.Timestamp
            };
        }

        private void OnProcessStopped(object sender, ProcessEventArgs e)
        {
            this.data.ExitCode = e.ExitCode;
            this.data.ExitTime = e.Timestamp;
            this.ProcessStopped(this, new ProcessDataEventArgs(this.data));
        }
    }
}
