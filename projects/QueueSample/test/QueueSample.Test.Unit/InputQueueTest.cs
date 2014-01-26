//-----------------------------------------------------------------------
// <copyright file="InputQueueTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample.Test.Unit
{
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
    }
}
