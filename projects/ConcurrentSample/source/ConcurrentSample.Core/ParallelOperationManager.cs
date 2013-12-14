//-----------------------------------------------------------------------
// <copyright file="ParallelOperationManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample
{
    using System;
using System.Threading.Tasks;

    public class ParallelOperationManager
    {
        private readonly Func<Task> doAsync;

        public ParallelOperationManager(int maxPendingCalls, Func<Task> doAsync)
        {
            this.doAsync = doAsync;
        }

        public Task RunAsync(int totalCalls)
        {
            return this.doAsync();
        }
    }
}
