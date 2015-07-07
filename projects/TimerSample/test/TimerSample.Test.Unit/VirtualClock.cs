//-----------------------------------------------------------------------
// <copyright file="VirtualClock.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TimerSample.Test.Unit
{
    using System;

    internal sealed class VirtualClock
    {
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
            this.action.Invoke();
        }

        private sealed class VirtualAction : PeriodicAction
        {
            private readonly Action action;

            public VirtualAction(TimeSpan interval, Action action)
                : base(interval, action)
            {
                this.action = action;
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
