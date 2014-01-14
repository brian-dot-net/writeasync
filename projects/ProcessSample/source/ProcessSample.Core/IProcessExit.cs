//-----------------------------------------------------------------------
// <copyright file="IProcessExit.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;

    public interface IProcessExit
    {
        event EventHandler Exited;

        bool HasExited { get; }

        bool EnableRaisingEvents { get; set; }
    }
}
