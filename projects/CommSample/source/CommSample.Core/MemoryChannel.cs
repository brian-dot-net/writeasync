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

    public class MemoryChannel : IDisposable
    {
        private readonly LinkedList<byte[]> excessBuffers;

        private ReceiveRequest pendingReceive;
        private bool disposed;

        public MemoryChannel()
        {
            this.excessBuffers = new LinkedList<byte[]>();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<int> ReceiveAsync(byte[] buffer)
        {
            this.ThrowIfDisposed();
            if (this.pendingReceive != null)
            {
                throw new InvalidOperationException("A receive operation is already in progress.");
            }

            this.pendingReceive = new ReceiveRequest(buffer);

            while ((this.excessBuffers.Count > 0) && (this.pendingReceive.RemainingBytes > 0))
            {
                byte[] excess = this.excessBuffers.First.Value;
                this.excessBuffers.RemoveFirst();
                int bytesReceived = this.pendingReceive.AddData(excess);
                this.AddExcess(excess, bytesReceived, true);
            }

            Task<int> task = this.pendingReceive.Task;
            if (this.pendingReceive.TryComplete(false))
            {
                this.pendingReceive = null;
            }

            return task;
        }

        public void Send(byte[] buffer)
        {
            this.ThrowIfDisposed();
            int bytesReceived;
            if (this.pendingReceive != null)
            {
                bytesReceived = this.pendingReceive.AddData(buffer);
            }
            else
            {
                bytesReceived = 0;
            }

            this.AddExcess(buffer, bytesReceived, false);

            if (bytesReceived > 0)
            {
                if (this.pendingReceive.TryComplete(false))
                {
                    this.pendingReceive = null;
                }
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

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    if (this.pendingReceive != null)
                    {
                        this.pendingReceive.TryComplete(true);
                        this.pendingReceive = null;
                    }

                    this.disposed = true;
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("MemoryChannel");
            }
        }

        private sealed class ReceiveRequest
        {
            private readonly TaskCompletionSource<int> task;
            private readonly byte[] buffer;

            private int totalBytesReceived;

            public ReceiveRequest(byte[] buffer)
            {
                this.buffer = buffer;
                this.task = new TaskCompletionSource<int>();
                this.RemainingBytes = this.buffer.Length;
            }

            public Task<int> Task
            {
                get { return this.task.Task; }
            }

            public int RemainingBytes { get; private set; }

            public int AddData(byte[] sendBuffer)
            {
                int bytesReceived = Math.Min(this.RemainingBytes, sendBuffer.Length);
                this.RemainingBytes -= bytesReceived;
                Array.Copy(sendBuffer, 0, this.buffer, this.totalBytesReceived, bytesReceived);
                this.totalBytesReceived += bytesReceived;
                return bytesReceived;
            }

            public bool TryComplete(bool disposing)
            {
                bool succeeded = false;
                if (disposing || (this.totalBytesReceived > 0))
                {
                    succeeded = true;
                    this.task.SetResult(this.totalBytesReceived);
                }

                return succeeded;
            }
        }
    }
}
