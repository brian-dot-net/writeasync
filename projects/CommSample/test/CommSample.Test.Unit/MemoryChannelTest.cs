//-----------------------------------------------------------------------
// <copyright file="MemoryChannelTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample.Test.Unit
{
    using System.Threading.Tasks;
    using Xunit;

    public class MemoryChannelTest
    {
        public MemoryChannelTest()
        {
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_same_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_lower_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(2, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 0 }, receiveBuffer);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_greater_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3, 4 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);
        }

        private static Task<TResult> AssertTaskPending<TResult>(Task<TResult> task)
        {
            Assert.False(task.IsCompleted, "Task should not be completed.");
            Assert.False(task.IsFaulted, "Task should not be faulted: " + task.Exception);
            return task;
        }

        private static void AssertTaskCompleted<TResult>(TResult expected, Task<TResult> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(expected, task.Result);
        }
    }
}
