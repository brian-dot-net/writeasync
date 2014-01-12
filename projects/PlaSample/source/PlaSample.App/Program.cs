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
            CreateRealTimeTraceCollector();
            CreatePerfCounterAlert();
            CreateTraceCollector();
            CreatePerfCounterCollector();
        }

        private static void CreateRealTimeTraceCollector()
        {
            RealTimeTraceCollectorInfo info = new RealTimeTraceCollectorInfo("MyRealTimeCollector");

            // Microsoft-Windows-Kernel-Process         
            Guid providerId = new Guid("{22FB2CD6-0E7B-422B-A0C7-2FAD1FD0E716}");

            info.Providers.Add(new ProviderInfo(providerId) { Level = 5 });

            ISessionController controller = info.Create();
            controller.Start();

            Thread.Sleep(5000);

            controller.Stop();
        }

        private static void CreatePerfCounterAlert()
        {
            CounterAlertInfo info = new CounterAlertInfo("MyAlert");

            info.SampleInterval = TimeSpan.FromSeconds(2.0d);

            CounterName counterName = new CounterName() { Category = "Process", Counter = "% Processor Time", Instance = "notepad" };
            info.Thresholds.Add(new CounterThreshold() { Name = counterName, Condition = ThresholdCondition.Below, Value = 5.0d });

            ICollectorSet collector = info.Create();
            collector.Start();

            Thread.Sleep(5000);

            collector.Stop();

            collector.Delete();
        }

        private static void CreatePerfCounterCollector()
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

        private static void CreateTraceCollector()
        {
            TraceCollectorInfo info = new TraceCollectorInfo("MyTraces");

            info.BufferSizeInKB = 64;
            info.Segmented = true;
            info.MaxDuration = TimeSpan.FromSeconds(1.0d);
            info.OutputPath = Environment.CurrentDirectory;

            // Microsoft-Windows-Kernel-Process         
            Guid providerId = new Guid("{22FB2CD6-0E7B-422B-A0C7-2FAD1FD0E716}");

            info.Providers.Add(new ProviderInfo(providerId) { Level = 5 });

            ICollectorSet collector = info.Create();
            collector.Start();

            Thread.Sleep(5000);

            collector.Stop();

            collector.Delete();
        }
    }
}
