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
        private byte[] excess;

        public MemoryChannel()
        {
        }

        public Task<int> ReceiveAsync(byte[] buffer)
        {
            this.pendingReceive = new TaskCompletionSource<int>();
            this.pendingReceiveBuffer = buffer;

            if (this.excess != null)
            {
                this.excess.CopyTo(this.pendingReceiveBuffer, 0);
                this.pendingReceive.SetResult(this.excess.Length);
            }

            return this.pendingReceive.Task;
        }

        public void Send(byte[] buffer)
        {
            int bytesReceived = Math.Min(this.pendingReceiveBuffer.Length, buffer.Length);
            Array.Copy(buffer, 0, this.pendingReceiveBuffer, 0, bytesReceived);
            int remainingBytes = buffer.Length - bytesReceived;
            if (remainingBytes > 0)
            {
                this.excess = new byte[remainingBytes];
                Array.Copy(buffer, bytesReceived, this.excess, 0, remainingBytes);
            }

            this.pendingReceive.SetResult(bytesReceived);
        }
    }
}
