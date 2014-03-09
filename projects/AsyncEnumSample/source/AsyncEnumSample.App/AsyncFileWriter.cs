//-----------------------------------------------------------------------
// <copyright file="AsyncFileWriter.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AsyncEnumSample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class AsyncFileWriter
    {
        private readonly int bufferSize;

        public AsyncFileWriter(int bufferSize)
        {
            this.bufferSize = bufferSize;
        }

        public Task WriteAllBytesAsync(string path, byte[] bytes)
        {
            return new WriteAllBytesAsyncOperation(path, bytes, this.bufferSize).Start();
        }

        private sealed class WriteAllBytesAsyncOperation : AsyncOperation<bool>
        {
            private readonly string path;
            private readonly byte[] bytes;
            private readonly int bufferSize;

            private FileStream stream;
            private int offset;
            private int bytesToWrite;

            public WriteAllBytesAsyncOperation(string path, byte[] bytes, int bufferSize)
            {
                this.path = path;
                this.bytes = bytes;
                this.bufferSize = bufferSize;
            }

            protected override IEnumerator<Step> Steps()
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (this.stream = new FileStream(this.path, FileMode.Create, FileAccess.Write, FileShare.None, this.bufferSize, true))
                {
                    do
                    {
                        this.bytesToWrite = Math.Min(this.bufferSize, this.bytes.Length - this.offset);
                        Console.WriteLine("[{0}] Writing...", Thread.CurrentThread.ManagedThreadId);
                        yield return Step.Await(
                            this,
                            (thisPtr, c, s) => thisPtr.stream.BeginWrite(thisPtr.bytes, thisPtr.offset, thisPtr.bytesToWrite, c, s),
                            (thisPtr, r) => thisPtr.stream.EndWrite(r));
                        Console.WriteLine("[{0}] Wrote {1} bytes.", Thread.CurrentThread.ManagedThreadId, this.bytesToWrite);
                        this.offset += this.bytesToWrite;
                    }
                    while (this.offset < this.bytes.Length);
                }
            }
        }
    }
}
