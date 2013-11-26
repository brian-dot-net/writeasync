//-----------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Threading.Tasks;

    internal sealed class Receiver
    {
        private readonly MemoryChannel channel;
        private readonly Logger logger;
        private readonly int bufferSize;

        public Receiver(MemoryChannel channel, Logger logger, int bufferSize)
        {
            this.channel = channel;
            this.logger = logger;
            this.bufferSize = bufferSize;
        }

        public event EventHandler<DataEventArgs> DataReceived;

        public async Task RunAsync()
        {
            this.logger.WriteLine("Receiver starting...");
            byte[] buffer = new byte[this.bufferSize];

            long totalBytes = 0;
            int bytesRead;
            do
            {
                bytesRead = await this.channel.ReceiveAsync(buffer);
                this.OnDataReceived(buffer, bytesRead);
                totalBytes += bytesRead;
            }
            while (bytesRead > 0);

            this.logger.WriteLine("Receiver completed. Received {0} bytes.", totalBytes);
        }

        private void OnDataReceived(byte[] buffer, int bytesRead)
        {
            EventHandler<DataEventArgs> handler = this.DataReceived;
            if (handler != null)
            {
                handler(this, new DataEventArgs(buffer, bytesRead));
            }
        }
    }
}
