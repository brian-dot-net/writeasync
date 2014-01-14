//-----------------------------------------------------------------------
// <copyright file="ProcessExit.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;
    using System.Diagnostics;

    public class ProcessExit : IProcessExit
    {
        private readonly Process process;

        public ProcessExit(Process process)
        {
            this.process = process;
        }

        public event EventHandler Exited
        {
            add { this.process.Exited += value; }
            remove { this.process.Exited -= value; }
        }

        public bool HasExited
        {
            get { return this.process.HasExited; }
        }

        public bool EnableRaisingEvents
        {
            get { return this.process.EnableRaisingEvents; }
            set { this.process.EnableRaisingEvents = value; }
        }

        public int ExitCode
        {
            get { return this.process.ExitCode; }
        }

        public DateTime ExitTime
        {
            get { return this.process.ExitTime; }
        }
    }
}
