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
            Task<int> receiveTask = channel.ReceiveAsync(receiveBuffer);

            Assert.False(receiveTask.IsCompleted);
            Assert.False(receiveTask.IsFaulted);

            byte[] sendBuffer = new byte[] { 1, 2, 3 };
            channel.Send(sendBuffer);

            Assert.Equal(TaskStatus.RanToCompletion, receiveTask.Status);
            Assert.Equal(3, receiveTask.Result);
            Assert.Equal(new byte[] { 1, 2, 3 }, receiveBuffer);
        }

        [Fact]
        public void Pending_receive_completes_after_send_with_lower_data_size()
        {
            MemoryChannel channel = new MemoryChannel();

            byte[] receiveBuffer = new byte[3];
            Task<int> receiveTask = channel.ReceiveAsync(receiveBuffer);

            Assert.False(receiveTask.IsCompleted);
            Assert.False(receiveTask.IsFaulted);

            byte[] sendBuffer = new byte[] { 1, 2 };
            channel.Send(sendBuffer);

            Assert.Equal(TaskStatus.RanToCompletion, receiveTask.Status);
            Assert.Equal(2, receiveTask.Result);
            Assert.Equal(new byte[] { 1, 2, 0 }, receiveBuffer);
        }
    }
}
