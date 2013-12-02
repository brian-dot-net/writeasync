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
                switch (random.Next(1))
                {
                    case 0:
                        await EnumerateListAsync(l, list);
                        break;
                }
            }
        }

        private static async Task EnumerateListAsync(ExclusiveLock l, IList<int> list)
        {
            ExclusiveLock.Token token = await l.AcquireAsync();
            await Task.Yield();
            try
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

                    await Task.Yield();
                }
            }
            finally
            {
                l.Release(token);
            }
        }
    }
}
