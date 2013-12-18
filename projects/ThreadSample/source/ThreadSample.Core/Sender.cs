//-----------------------------------------------------------------------
// <copyright file="Sender.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ThreadSample
{
    using System;
    using System.IO;
    using System.IO.Pipes;
    using System.Threading;
    using System.Threading.Tasks;

    public class Sender
    {
        private readonly string name;
        private readonly bool useDedicatedThread;

        public Sender(string name, bool useDedicatedThread)
        {
            this.name = name;
            this.useDedicatedThread = useDedicatedThread;
        }

        public event EventHandler DataSent;

        public Task SendAsync(CancellationToken token)
        {
            if (this.useDedicatedThread)
            {
                return Task.Factory.StartNew(() => this.SendInner(token), TaskCreationOptions.LongRunning);
            }
            else
            {
                return this.SendInnerAsync(token);
            }
        }

        private void SendInner(CancellationToken token)
        {
            using (NamedPipeClientStream stream = new NamedPipeClientStream(".", this.name, PipeDirection.Out, PipeOptions.None))
            {
                try
                {
                    stream.Connect();
                    byte[] buffer = new byte[1];
                    byte b = 0;
                    while (!token.IsCancellationRequested)
                    {
                        ++b;
                        if (b == 0)
                        {
                            b = 1;
                        }

                        buffer[0] = b;
                        stream.Write(buffer, 0, buffer.Length);
                        this.OnSent();
                    }
                }
                catch (IOException)
                {
                    // Pipe is broken or disconnected.
                }
            }
        }

        private void OnSent()
        {
            EventHandler handler = this.DataSent;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private async Task SendInnerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // TODO
                await Task.Delay(1000);
            }
        }
    }
}
