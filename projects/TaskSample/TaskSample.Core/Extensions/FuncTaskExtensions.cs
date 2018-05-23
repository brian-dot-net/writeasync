// <copyright file="FuncTaskExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace TaskSample.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public static class FuncTaskExtensions
    {
        public static async Task<T> FirstAsync<T>(this IEnumerable<Func<CancellationToken, Task<T>>> funcs, Predicate<T> pred)
        {
            var tasks = new List<Task<T>>();
            T firstResult = default(T);
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                foreach (Func<CancellationToken, Task<T>> func in funcs)
                {
                    if (cts.IsCancellationRequested)
                    {
                        break;
                    }

                    Func<CancellationToken, Task<T>> match = async t =>
                    {
                        T result = await func(t);
                        if (pred(result))
                        {
                            firstResult = result;
                            cts.Cancel();
                        }

                        return result;
                    };

                    Task<T> task = match(CancellationToken.None);
                    tasks.Add(task);
                }

                await Task.WhenAny(tasks);
                return firstResult;
            }
        }
    }
}
