//-----------------------------------------------------------------------
// <copyright file="Blocking.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample
{
    using System;
    using System.Threading.Tasks;

    public static class Blocking
    {
        public static Func<Task> Task<TInput>(Action<TInput> action, TInput input)
        {
            return new BlockingTask<TInput>(action, input).Func;
        }

        private struct BlockingTask<TInput>
        {
            private readonly Action<TInput> action;
            private readonly TInput input;

            public BlockingTask(Action<TInput> action, TInput input)
            {
                this.action = action;
                this.input = input;
            }

            public Func<Task> Func
            {
                get { return this.Do; }
            }

            private Task Do()
            {
                this.action(this.input);
                return System.Threading.Tasks.Task.FromResult(false);
            }
        }
    }
}
