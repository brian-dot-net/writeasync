//-----------------------------------------------------------------------
// <copyright file="RetryLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class RetryLoop
    {
        private readonly Func<RetryContext, Task> func;

        public RetryLoop(Func<RetryContext, Task> func)
        {
            this.func = func;
            this.Condition = r => false;
            this.Succeeded = r => true;
            this.Timer = new ElapsedTimer();
        }

        public Expression<Func<RetryContext, bool>> Condition { get; set; }

        public Expression<Func<RetryContext, bool>> Succeeded { get; set; }

        public IElapsedTimer Timer { get; set; }

        public async Task<RetryContext> ExecuteAsync()
        {
            Func<RetryContext, bool> condition = this.Condition.Compile();
            Func<RetryContext, bool> succeeded = this.Succeeded.Compile();
            RetryContext context = new RetryContext();
            TimeSpan startTime = this.Timer.Elapsed;
            do
            {
                context.ElapsedTime = this.Timer.Elapsed - startTime;
                await this.func(context);
                context.Succeeded = succeeded(context);
                ++context.Iteration;
                context.ElapsedTime = this.Timer.Elapsed - startTime;
            }
            while (!context.Succeeded && condition(context));

            return context;
        }
    }
}
