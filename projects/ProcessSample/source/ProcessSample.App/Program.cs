//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            int exitCode;
            DateTime exitTime;

            using (Process process = Process.Start("notepad.exe"))
            using (ProcessExitWatcher watcher = new ProcessExitWatcher(new ProcessExit(process)))
            {
                Console.WriteLine("Waiting for Notepad to exit...");
                watcher.WaitForExitAsync(CancellationToken.None).Wait();
                exitCode = watcher.Status.ExitCode;
                exitTime = watcher.Status.ExitTime;
            }

            Console.WriteLine("Done, exited with code {0} at {1}.", exitCode, exitTime);
        }
    }
}
