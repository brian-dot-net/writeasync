//-----------------------------------------------------------------------
// <copyright file="LoopingScheduler.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class LoopingScheduler
    {
        private static readonly Stopwatch DefaultStopwatch = Stopwatch.StartNew();
        private static readonly Func<TimeSpan> InitialGetElapsed = DefaultGetElapsed;

        private readonly Func<Task> doAsync;

        public LoopingScheduler(Func<Task> doAsync)
        {
            this.doAsync = doAsync;
            this.GetElapsed = InitialGetElapsed;
        }

        public event EventHandler Paused;

        public Func<TimeSpan> GetElapsed { get; set; }

        public async Task RunAsync(TimeSpan pauseInterval)
        {
            TimeSpan start = this.GetElapsed();
            while (true)
            {
                await this.doAsync();
                start = this.CheckPauseInterval(start, pauseInterval);
            }
        }

        private static TimeSpan DefaultGetElapsed()
        {
            return DefaultStopwatch.Elapsed;
        }

        private TimeSpan CheckPauseInterval(TimeSpan start, TimeSpan pauseInterval)
        {
            TimeSpan elapsed = this.GetElapsed() - start;
            if (elapsed >= pauseInterval)
            {
                start = elapsed;
                this.Raise(this.Paused);
            }

            return start;
        }

        private void Raise(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
