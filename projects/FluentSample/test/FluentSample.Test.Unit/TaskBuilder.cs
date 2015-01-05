//-----------------------------------------------------------------------
// <copyright file="TaskBuilder.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;

    internal static class TaskBuilder
    {
        public static Task Completed()
        {
            return TaskResultBuilder.Completed();
        }

        public static Task Pending()
        {
            return TaskResultBuilder.Pending();
        }

        public static Task Faulted(params Exception[] exceptions)
        {
            return TaskResultBuilder.Faulted(exceptions);
        }

        public static Task Canceled()
        {
            return TaskResultBuilder.Canceled();
        }
    }
}
