//-----------------------------------------------------------------------
// <copyright file="IElapsedTimer.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;

    public interface IElapsedTimer
    {
        TimeSpan Elapsed { get; }
    }
}
