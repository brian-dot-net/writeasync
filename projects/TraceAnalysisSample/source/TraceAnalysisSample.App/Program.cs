//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;

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
            EventCollector collector = new EventCollector();
            DateTime startTime = DateTime.MinValue;
            collector.WindowClosed += (o, e) => OnWindowClosed(ref startTime, e.Window);
            Console.WriteLine("Time,Pending,Completed");
            reader.ReadAsync(collector).Wait();

            return 0;
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
