//-----------------------------------------------------------------------
// <copyright file="IProcessExitEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;

    public interface IProcessExitEvents
    {
        event EventHandler Exited;

        bool EnableRaisingEvents { get; set; }
    }
}
