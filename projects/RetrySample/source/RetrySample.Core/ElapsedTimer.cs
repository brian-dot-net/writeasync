//-----------------------------------------------------------------------
// <copyright file="ElapsedTimer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;
    using System.Diagnostics;

    public sealed class ElapsedTimer : IElapsedTimer
    {
        private readonly Stopwatch stopwatch;

        public ElapsedTimer()
        {
            this.stopwatch = Stopwatch.StartNew();
        }

        public TimeSpan Elapsed
        {
            get { return this.stopwatch.Elapsed; }
        }
    }
}
