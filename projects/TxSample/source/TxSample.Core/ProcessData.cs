//-----------------------------------------------------------------------
// <copyright file="ProcessData.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample
{
    using System;

    public class ProcessData
    {
        public ProcessData()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime ExitTime { get; set; }

        public int ExitCode { get; set; }
    }
}