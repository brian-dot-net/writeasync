//-----------------------------------------------------------------------
// <copyright file="LoopingScheduler.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample
{
    using System;
    using System.Threading.Tasks;

    public class LoopingScheduler
    {
        private readonly Func<Task> doAsync;

        public LoopingScheduler(Func<Task> doAsync)
        {
            this.doAsync = doAsync;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                await this.doAsync();
            }
        }
    }
}
