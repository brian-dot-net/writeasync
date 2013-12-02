//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Random random = new Random();
            ExclusiveLock l = new ExclusiveLock();
            List<int> list = new List<int>();
            TimeSpan targetDuration = TimeSpan.FromSeconds(3.0d);
            TimeSpan statusInterval = TimeSpan.FromSeconds(1.0d);
            int parallelCount = 4;

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Task<Exception>[] tasks = new Task<Exception>[parallelCount];
                for (int i = 0; i < parallelCount; ++i)
                {
                    tasks[i] = RunWithExceptionGuardAsync(() => LoopAsync(random, l, list, cts.Token));
                }

                while (stopwatch.Elapsed < targetDuration)
                {
                    Thread.Sleep(statusInterval);
                    Console.WriteLine("[{0:000.000}] List count={1}.", stopwatch.Elapsed.TotalSeconds, list.Count);
                }

                cts.Cancel();
                Task<Exception[]> finalTask = Task.WhenAll(tasks);
                Console.WriteLine("Waiting for final task...");
                if (!finalTask.Wait(5000))
                {
                    throw new TimeoutException();
                }

                Exception[] results = finalTask.Result;
                Exception[] exceptions = results.Where(e => e != null).ToArray();
                if (exceptions.Length > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }

        private static async Task<Exception> RunWithExceptionGuardAsync(Func<Task> doAsync)
        {
            Exception exception = null;
            try
            {
                await doAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e);
                exception = e;
            }

            return exception;
        }

        private static async Task LoopAsync(Random random, ExclusiveLock l, IList<int> list, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                ExclusiveLock.Token elt = await l.AcquireAsync();
                try
                {
                    await Task.Yield();
                    switch (random.Next(4))
                    {
                        case 0:
                            await EnumerateListAsync(list);
                            break;
                        case 1:
                            await AppendToListAsync(list);
                            break;
                        case 2:
                            await RemoveFromListAsync(list);
                            break;
                        case 3:
                            await RemoveAllFromListAsync(list);
                            break;
                    }
                }
                finally
                {
                    l.Release(elt);
                }
            }
        }

        private static async Task RemoveFromListAsync(IList<int> list)
        {
            int n = list.Count;
            if (n > 0)
            {
                list.RemoveAt(n - 1);
                await Task.Yield();
                list.RemoveAt(n - 2);
            }
        }

        private static async Task RemoveAllFromListAsync(IList<int> list)
        {
            while (list.Count > 0)
            {
                await RemoveFromListAsync(list);
            }
        }

        private static async Task AppendToListAsync(IList<int> list)
        {
            int n = list.Count;
            list.Add(n + 1);
            await Task.Yield();
            list.Add(n + 2);
        }

        private static async Task EnumerateListAsync(IList<int> list)
        {
            int lastItem = 0;
            foreach (int item in list)
            {
                if (lastItem != (item - 1))
                {
                    throw new InvalidOperationException(string.Format(
                        CultureInfo.InvariantCulture,
                        "State corruption detected; expected {0} but saw {1} in next list entry.",
                        lastItem + 1,
                        item));
                }

                lastItem = item;
                await Task.Yield();
            }
        }
    }
}
