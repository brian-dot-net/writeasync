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
    }
}
