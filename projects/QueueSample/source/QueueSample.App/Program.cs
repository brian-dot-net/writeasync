//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            IInputQueue<int> queue = new InputQueueWithEvents<int>(new InputQueue<int>(), Guid.NewGuid(), QueueEventSource.Instance);
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task enqueueTask = PrintException(EnqueueLoopAsync(queue, cts.Token));
                Task dequeueTask = PrintException(DequeueLoopAsync(queue, cts.Token));

                Console.WriteLine("Press ENTER to cancel.");
                Console.ReadLine();
                cts.Cancel();
                queue.Dispose();

                Task.WaitAll(enqueueTask, dequeueTask);
            }
        }

        private static async Task EnqueueLoopAsync(IProducerQueue<int> queue, CancellationToken token)
        {
            await Task.Yield();

            int i = 0;
            while (!token.IsCancellationRequested)
            {
                ++i;
                queue.Enqueue(i);
                await Task.Delay(1);
            }
        }

        private static async Task DequeueLoopAsync(IConsumerQueue<int> queue, CancellationToken token)
        {
            await Task.Yield();

            int previous = 0;
            try
            {
                while (!token.IsCancellationRequested)
                {
                    int current = await queue.DequeueAsync();
                    if (current - previous != 1)
                    {
                        throw GetOutOfOrderError(current, previous);
                    }

                    previous = current;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private static Exception GetOutOfOrderError(int current, int previous)
        {
            string message = string.Format(CultureInfo.InvariantCulture, "Invalid data! Current is {0} but previous was {1}.", current, previous);
            return new InvalidOperationException(message);
        }

        private static async Task PrintException(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
