// <copyright file="TimePoint.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Diagnostics;

    public readonly struct TimePoint
    {
        private static readonly double TicksPerTimerTick = 10000000.0 / Stopwatch.Frequency;

        private readonly long timerTicks;

        public TimePoint(long timerTicks)
        {
            this.timerTicks = timerTicks;
        }

        public static TimeSpan operator -(TimePoint x, TimePoint y)
        {
            long deltaTimerTicks = x.timerTicks - y.timerTicks;
            double deltaTicks = TicksPerTimerTick * deltaTimerTicks;
            return new TimeSpan((long)deltaTicks);
        }

        public static TimePoint Now() => new TimePoint(Stopwatch.GetTimestamp());

        public TimeSpan Elapsed() => Now() - this;
    }
}
