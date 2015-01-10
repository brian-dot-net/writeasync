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
        private readonly string name;

        public LoopingScheduler(string name)
        {
            this.name = name;
        }

        public Task<string> DoAsync()
        {
            return Task.FromResult(this.name);
        }
    }
}
