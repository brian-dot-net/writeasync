﻿//-----------------------------------------------------------------------
// <copyright file="OperationManager.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ConcurrentSample
{
    using System;
    using System.Threading.Tasks;

    public class OperationManager
    {
        private readonly Func<Task> doAsync;

        public OperationManager(int maxPendingCalls, Func<Task> doAsync)
        {
            this.doAsync = doAsync;
        }

        public async Task RunAsync(int totalCalls)
        {
            for (int i = 0; i < totalCalls; ++i)
            {
                await this.doAsync();
            }
        }
    }
}
