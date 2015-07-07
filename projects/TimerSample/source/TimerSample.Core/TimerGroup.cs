//-----------------------------------------------------------------------
// <copyright file="TimerGroup.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class TimerGroup : IDisposable
    {
        private readonly Dictionary<Guid, PeriodicAction> actions;

        public TimerGroup()
        {
            this.actions = new Dictionary<Guid, PeriodicAction>();
            this.Create = (i, a) => new PeriodicAction(i, a);
        }

        private Func<TimeSpan, Action, PeriodicAction> Create { get; set; }

        public Guid Add(TimeSpan interval, Action action)
        {
            Guid id = Guid.NewGuid();
            this.actions.Add(id, this.Create(interval, action));
            return id;
        }

        public void Remove(Guid id)
        {
            PeriodicAction action;
            if (this.actions.TryGetValue(id, out action))
            {
                using (action)
                {
                    this.actions.Remove(id);
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (PeriodicAction action in this.actions.Values)
                {
                    using (action)
                    {
                    }
                }
            }
        }

        private sealed class PeriodicAction : IDisposable
        {
            private readonly Timer timer;

            public PeriodicAction(TimeSpan interval, Action action)
            {
                this.timer = new Timer(o => action(), null, interval, interval);
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                using (this.timer)
                {
                    this.timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                }
            }
        }
    }
}
