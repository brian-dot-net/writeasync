//-----------------------------------------------------------------------
// <copyright file="StatusInfo.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System.Threading;

    internal sealed class StatusInfo
    {
        private int senderCount;
        private int bytesSent;
        private int bytesReceived;

        public StatusInfo()
        {
        }

        public int SenderCount
        {
            get { return this.senderCount; }
        }

        public int BytesSent
        {
            get { return Thread.VolatileRead(ref this.bytesSent); }
        }

        public int BytesReceived
        {
            get { return Thread.VolatileRead(ref this.bytesSent); }
        }

        public void OnSenderAdded()
        {
            ++this.senderCount;
        }

        public void OnSent()
        {
            Interlocked.Increment(ref this.bytesSent);
        }

        public void OnReceived()
        {
            Interlocked.Increment(ref this.bytesReceived);
        }
    }
}
