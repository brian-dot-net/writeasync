//-----------------------------------------------------------------------
// <copyright file="ProcessTracker.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public sealed class ProcessTracker : IDisposable
    {
        private readonly IProcessEvents parent;

        private Dictionary<int, ProcessData> processes;

        public ProcessTracker(IProcessEvents parent)
        {
            this.parent = parent;
            this.parent.ProcessStarted += this.OnProcessStarted;
            this.parent.ProcessStopped += this.OnProcessStopped;
            this.processes = new Dictionary<int, ProcessData>();
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
            ProcessData data = new ProcessData()
            {
                Id = e.Id,
                Name = Path.GetFileName(e.ImageName),
                StartTime = e.Timestamp
            };

            this.processes.Add(data.Id, data);
        }

        private void OnProcessStopped(object sender, ProcessEventArgs e)
        {
            ProcessData data = this.processes[e.Id];
            data.ExitCode = e.ExitCode;
            data.ExitTime = e.Timestamp;

            this.ProcessStopped(this, new ProcessDataEventArgs(data));
        }
    }
}
