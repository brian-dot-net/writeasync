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

    public class TimerGroup
    {
        private readonly List<Timer> timers;

        public TimerGroup()
        {
            this.timers = new List<Timer>();
        }

        public void Add(TimeSpan interval, Action action)
        {
            this.timers.Add(new Timer(o => action(), null, interval, interval));
        }
    }
}
