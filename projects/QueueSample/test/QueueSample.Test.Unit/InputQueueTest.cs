//-----------------------------------------------------------------------
// <copyright file="InputQueueTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample.Test.Unit
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class InputQueueTest
    {
        public InputQueueTest()
        {
        }

        [Fact]
        public void Dequeue_completes_after_enqueue()
        {
            InputQueue<string> queue = new InputQueue<string>();
            
            Task<string> task = queue.DequeueAsync();

            Assert.False(task.IsCompleted);

            queue.Enqueue("a");

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("a", task.Result);
        }

        [Fact]
        public void Enqueue_then_dequeue_completes_sync()
        {
            InputQueue<string> queue = new InputQueue<string>();

            queue.Enqueue("a");

            Task<string> task = queue.DequeueAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("a", task.Result);
        }

        [Fact]
        public void Multiple_enqueues_then_dequeues_complete_sync_in_order()
        {
            InputQueue<string> queue = new InputQueue<string>();

            queue.Enqueue("a");
            queue.Enqueue("b");

            Task<string> task = queue.DequeueAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("a", task.Result);

            task = queue.DequeueAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("b", task.Result);
        }

        [Fact]
        public void Enqueue_then_dequeue_repeated_twice_completes_sync()
        {
            InputQueue<string> queue = new InputQueue<string>();

            queue.Enqueue("a");

            Task<string> task = queue.DequeueAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("a", task.Result);

            queue.Enqueue("b");

            task = queue.DequeueAsync();

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("b", task.Result);
        }

        [Fact]
        public void Dequeue_then_enqueue_repeated_twice_completes_pending()
        {
            InputQueue<string> queue = new InputQueue<string>();

            Task<string> task = queue.DequeueAsync();

            Assert.False(task.IsCompleted);

            queue.Enqueue("a");

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("a", task.Result);

            task = queue.DequeueAsync();

            Assert.False(task.IsCompleted);

            queue.Enqueue("b");

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal("b", task.Result);
        }

        [Fact]
        public void Dequeue_while_dequeue_still_pending_throws_InvalidOperation()
        {
            InputQueue<string> queue = new InputQueue<string>();

            Task<string> task = queue.DequeueAsync();

            Assert.False(task.IsCompleted);

            Assert.Throws<InvalidOperationException>(() => queue.DequeueAsync());
        }

        [Fact]
        public void Dispose_completes_pending_receive_with_ObjectDisposed()
        {
            InputQueue<string> queue = new InputQueue<string>();

            Task<string> task = queue.DequeueAsync();

            Assert.False(task.IsCompleted);

            queue.Dispose();

            Assert.True(task.IsFaulted);
            Assert.NotNull(task.Exception);
            AggregateException ae = task.Exception;
            Assert.Equal(1, ae.InnerExceptions.Count);
            Assert.IsType<ObjectDisposedException>(ae.InnerExceptions[0]);
        }
    }
}
