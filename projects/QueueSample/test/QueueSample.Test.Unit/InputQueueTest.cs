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
    }
}
