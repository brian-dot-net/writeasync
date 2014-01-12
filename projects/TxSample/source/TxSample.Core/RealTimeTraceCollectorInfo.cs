//-----------------------------------------------------------------------
// <copyright file="RealTimeTraceCollectorInfo.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample
{
    using System;
    using System.Collections.Generic;
    using PlaLibrary;

    public class RealTimeTraceCollectorInfo
    {
        public RealTimeTraceCollectorInfo(string name)
        {
            this.Name = name;
            this.Providers = new List<ProviderInfo>();
        }

        public string Name { get; private set; }

        public uint? BufferSizeInKB { get; set; }

        public TimeSpan? FlushTimer { get; set; }

        public uint? MaximumBuffers { get; set; }

        public uint? MinimumBuffers { get; set; }

        public IList<ProviderInfo> Providers { get; private set; }

        public ISessionController Create()
        {
            // Data collector set is the core abstraction for collecting diagnostic data.
            DataCollectorSet dcs = new DataCollectorSet();

            // Create a data collector for traces.
            ITraceDataCollector dc = (ITraceDataCollector)dcs.DataCollectors.CreateDataCollector(DataCollectorType.plaTrace);
            dc.name = this.Name;
            dcs.DataCollectors.Add(dc);

            // We need to set real-time mode and the session name
            dc.StreamMode = StreamMode.plaRealTime;
            dc.SessionName = this.Name;

            // Set various values (if present)
            SetValue(dc, this.BufferSizeInKB, (d, v) => d.BufferSize = v);
            SetValue(dc, this.FlushTimer, (d, v) => d.FlushTimer = (uint)v.TotalSeconds);
            SetValue(dc, this.MaximumBuffers, (d, v) => d.MaximumBuffers = v);
            SetValue(dc, this.MinimumBuffers, (d, v) => d.MinimumBuffers = v);

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
            // We explicitly specify the 'Session' namespace for real-time collectors.
            dcs.Commit("Session\\" + this.Name, null, CommitMode.plaCreateOrModify);

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
