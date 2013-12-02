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
            ListWorker worker = new ListWorker(list);
            TimeSpan targetDuration = TimeSpan.FromSeconds(10.0d);
            TimeSpan statusInterval = TimeSpan.FromSeconds(1.0d);
            int parallelCount = 4;

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                Task<Exception>[] tasks = new Task<Exception>[parallelCount];
                for (int i = 0; i < parallelCount; ++i)
                {
                    tasks[i] = RunWithExceptionGuardAsync(() => LoopAsync(random, l, worker, cts.Token));
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

        private static async Task LoopAsync(Random random, ExclusiveLock l, ListWorker worker, CancellationToken token)
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
                            await worker.EnumerateAsync();
                            break;
                        case 1:
                            await worker.AppendAsync();
                            break;
                        case 2:
                            await worker.RemoveAsync();
                            break;
                        case 3:
                            await worker.RemoveAllAsync();
                            break;
                    }
                }
                finally
                {
                    l.Release(elt);
                }
            }
        }
    }
}
