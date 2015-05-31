//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static readonly Stopwatch DefaultStopwatch = Stopwatch.StartNew();

        private static void Main(string[] args)
        {
            Log("Started.");
            try
            {
                using (WorldClock worldClock = new WorldClock())
                {
                    RunAsync(worldClock).Wait();
                }
            }
            catch (AggregateException e)
            {
                Log("*** " + e.InnerException.Message);
            }

            Log("Completed.");
        }

        private static async Task RunAsync(WorldClock worldClock)
        {
            DateTime initialTime = await worldClock.GetTimeAsync();
            Log("Time is " + TimeString(initialTime));

            // An infinite loop of successive 100 ms delay tasks
            Func<Task> delayAsync = delegate
            {
                Log("Delaying...");
                return Task.Delay(TimeSpan.FromSeconds(0.1d));
            };

            LoopingScheduler scheduler = new LoopingScheduler(delayAsync);

            Func<DateTime, TimeSpan, Task> throwIfMaxDurationAsync = async delegate(DateTime start, TimeSpan duration)
            {
                Log("Checking time...");
                DateTime now = await worldClock.GetTimeAsync();
                Log("Time is " + TimeString(now));
                if ((now - start) > duration)
                {
                    throw new InvalidOperationException("Max duration reached!");
                }
            };

            // On every pause cycle, check if duration is reached based on world time
            scheduler.Paused += delegate(object sender, AsyncEventArgs e)
            {
                Log("Paused.");
                e.Tasks.Add(() => throwIfMaxDurationAsync(initialTime, TimeSpan.FromSeconds(2.0d)));
            };

            TimeSpan pauseInterval = TimeSpan.FromSeconds(0.5d);
            await scheduler.RunAsync(pauseInterval);
        }

        private static void Log(string message)
        {
            Console.WriteLine("[{0:0.000}]T{1}: {2}", DefaultStopwatch.Elapsed.TotalSeconds, Thread.CurrentThread.ManagedThreadId, message);
        }

        private static string TimeString(DateTime time)
        {
            return time.ToString("HH:mm:ss.fff");
        }
    }
}
