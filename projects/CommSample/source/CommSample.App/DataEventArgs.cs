//-----------------------------------------------------------------------
// <copyright file="DataEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;

    internal sealed class DataEventArgs : EventArgs
    {
        public DataEventArgs(byte[] buffer)
        {
            this.Buffer = buffer;
        }

        public byte[] Buffer { get; private set; }
    }
}
