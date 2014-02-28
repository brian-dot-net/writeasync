//-----------------------------------------------------------------------
// <copyright file="RetryContext.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;

    public class RetryContext
    {
        private readonly IElapsedTimer timer;
        private readonly TimeSpan startTime;

        public RetryContext(IElapsedTimer timer)
        {
            this.timer = timer;
            this.startTime = this.timer.Elapsed;
        }

        public int Iteration { get; set; }

        public TimeSpan ElapsedTime
        {
            get { return this.timer.Elapsed - this.startTime; }
        }
    }
}
