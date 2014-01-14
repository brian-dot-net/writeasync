//-----------------------------------------------------------------------
// <copyright file="IProcessExitStatus.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;

    public interface IProcessExitStatus
    {
        bool HasExited { get; }

        int ExitCode { get; }

        DateTime ExitTime { get; }
    }
}
