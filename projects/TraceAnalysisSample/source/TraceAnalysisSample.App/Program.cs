//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide an ETL file name.");
                return 1;
            }

            TraceReader reader = new TraceReader(args[0]);
            EventLatencyCollector latencyCollector = new EventLatencyCollector();
            List<double> latencies = new List<double>();
            latencyCollector.EventCompleted += (o, e) => latencies.Add(e.Latency.TotalSeconds);

            EventWindowCollector windowCollector = new EventWindowCollector();
            DateTime startTime = DateTime.MinValue;
            windowCollector.WindowClosed += (o, e) => OnWindowClosed(ref startTime, e.Window);

            Console.WriteLine("Window,Pending,Completed");
            reader.ReadAsync(new CompositeEventCollector(windowCollector, latencyCollector)).Wait();
            
            windowCollector.CloseWindow();
            Console.WriteLine();

            CalculatePercentiles(latencies);

            return 0;
        }

        private static void CalculatePercentiles(List<double> values)
        {
            values.Sort();
            double p50 = GetPercentile(values, 0.5d);
            double p80 = GetPercentile(values, 0.8d);
            double p95 = GetPercentile(values, 0.95d);
            double p99 = GetPercentile(values, 0.99d);
            double p99_5 = GetPercentile(values, 0.995d);
            Console.WriteLine("Latency: P50={0:0.000}, P80={1:0.000}, P95={2:0.000}, P99={3:0.000}, P99.5={4:0.000}", p50, p80, p95, p99, p99_5);
        }

        private static double GetPercentile(List<double> sortedValues, double percentile)
        {
            double index = 0.5d + (sortedValues.Count * percentile);
            int k = (int)index;
            double f = index - k;
            return ((1 - f) * sortedValues[k]) + (f * sortedValues[k + 1]);
        }

        private static void OnWindowClosed(ref DateTime startTime, EventWindow window)
        {
            if (startTime == DateTime.MinValue)
            {
                startTime = window.StartTime;
            }

            TimeSpan relativeStartTime = window.StartTime - startTime;
            int pending = window.GetPendingCount(1);
            int completed = window.GetCompletedCount(1);
            Console.WriteLine("{0:0.000},{1},{2}", relativeStartTime.TotalSeconds, pending, completed);
        }
    }
}
