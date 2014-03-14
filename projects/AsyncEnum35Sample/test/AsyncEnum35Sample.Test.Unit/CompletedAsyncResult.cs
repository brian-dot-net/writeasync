//-----------------------------------------------------------------------
// <copyright file="CompletedAsyncResult.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnum35Sample.Test.Unit
{
    using System;

    internal sealed class CompletedAsyncResult<TResult> : AsyncResult<TResult>
    {
        public CompletedAsyncResult(TResult result, AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.SetAsCompleted(result, true);
        }

        public static TResult End(IAsyncResult result)
        {
            return ((CompletedAsyncResult<TResult>)result).EndInvoke();
        }
    }
}
