//-----------------------------------------------------------------------
// <copyright file="MemoryChannel.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MemoryChannel
    {
        private readonly LinkedList<byte[]> excessBuffers;

        private TaskCompletionSource<int> pendingReceive;
        private byte[] pendingReceiveBuffer;

        public MemoryChannel()
        {
            this.excessBuffers = new LinkedList<byte[]>();
        }

        public Task<int> ReceiveAsync(byte[] buffer)
        {
            if (this.pendingReceive != null)
            {
                throw new InvalidOperationException("A receive operation is already in progress.");
            }

            this.pendingReceive = new TaskCompletionSource<int>();
            this.pendingReceiveBuffer = buffer;

            int totalBytesReceived = 0;
            int remainingBytes = buffer.Length;
            while ((this.excessBuffers.Count > 0) && (remainingBytes > 0))
            {
                byte[] excess = this.excessBuffers.First.Value;
                this.excessBuffers.RemoveFirst();
                int bytesReceived = Math.Min(remainingBytes, excess.Length);
                remainingBytes -= bytesReceived;
                Array.Copy(excess, 0, this.pendingReceiveBuffer, totalBytesReceived, bytesReceived);
                totalBytesReceived += bytesReceived;
                this.AddExcess(excess, bytesReceived, true);
            }

            Task<int> task = this.pendingReceive.Task;
            if (totalBytesReceived > 0)
            {
                this.pendingReceive.SetResult(totalBytesReceived);
                this.pendingReceive = null;
            }

            return task;
        }

        public void Send(byte[] buffer)
        {
            int bytesReceived;
            if (this.pendingReceive != null)
            {
                bytesReceived = Math.Min(this.pendingReceiveBuffer.Length, buffer.Length);
                Array.Copy(buffer, 0, this.pendingReceiveBuffer, 0, bytesReceived);
            }
            else
            {
                bytesReceived = 0;
            }

            this.AddExcess(buffer, bytesReceived, false);

            if (bytesReceived > 0)
            {
                this.pendingReceive.SetResult(bytesReceived);
                this.pendingReceive = null;
            }
        }

        private void AddExcess(byte[] buffer, int bytesReceived, bool addFirst)
        {
            int remainingBytes = buffer.Length - bytesReceived;
            if (remainingBytes > 0)
            {
                byte[] excess = new byte[remainingBytes];
                Array.Copy(buffer, bytesReceived, excess, 0, remainingBytes);
                if (addFirst)
                {
                    this.excessBuffers.AddFirst(excess);
                }
                else
                {
                    this.excessBuffers.AddLast(excess);
                }
            }
        }
    }
}
