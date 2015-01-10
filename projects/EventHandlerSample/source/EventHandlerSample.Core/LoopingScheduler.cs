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

        public event EventHandler<AsyncEventArgs> Paused;

        public Func<TimeSpan> GetElapsed { get; set; }

        public async Task RunAsync(TimeSpan pauseInterval)
        {
            TimeSpan start = this.GetElapsed();
            while (true)
            {
                await this.doAsync();
                start = await this.CheckPauseIntervalAsync(start, pauseInterval);
            }
        }

        private static TimeSpan DefaultGetElapsed()
        {
            return DefaultStopwatch.Elapsed;
        }

        private async Task<TimeSpan> CheckPauseIntervalAsync(TimeSpan start, TimeSpan pauseInterval)
        {
            TimeSpan elapsed = this.GetElapsed() - start;
            if (elapsed >= pauseInterval)
            {
                start = elapsed;
                await this.RaiseAsync(this.Paused);
            }

            return start;
        }

        private async Task RaiseAsync(EventHandler<AsyncEventArgs> handler)
        {
            if (handler != null)
            {
                AsyncEventArgs e = new AsyncEventArgs();
                handler(this, e);
                foreach (Func<Task> nextAsync in e.Tasks)
                {
                    await nextAsync();
                }
            }
        }
    }
}
