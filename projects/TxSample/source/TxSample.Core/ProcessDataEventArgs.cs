//-----------------------------------------------------------------------
// <copyright file="ProcessDataEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample
{
    using System;

    public class ProcessDataEventArgs : EventArgs
    {
        public ProcessDataEventArgs(ProcessData data)
        {
            this.Data = data;
        }

        public ProcessData Data { get; private set; }
    }
}
