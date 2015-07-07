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
            return this.action = new VirtualAction(interval, action, this.currentTime);
        }

        public void Sleep(TimeSpan interval)
        {
            TimeSpan start = this.currentTime;
            TimeSpan end = start + interval;
            
            if (this.action != null)
            {
                foreach (TimeSpan deadline in this.action.DeadlinesBetween(start, end))
                {
                    this.action.Invoke();
                }
            }

            this.currentTime = end;
        }

        private sealed class VirtualAction : PeriodicAction
        {
            private readonly TimeSpan interval;
            private readonly Action action;
            private readonly TimeSpan creationTime;

            public VirtualAction(TimeSpan interval, Action action, TimeSpan creationTime)
                : base(interval, action)
            {
                this.interval = interval;
                this.action = action;
                this.creationTime = creationTime;
            }

            public IEnumerable<TimeSpan> DeadlinesBetween(TimeSpan start, TimeSpan end)
            {
                int n = (int)Math.Ceiling((double)(start - this.creationTime).Ticks / this.interval.Ticks);
                if (n < 1)
                {
                    n = 1;
                }

                TimeSpan firstDeadline = TimeSpan.FromTicks(n * this.interval.Ticks) + this.creationTime;

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
