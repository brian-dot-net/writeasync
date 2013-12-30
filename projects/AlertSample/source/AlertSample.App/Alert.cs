//-----------------------------------------------------------------------
// <copyright file="Alert.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal sealed class Alert
    {
        private readonly ICollectorSet collector;

        public Alert(string name, double lowerBound, double upperBound)
        {
            CounterAlertInfo info = new CounterAlertInfo("AlertSample");
            info.SampleInterval = TimeSpan.FromSeconds(5.0d);

            CounterName counterName = new CounterName() { Category = "Process", Counter = "IO Data Operations/sec", Instance = "AlertSample.App#1" };
            info.Thresholds.Add(new CounterThreshold() { Name = counterName, Condition = ThresholdCondition.Below, Value = lowerBound });
            info.Thresholds.Add(new CounterThreshold() { Name = counterName, Condition = ThresholdCondition.Above, Value = upperBound });

            this.collector = info.Create();
        }

        public void Start()
        {
            this.collector.Start();
        }

        public void Stop()
        {
            List<Exception> exceptions = new List<Exception>();
            try
            {
                this.collector.Stop();
            }
            catch (COMException e)
            {
                exceptions.Add(e);
            }

            try
            {
                this.collector.Delete();
            }
            catch (COMException e)
            {
                exceptions.Add(e);
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
