//-----------------------------------------------------------------------
// <copyright file="TraceCollectorInfo.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PlaSample
{
    using System;
    using System.Collections.Generic;
    using PlaLibrary;

    public class TraceCollectorInfo
    {
        public TraceCollectorInfo(string name)
        {
            this.Name = name;
            this.Providers = new List<ProviderInfo>();
        }

        public string Name { get; private set; }

        public string OutputPath { get; set; }

        public uint? BufferSizeInKB { get; set; }

        public bool? Circular { get; set; }

        public TimeSpan? FlushTimer { get; set; }

        public TimeSpan? MaxDuration { get; set; }

        public uint? MaxSizeInMB { get; set; }

        public uint? MaximumBuffers { get; set; }

        public uint? MinimumBuffers { get; set; }

        public bool? Segmented { get; set; }

        public IList<ProviderInfo> Providers { get; private set; }

        public ICollectorSet Create()
        {
            // Data collector set is the core abstraction for collecting diagnostic data.
            DataCollectorSet dcs = new DataCollectorSet();

            // Set base folder to place output files.
            dcs.RootPath = this.OutputPath;

            // Create a data collector for traces.
            ITraceDataCollector dc = (ITraceDataCollector)dcs.DataCollectors.CreateDataCollector(DataCollectorType.plaTrace);
            dc.name = this.Name + "_DC";
            dcs.DataCollectors.Add(dc);

            // Set output file name to use a pattern, as described at
            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa372131(v=vs.85).aspx .
            dc.FileName = this.Name;
            dc.FileNameFormat = AutoPathFormat.plaPattern;
            dc.FileNameFormatPattern = @"\-yyyyMMdd\-HHmmss";

            // Set various values (if present)
            SetValue(dc, this.BufferSizeInKB, (d, v) => d.BufferSize = v);
            SetValue(dc, this.Circular, (d, v) => d.LogCircular = v);
            SetValue(dc, this.FlushTimer, (d, v) => d.FlushTimer = (uint)v.TotalSeconds);
            SetValue(dc, this.MaximumBuffers, (d, v) => d.MaximumBuffers = v);
            SetValue(dc, this.MinimumBuffers, (d, v) => d.MinimumBuffers = v);
            SetValue(dc, this.MinimumBuffers, (d, v) => d.MinimumBuffers = v);

            SetValue(dcs, this.MaxDuration, (d, v) => d.SegmentMaxDuration = (uint)v.TotalSeconds);
            SetValue(dcs, this.MaxSizeInMB, (d, v) => d.SegmentMaxSize = (uint)v);
            SetValue(dcs, this.Segmented, (d, v) => d.Segment = v);

            // Build up the list of providers.
            foreach (ProviderInfo providerInfo in this.Providers)
            {
                TraceDataProvider provider = dc.TraceDataProviders.CreateTraceDataProvider();
                dc.TraceDataProviders.Add(provider);

                provider.Guid = providerInfo.Id;
                AddValue(provider.KeywordsAll, providerInfo.KeywordsAll);
                AddValue(provider.KeywordsAny, providerInfo.KeywordsAny);
                AddValue(provider.Level, providerInfo.Level);
            }

            // Now actually create (or modify existing) the set.
            dcs.Commit(this.Name, null, CommitMode.plaCreateOrModify);

            // Return an opaque wrapper with which the user can control the session.
            return new CollectorSetWrapper(dcs);
        }

        private static void SetValue<TClass, TValue>(TClass c, TValue? v, Action<TClass, TValue> setValue) where TValue : struct
        {
            if (v.HasValue)
            {
                setValue(c, v.Value);
            }
        }

        private static void AddValue<TValue>(IValueMap map, TValue? v) where TValue : struct
        {
            if (v.HasValue)
            {
                map.Add(v.Value);
            }
        }
    }
}
