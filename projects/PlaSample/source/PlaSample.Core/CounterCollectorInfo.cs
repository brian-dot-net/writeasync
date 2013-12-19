//-----------------------------------------------------------------------
// <copyright file="CounterCollectorInfo.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PlaSample
{
    using System;
    using System.Collections.Generic;
    using PlaLibrary;

    public class CounterCollectorInfo
    {
        public CounterCollectorInfo(string name)
        {
            this.Name = name;
            this.CounterNames = new List<CounterName>();
        }

        public string Name { get; private set; }

        public string OutputPath { get; set; }

        public TimeSpan? SampleInterval { get; set; }

        public LogFileFormat? LogFileFormat { get; set; }

        public IList<CounterName> CounterNames { get; private set; }

        public ICollectorSet Create()
        {
            // Data collector set is the core abstraction for collecting diagnostic data.
            DataCollectorSet dcs = new DataCollectorSet();

            // Set base folder to place output files.
            dcs.RootPath = this.OutputPath;

            // Create a data collector for perf counters.
            IPerformanceCounterDataCollector dc = (IPerformanceCounterDataCollector)dcs.DataCollectors.CreateDataCollector(DataCollectorType.plaPerformanceCounter);
            dc.name = this.Name + "_DC";
            dcs.DataCollectors.Add(dc);

            // Set output file name to use a pattern, as described at
            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa372131(v=vs.85).aspx .
            dc.FileName = this.Name;
            dc.FileNameFormat = AutoPathFormat.plaPattern;
            dc.FileNameFormatPattern = @"\-yyyyMMdd\-HHmmss";

            // Set sample interval, if present.
            if (this.SampleInterval.HasValue)
            {
                dc.SampleInterval = (uint)this.SampleInterval.Value.TotalSeconds;
            }

            // Set log file format, if present.
            if (this.LogFileFormat.HasValue)
            {
                dc.LogFileFormat = (FileFormat)this.LogFileFormat.Value;
            }

            // Build up the list of performance counters.
            string[] counterNames = new string[this.CounterNames.Count];
            for (int i = 0; i < this.CounterNames.Count; ++i)
            {
                counterNames[i] = this.CounterNames[i].ToString();
            }

            dc.PerformanceCounters = counterNames;

            // Now actually create (or modify existing) the set.
            dcs.Commit(this.Name, null, CommitMode.plaCreateOrModify);

            // Return an opaque wrapper with which the user can control the session.
            return new CollectorSetWrapper(dcs);
        }
    }
}
