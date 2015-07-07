//-----------------------------------------------------------------------
// <copyright file="VirtualClock.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample.Test.Unit
{
    using System;
    using System.Collections.Generic;

    internal sealed class VirtualClock
    {
        private TimeSpan currentTime;
        private VirtualAction action;

        public VirtualClock()
        {
        }

        public PeriodicAction CreateAction(TimeSpan interval, Action action)
        {
            return this.action = new VirtualAction(interval, action);
        }

        public void Sleep(TimeSpan interval)
        {
            TimeSpan start = this.currentTime;
            TimeSpan end = start + interval;
            foreach (TimeSpan deadline in this.action.DeadlinesBetween(start, end))
            {
                this.action.Invoke();
            }

            this.currentTime = end;
        }

        private sealed class VirtualAction : PeriodicAction
        {
            private readonly TimeSpan interval;
            private readonly Action action;

            public VirtualAction(TimeSpan interval, Action action)
                : base(interval, action)
            {
                this.interval = interval;
                this.action = action;
            }

            public IEnumerable<TimeSpan> DeadlinesBetween(TimeSpan start, TimeSpan end)
            {
                if (start < this.interval)
                {
                    start = this.interval;
                }

                int n = (int)Math.Ceiling((double)start.Ticks / this.interval.Ticks);
                TimeSpan firstDeadline = TimeSpan.FromTicks(n * this.interval.Ticks);

                for (TimeSpan deadline = firstDeadline; deadline < end; deadline += this.interval)
                {
                    yield return deadline;
                }
            }

            public void Invoke()
            {
                this.action();
            }

            public override void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
