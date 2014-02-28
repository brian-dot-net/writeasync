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
        private readonly Func<Task> func;

        public RetryLoop(Func<Task> func)
        {
            this.func = func;
            this.Condition = r => false;
            this.Timer = new ElapsedTimer();
        }

        public Expression<Func<RetryContext, bool>> Condition { get; set; }

        public IElapsedTimer Timer { get; set; }

        public async Task<RetryContext> ExecuteAsync()
        {
            Func<RetryContext, bool> condition = this.Condition.Compile();
            RetryContext context = new RetryContext(this.Timer);
            do
            {
                await this.func();
                ++context.Iteration;
            }
            while (condition(context));

            return context;
        }
    }
}
