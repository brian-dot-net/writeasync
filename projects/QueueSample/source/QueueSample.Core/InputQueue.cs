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

    public class InputQueue<T> : IInputQueue<T>
    {
        private readonly Queue<T> items;

        private TaskCompletionSource<T> pending;
        private bool disposed;

        public InputQueue()
        {
            this.items = new Queue<T>();
        }

        public Task<T> DequeueAsync()
        {
            Task<T> task;
            lock (this.items)
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("InputQueue");
                }

                if (this.pending != null)
                {
                    throw new InvalidOperationException("A dequeue operation is already in progress.");
                }

                TaskCompletionSource<T> current = new TaskCompletionSource<T>();
                task = current.Task;

                if (this.items.Count > 0)
                {
                    T item = this.items.Dequeue();
                    current.SetResult(item);
                }
                else
                {
                    this.pending = current;
                }
            }

            return task;
        }

        public void Enqueue(T item)
        {
            TaskCompletionSource<T> current = null;
            lock (this.items)
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("InputQueue");
                }

                if (this.pending == null)
                {
                    this.items.Enqueue(item);
                }
                else
                {
                    current = this.pending;
                    this.pending = null;
                }
            }

            if (current != null)
            {
                current.SetResult(item);
            }
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                TaskCompletionSource<T> current = null;
                lock (this.items)
                {
                    current = this.pending;
                    this.pending = null;
                    this.disposed = true;
                }

                if (current != null)
                {
                    current.SetException(new ObjectDisposedException("InputQueue"));
                }
            }
        }
    }
}
