//-----------------------------------------------------------------------
// <copyright file="ProcessEventArgs.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample
{
    using System;

    public class ProcessEventArgs : EventArgs
    {
        public ProcessEventArgs()
        {
        }

        public string ImageName { get; set; }

        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public int ExitCode { get; set; }
    }
}
