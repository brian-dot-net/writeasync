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

        [Fact]
        public void Pending_receive_completes_after_send_with_greater_data_size_followed_by_another_receive_with_same_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3, 4 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);

            byte[] receiveBuffer2 = new byte[1];
            AssertTaskCompleted(1, channel.ReceiveAsync(receiveBuffer2));
            Assert.Equal(new byte[] { 4 }, receiveBuffer2);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_greater_data_size_followed_by_another_receive_with_lower_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3, 4, 5 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);

            byte[] receiveBuffer2 = new byte[1];
            AssertTaskCompleted(1, channel.ReceiveAsync(receiveBuffer2));
            Assert.Equal(new byte[] { 4 }, receiveBuffer2);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_greater_data_size_followed_by_another_receive_with_greater_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3, 4, 5 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);

            byte[] receiveBuffer2 = new byte[3];
            AssertTaskCompleted(2, channel.ReceiveAsync(receiveBuffer2));
            Assert.Equal(new byte[] { 4, 5, 0 }, receiveBuffer2);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_greater_data_size_followed_by_two_receives_to_read_excess()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);

            byte[] receiveBuffer2 = new byte[1];
            AssertTaskCompleted(1, channel.ReceiveAsync(receiveBuffer2));
            Assert.Equal(new byte[] { 4 }, receiveBuffer2);

            byte[] receiveBuffer3 = new byte[2];
            AssertTaskCompleted(2, channel.ReceiveAsync(receiveBuffer3));
            Assert.Equal(new byte[] { 5, 6 }, receiveBuffer3);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_greater_data_size_followed_by_one_receive_to_read_excess_leaving_final_receive_pending()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);

            byte[] receiveBuffer2 = new byte[3];
            AssertTaskCompleted(3, channel.ReceiveAsync(receiveBuffer2));
            Assert.Equal(new byte[] { 4, 5, 6 }, receiveBuffer2);

            byte[] receiveBuffer3 = new byte[1];
            AssertTaskPending(channel.ReceiveAsync(receiveBuffer3));
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_equal_data_size_followed_by_another_send_and_receive()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = AssertTaskPending(channel.ReceiveAsync(receiveBuffer));

            byte[] sendBuffer = new byte[] { 1, 2, 3 };
            channel.Send(sendBuffer);

            AssertTaskCompleted(3, receiveTask);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);

            byte[] receiveBuffer2 = new byte[3];
            Task<int> receiveTask2 = AssertTaskPending(channel.ReceiveAsync(receiveBuffer2));

            byte[] sendBuffer2 = new byte[] { 4, 5, 6 };
            channel.Send(sendBuffer2);

            AssertTaskCompleted(3, receiveTask2);
            Assert.Equal(new byte[] { 4, 5, 6 }, receiveBuffer2);
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
