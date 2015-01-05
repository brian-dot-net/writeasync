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
        public static TaskAssertions<TResult> Should<TResult>(this Task<TResult> subject)
        {
            return new TaskAssertions<TResult>(subject);
        }

        public static TaskAssertions Should(this Task subject)
        {
            return new TaskAssertions(subject);
        }
    }
}
