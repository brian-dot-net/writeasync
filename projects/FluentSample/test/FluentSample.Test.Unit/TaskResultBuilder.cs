//-----------------------------------------------------------------------
// <copyright file="TaskResultBuilder.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace FluentSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;

    internal static class TaskResultBuilder
    {
        public static Task<bool> Completed()
        {
            return Task.FromResult(false);
        }

        public static Task<bool> Pending()
        {
            return new TaskCompletionSource<bool>().Task;
        }

        public static Task<bool> Faulted()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetException(new InvalidCastException("Expected failure."));
            return tcs.Task;
        }

        public static Task<bool> Canceled()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetCanceled();
            return tcs.Task;
        }
    }
}
