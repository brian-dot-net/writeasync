//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    internal sealed class Program
    {
        private static Stopwatch stopwatch;

        private static void Main(string[] args)
        {
            stopwatch = Stopwatch.StartNew();
            List<ProcessData> processData = new List<ProcessData>();
            using (KernelProcessSession session = new KernelProcessSession("TraceSample"))
            using (ProcessTracker tracker = new ProcessTracker(session))
            {
                tracker.ProcessStopped += (o, e) => processData.Add(e.Data);

                Log("Starting session...");
                session.Start();
                Log("Running.");
                Thread.Sleep(TimeSpan.FromSeconds(10.0d));
                Log("Stopping session...");
                session.Stop();
                Log("Stopped.");
            }

            Log("Dumping event data...");

            foreach (ProcessData data in processData)
            {
                Console.WriteLine(
                    "Process ID {0} with image name '{1}' started at {2} and exited at {3} with code {4}.",
                    data.Id,
                    data.Name,
                    data.StartTime,
                    data.ExitTime,
                    data.ExitCode);
            }
        }

        private static void Log(string format, params object[] args)
        {
            string message = format;
            if ((args != null) && (args.Length > 0))
            {
                message = string.Format(CultureInfo.InvariantCulture, format, args);
            }

            Console.WriteLine("[{0:000.000}] {1}", stopwatch.Elapsed.TotalSeconds, message);
        }
    }
}
