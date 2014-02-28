//-----------------------------------------------------------------------
// <copyright file="RetryContextExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System.Threading.Tasks;

    public static class RetryContextExtensions
    {
        public static async Task AddAsync<TResult>(this RetryContext context, string name, Task<TResult> task)
        {
            TResult result = await task;
            context.Add(name, result);
        }
    }
}
