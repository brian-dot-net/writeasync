//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PlaSample
{
    using System;
    using System.Threading;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            CounterCollectorInfo info = new CounterCollectorInfo("MyCounters");

            info.SampleInterval = TimeSpan.FromSeconds(1.0d);
            info.LogFileFormat = LogFileFormat.CommaSeparated;
            info.OutputPath = Environment.CurrentDirectory;

            info.CounterNames.Add(new CounterName() { Category = "Process", Counter = "Thread Count", Instance = "explorer" });
            info.CounterNames.Add(new CounterName() { Category = "System", Counter = "System Calls/sec" });
            info.CounterNames.Add(new CounterName() { Category = "Processor", Counter = "Interrupts/sec", Instance = "_Total" });

            ICollectorSet collector = info.Create();
            collector.Start();

            Thread.Sleep(5000);

            collector.Stop();

            collector.Delete();
        }
    }
}
