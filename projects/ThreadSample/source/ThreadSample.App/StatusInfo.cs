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
        private int bytesSent;
        private int bytesReceived;

        public StatusInfo()
        {
        }

        public int BytesSent
        {
            get { return Thread.VolatileRead(ref this.bytesSent); }
        }

        public int BytesReceived
        {
            get { return Thread.VolatileRead(ref this.bytesSent); }
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
