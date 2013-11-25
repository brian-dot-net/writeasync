//-----------------------------------------------------------------------
// <copyright file="MemoryChannel.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading.Tasks;

    public class MemoryChannel
    {
        private TaskCompletionSource<int> pendingReceive;
        private byte[] pendingReceiveBuffer;

        public MemoryChannel()
        {
        }

        public Task<int> ReceiveAsync(byte[] buffer)
        {
            this.pendingReceive = new TaskCompletionSource<int>();
            this.pendingReceiveBuffer = buffer;
            return this.pendingReceive.Task;
        }

        public void Send(byte[] buffer)
        {
            buffer.CopyTo(this.pendingReceiveBuffer, 0);
            int bytesReceived = Math.Min(this.pendingReceiveBuffer.Length, buffer.Length);
            this.pendingReceive.SetResult(bytesReceived);
        }
    }
}
