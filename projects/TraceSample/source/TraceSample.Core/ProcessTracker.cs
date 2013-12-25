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
            // Note that it is possible (but unlikely) that we already have an entry for
            // a process with this ID. This can happen if the stopped event was lost.
            ProcessData data;
            if (!this.processes.TryGetValue(e.Id, out data))
            {
                data = new ProcessData();
                this.processes.Add(e.Id, data);
            }

            data.Id = e.Id;
            data.Name = Path.GetFileName(e.ImageName);
            data.StartTime = e.Timestamp;
        }

        private void OnProcessStopped(object sender, ProcessEventArgs e)
        {
            // Note that it is possible that we do not have an entry for this process. For
            // example, a process could have started just before we began tracking events.
            ProcessData data;
            if (this.processes.TryGetValue(e.Id, out data))
            {
                this.processes.Remove(e.Id);
                data.ExitCode = e.ExitCode;
                data.ExitTime = e.Timestamp;

                this.ProcessStopped(this, new ProcessDataEventArgs(data));
            }
        }
    }
}
