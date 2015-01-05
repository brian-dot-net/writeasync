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
        public static TaskAssertions Should<TResult>(this Task<TResult> subject)
        {
            return new TaskAssertions(subject);
        }

        public static TaskAssertions Should(this Task subject)
        {
            return new TaskAssertions(subject);
        }
    }
}
