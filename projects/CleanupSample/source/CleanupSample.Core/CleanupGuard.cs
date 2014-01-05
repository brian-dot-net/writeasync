//-----------------------------------------------------------------------
// <copyright file="CleanupGuard.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample
{
    using System;
    using System.Threading.Tasks;

    public class CleanupGuard
    {
        public CleanupGuard()
        {
        }

        public Task RunAsync(Func<CleanupGuard, Task> doAsync)
        {
            return doAsync(this);
        }
    }
}
