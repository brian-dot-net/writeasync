// <copyright file="UberQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace QueueChallenge
{
    using System.Threading.Tasks;

    public sealed class UberQueue<T> : IAsyncQueue<T>
    {
        private readonly AsyncQueue<T> output;
        private readonly Task input;

        public UberQueue(IAsyncQueue<T>[] queues)
        {
            this.output = new AsyncQueue<T>();
            this.input = this.DequeueAllAsync(queues);
        }

        public Task<T> DequeueAsync() => this.output.DequeueAsync();

        private Task DequeueAllAsync(IAsyncQueue<T>[] queues)
        {
            Task[] tasks = new Task[queues.Length];
            for (int i = 0; i < tasks.Length; ++i)
            {
                tasks[i] = this.DequeueOneAsync(queues[i]);
            }

            return Task.WhenAll(tasks);
        }

        private async Task DequeueOneAsync(IAsyncQueue<T> input)
        {
            while (true)
            {
                T next = await input.DequeueAsync();
                this.output.Enqueue(next);
            }
        }
    }
}
