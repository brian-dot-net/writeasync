//-----------------------------------------------------------------------
// <copyright file="InputQueue.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InputQueue<T>
    {
        private readonly Queue<T> items;

        private TaskCompletionSource<T> pending;

        public InputQueue()
        {
            this.items = new Queue<T>();
        }

        public Task<T> DequeueAsync()
        {
            this.pending = new TaskCompletionSource<T>();
            if (this.items.Count > 0)
            {
                T item = this.items.Dequeue();
                this.pending.SetResult(item);
            }

            return this.pending.Task;
        }

        public void Enqueue(T item)
        {
            if (this.pending == null)
            {
                this.items.Enqueue(item);
            }
            else
            {
                this.pending.SetResult(item);
            }
        }
    }
}
