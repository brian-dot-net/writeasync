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
        public RetryContext()
        {
        }

        public int Iteration { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public bool Succeeded { get; set; }

        public AggregateException Exception { get; set; }
    }
}
