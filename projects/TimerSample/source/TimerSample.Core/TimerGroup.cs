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
        private readonly Dictionary<Guid, Timer> timers;

        public TimerGroup()
        {
            this.timers = new Dictionary<Guid, Timer>();
        }

        public Guid Add(TimeSpan interval, Action action)
        {
            Guid id = Guid.NewGuid();
            this.timers.Add(id, new Timer(o => action(), null, interval, interval));
            return id;
        }

        public void Remove(Guid id)
        {
            this.timers[id].Dispose();
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
                foreach (Timer timer in this.timers.Values)
                {
                    timer.Dispose();
                }
            }
        }
    }
}
