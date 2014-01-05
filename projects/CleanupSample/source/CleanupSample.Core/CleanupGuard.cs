//-----------------------------------------------------------------------
// <copyright file="CleanupGuard.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CleanupGuard
    {
        public CleanupGuard()
        {
            this.Steps = new Stack<Func<Task>>();
        }

        public Stack<Func<Task>> Steps { get; private set; }

        public async Task RunAsync(Func<CleanupGuard, Task> doAsync)
        {
            Exception exception = null;
            try
            {
                await doAsync(this);
            }
            catch (Exception e)
            {
                exception = e;
            }

            while (this.Steps.Count > 0)
            {
                Func<Task> cleanupAsync = this.Steps.Pop();
                await cleanupAsync();
            }
            
            if (exception != null)
            {
                throw new AggregateException(exception).Flatten();
            }
        }
    }
}
