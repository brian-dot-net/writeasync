//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace LockSample
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Random random = new Random();
            ExclusiveLock l = new ExclusiveLock();
            List<int> list = new List<int>();

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task task = LoopAsync(random, l, list, cts.Token);

                Thread.Sleep(1000);

                cts.Cancel();
                task.Wait();
            }
        }

        private static async Task LoopAsync(Random random, ExclusiveLock l, IList<int> list, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                ExclusiveLock.Token elt = await l.AcquireAsync();
                try
                {
                    await Task.Yield();
                    switch (random.Next(2))
                    {
                        case 0:
                            await EnumerateListAsync(list);
                            break;
                        case 1:
                            await AppendToListAsync(list);
                            break;
                    }
                }
                finally
                {
                    l.Release(elt);
                }
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
