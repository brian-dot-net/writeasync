// <copyright file="UberQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System.Threading.Tasks;

    public sealed class UberQueue<T>
    {
        private readonly AsyncQueue<T> output;
        private readonly Task input;

        public UberQueue(AsyncQueue<T>[] queues)
        {
            this.output = new AsyncQueue<T>();
            this.input = this.DequeueAllAsync(queues);
        }

        public Task<T> DequeueAsync() => this.output.DequeueAsync();

        private Task DequeueAllAsync(AsyncQueue<T>[] queues)
        {
            Task[] tasks = new Task[queues.Length];
            for (int i = 0; i < tasks.Length; ++i)
            {
                tasks[i] = this.DequeueOneAsync(queues[i]);
            }

            return Task.WhenAll(tasks);
        }

        private async Task DequeueOneAsync(AsyncQueue<T> queue)
        {
            T next = await queue.DequeueAsync();
            this.output.Enqueue(next);
        }
    }
}
