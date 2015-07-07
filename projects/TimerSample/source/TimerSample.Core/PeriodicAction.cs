//-----------------------------------------------------------------------
// <copyright file="PeriodicAction.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample
{
    using System;

    public abstract class PeriodicAction : IDisposable
    {
        protected PeriodicAction(TimeSpan interval, Action action)
        {
        }

        public abstract void Dispose();
    }
}
