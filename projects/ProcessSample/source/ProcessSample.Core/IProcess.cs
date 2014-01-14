//-----------------------------------------------------------------------
// <copyright file="IProcess.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;

    public interface IProcess
    {
        event EventHandler Exited;

        bool HasExited { get; }

        bool EnableRaisingEvents { get; set; }
    }
}
