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
        private readonly Func<RetryContext, Task> func;

        public RetryLoop(Func<RetryContext, Task> func)
        {
            this.func = func;
            this.ShouldRetry = r => false;
            this.Succeeded = r => true;
            this.Timer = new ElapsedTimer();
        }

        public Func<RetryContext, bool> ShouldRetry { get; set; }

        public Func<RetryContext, bool> Succeeded { get; set; }

        public IElapsedTimer Timer { get; set; }

        public async Task<RetryContext> ExecuteAsync()
        {
            RetryContext context = new RetryContext();
            TimeSpan startTime = this.Timer.Elapsed;
            do
            {
                context.ElapsedTime = this.Timer.Elapsed - startTime;
                await this.func(context);
                context.ElapsedTime = this.Timer.Elapsed - startTime;
                context.Succeeded = this.Succeeded(context);
                ++context.Iteration;
            }
            while (!context.Succeeded && this.ShouldRetry(context));

            return context;
        }
    }
}
