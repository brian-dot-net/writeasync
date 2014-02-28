//-----------------------------------------------------------------------
// <copyright file="RetryLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;
    using System.Threading.Tasks;

    public class RetryLoop
    {
        private readonly Func<Task> func;

        public RetryLoop(Func<Task> func)
        {
            this.func = func;
        }

        public Task ExecuteAsync()
        {
            return this.func();
        }
    }
}
