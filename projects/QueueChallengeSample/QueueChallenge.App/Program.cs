// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge.App
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private const byte N = 31;
        private const ushort C = 50000;

        private static void Main()
        {
            AsyncQueue<int> queue = new();

            Task[] tasks = new Task[N + 1];
            Barrier barrier = new(N);
            tasks[N] = DequeueAsync(queue, N * C);
            for (int i = 0; i < N; ++i)
            {
                byte n = (byte)i;
                tasks[i] = Task.Run(() => Enqueue(queue, barrier, n));
            }

            Task.WaitAll(tasks);
        }

        private static void Enqueue(AsyncQueue<int> queue, Barrier barrier, byte n)
        {
            Console.WriteLine($"START . . Enqueue[{n}]");
            barrier.SignalAndWait();
            for (int i = 0; i < C; ++i)
            {
                queue.Enqueue((i << 8) | n);
            }

            Console.WriteLine($"END . . . Enqueue[{n}]");
        }

        private static async Task DequeueAsync(AsyncQueue<int> queue, int expectedCount)
        {
            Dictionary<byte, int> lastItems = new();
            for (int i = 0; i < expectedCount; ++i)
            {
                int next = await queue.DequeueAsync();
                byte slot = (byte)(next % 256);
                if (lastItems.TryGetValue(slot, out int prev))
                {
                    if (next - prev != 256)
                    {
                        throw new InvalidOperationException($"Bad result! slot={slot} prev={prev} next={next}");
                    }
                }

                lastItems[slot] = next;
            }
        }
    }
}
