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
        private readonly List<VirtualAction> actions;
        
        private TimeSpan currentTime;

        public VirtualClock()
        {
            this.actions = new List<VirtualAction>();
        }

        public PeriodicAction CreateAction(TimeSpan interval, Action action)
        {
            VirtualAction newAction = new VirtualAction(interval, action, this.currentTime);
            this.actions.Add(newAction);
            return newAction;
        }

        public void Sleep(TimeSpan interval)
        {
            TimeSpan start = this.currentTime;
            TimeSpan end = start + interval;

            List<Tuple<TimeSpan, VirtualAction>> actionsToInvoke = new List<Tuple<TimeSpan, VirtualAction>>();
            foreach (VirtualAction action in this.actions)
            {
                foreach (TimeSpan deadline in action.DeadlinesBetween(start, end))
                {
                    actionsToInvoke.Add(Tuple.Create(deadline, action));
                }
            }

            actionsToInvoke.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            foreach (Tuple<TimeSpan, VirtualAction> t in actionsToInvoke)
            {
                t.Item2.Invoke();
            }

            this.currentTime = end;
        }

        private sealed class VirtualAction : PeriodicAction
        {
            private readonly TimeSpan interval;
            private readonly Action action;
            private readonly TimeSpan creationTime;

            private bool canceled;

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
                if (!this.canceled)
                {
                    this.action();
                }
            }

            public override void Dispose()
            {
                this.canceled = true;
            }
        }
    }
}
