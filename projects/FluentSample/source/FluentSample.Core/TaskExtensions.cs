//-----------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample
{
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        public static TaskAssertions<Task<TResult>> Should<TResult>(this Task<TResult> subject)
        {
            return new TaskAssertions<Task<TResult>>(subject);
        }

        public static TaskAssertions<Task> Should(this Task subject)
        {
            return new TaskAssertions<Task>(subject);
        }
    }
}
