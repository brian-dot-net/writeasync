// <copyright file="WrappedMemoryStream.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using System.IO;

    internal sealed class WrappedMemoryStream : MemoryStream
    {
        public WrappedMemoryStream(byte[] buffer)
            : base(buffer)
        {
        }

        public int DisposeCount { get; private set; }

        protected override void Dispose(bool disposing)
        {
            ++this.DisposeCount;
            base.Dispose(disposing);
        }
    }
}
