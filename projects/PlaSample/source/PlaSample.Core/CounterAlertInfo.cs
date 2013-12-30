//-----------------------------------------------------------------------
// <copyright file="CounterAlertInfo.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PlaSample
{
    using System;
    using System.Collections.Generic;
    using PlaLibrary;

    public class CounterAlertInfo
    {
        public CounterAlertInfo(string name)
        {
            this.Name = name;
            this.Thresholds = new List<CounterThreshold>();
        }

        public string Name { get; private set; }

        public IList<CounterThreshold> Thresholds { get; private set; }

        public TimeSpan? SampleInterval { get; set; }

        public ICollectorSet Create()
        {
            // Data collector set is the core abstraction for collecting diagnostic data.
            DataCollectorSet dcs = new DataCollectorSet();

            // Create a data collector for a perf counter alert.
            IAlertDataCollector dc = (IAlertDataCollector)dcs.DataCollectors.CreateDataCollector(DataCollectorType.plaAlert);
            dc.name = this.Name + "_DC";
            dcs.DataCollectors.Add(dc);

            // Set sample interval, if present.
            if (this.SampleInterval.HasValue)
            {
                dc.SampleInterval = (uint)this.SampleInterval.Value.TotalSeconds;
            }

            // Set collector to create an event log entry when threshold is reached.
            dc.EventLog = true;

            // Build up the list of alert thresholds.
            string[] alertThresholds = new string[this.Thresholds.Count];
            for (int i = 0; i < this.Thresholds.Count; ++i)
            {
                alertThresholds[i] = this.Thresholds[i].ToString();
            }

            dc.AlertThresholds = alertThresholds;

            // Now actually create (or modify existing) the set.
            dcs.Commit(this.Name, null, CommitMode.plaCreateOrModify);

            // Return an opaque wrapper with which the user can control the session.
            return new CollectorSetWrapper(dcs);
        }
    }
}
