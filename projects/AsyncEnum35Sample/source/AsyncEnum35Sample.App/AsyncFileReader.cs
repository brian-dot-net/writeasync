//-----------------------------------------------------------------------
// <copyright file="AsyncFileReader.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnum35Sample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    internal sealed class AsyncFileReader
    {
        private readonly int bufferSize;

        public AsyncFileReader(int bufferSize)
        {
            this.bufferSize = bufferSize;
        }

        public IAsyncResult BeginReadAllBytes(string path, AsyncCallback callback, object state)
        {
            return new ReadAllBytesAsyncOperation(path, this.bufferSize).Start(callback, state);
        }

        public byte[] EndReadAllBytes(IAsyncResult result)
        {
            return ReadAllBytesAsyncOperation.End(result);
        }

        private sealed class ReadAllBytesAsyncOperation : AsyncOperation<byte[]>
        {
            private readonly string path;
            private readonly int bufferSize;

            private FileStream stream;
            private byte[] buffer;
            private int bytesRead;

            public ReadAllBytesAsyncOperation(string path, int bufferSize)
            {
                this.path = path;
                this.bufferSize = bufferSize;
            }

            protected override IEnumerator<Step> Steps()
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (this.stream = new FileStream(this.path, FileMode.Open, FileAccess.Read, FileShare.Read, this.bufferSize, true))
                {
                    this.buffer = new byte[this.bufferSize];
                    do
                    {
                        Console.WriteLine("[{0}] Reading...", Thread.CurrentThread.ManagedThreadId);
                        yield return Step.Await(
                            this,
                            (thisPtr, c, s) => thisPtr.stream.BeginRead(thisPtr.buffer, 0, thisPtr.bufferSize, c, s),
                            (thisPtr, r) => thisPtr.bytesRead = thisPtr.stream.EndRead(r));
                        Console.WriteLine("[{0}] Read {1} bytes.", Thread.CurrentThread.ManagedThreadId, this.bytesRead);
                        if (this.bytesRead > 0)
                        {
                            outputStream.Write(this.buffer, 0, this.bytesRead);
                        }
                    }
                    while (this.bytesRead > 0);

                    this.Result = outputStream.ToArray();
                }
            }
        }
    }
}
