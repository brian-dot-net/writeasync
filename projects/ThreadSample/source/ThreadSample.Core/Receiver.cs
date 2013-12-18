//-----------------------------------------------------------------------
// <copyright file="Receiver.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System.IO;
    using System.IO.Pipes;
    using System.Threading;
    using System.Threading.Tasks;

    public class Receiver
    {
        private readonly string name;
        private readonly bool useDedicatedThread;

        public Receiver(string name, bool useDedicatedThread)
        {
            this.name = name;
            this.useDedicatedThread = useDedicatedThread;
        }

        public Task ReceiveAsync(CancellationToken token)
        {
            if (this.useDedicatedThread)
            {
                return Task.Factory.StartNew(() => this.ReceiveInner(token), TaskCreationOptions.LongRunning);
            }
            else
            {
                return this.ReceiveInnerAsync(token);
            }
        }

        private static void Disconnect(NamedPipeServerStream stream)
        {
            if (stream.IsConnected)
            {
                stream.Disconnect();
            }
        }

        private void ReceiveInner(CancellationToken token)
        {
            using (NamedPipeServerStream stream = new NamedPipeServerStream(this.name, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.None))
            {
                token.Register(s => Disconnect((NamedPipeServerStream)s), stream);
                try
                {
                    stream.WaitForConnection();
                    byte[] buffer = new byte[1];
                    int bytesRead;
                    do
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                    }
                    while (bytesRead > 0);
                }
                catch (IOException)
                {
                    // Pipe is broken or disconnected.
                }
            }
        }

        private async Task ReceiveInnerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                await Task.Delay(1000);
            }
        }
    }
}
