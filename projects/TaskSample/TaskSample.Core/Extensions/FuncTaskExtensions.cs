// <copyright file="FuncTaskExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace TaskSample.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class FuncTaskExtensions
    {
        public static async Task<T> FirstAsync<T>(this IEnumerable<Func<CancellationToken, Task<T>>> funcs, Predicate<T> pred)
        {
            foreach (Func<CancellationToken, Task<T>> func in funcs)
            {
                T result = await func(CancellationToken.None);
                if (pred(result))
                {
                    return result;
                }
            }

            return default(T);
        }
    }
}
