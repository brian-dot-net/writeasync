//-----------------------------------------------------------------------
// <copyright file="ChildProcess.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.Diagnostics;

    internal sealed class ChildProcess
    {
        private readonly ProcessStartInfo startInfo;

        private Process process;

        public ChildProcess(string childName)
        {
            this.startInfo = new ProcessStartInfo("AlertSample.App.exe", childName);
            this.startInfo.UseShellExecute = false;
            this.startInfo.RedirectStandardInput = true;
        }

        public void Start()
        {
            if (this.process != null)
            {
                throw new InvalidOperationException("The process was already started.");
            }

            this.process = Process.Start(this.startInfo);
        }

        public void Stop()
        {
            if (this.process != null)
            {
                // Press "ENTER" at the waiting prompt.
                this.process.StandardInput.WriteLine();

                this.process.WaitForExit();
            }
        }
    }
}
