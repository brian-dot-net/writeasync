//-----------------------------------------------------------------------
// <copyright file="Alert.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using System.Runtime.InteropServices;
    using System.Xml;

    internal sealed class Alert
    {
        private readonly ICollectorSet collector;

        private EventLogWatcher watcher;

        public Alert(string name, double lowerBound, double upperBound)
        {
            CounterAlertInfo info = new CounterAlertInfo("AlertSample");
            info.SampleInterval = TimeSpan.FromSeconds(5.0d);

            CounterName counterName = new CounterName() { Category = "Process", Counter = "IO Data Operations/sec", Instance = "AlertSample.App#1" };
            info.Thresholds.Add(new CounterThreshold() { Name = counterName, Condition = ThresholdCondition.Below, Value = lowerBound });
            info.Thresholds.Add(new CounterThreshold() { Name = counterName, Condition = ThresholdCondition.Above, Value = upperBound });

            this.collector = info.Create();
        }

        public event EventHandler<ThresholdEventArgs> ThresholdReached;

        public void Start()
        {
            string queryText = GetQueryText();
            this.watcher = new EventLogWatcher(new EventLogQuery("Microsoft-Windows-Diagnosis-PLA/Operational", PathType.LogName, queryText));
            this.watcher.EventRecordWritten += this.OnEventWritten;
            this.watcher.Enabled = true;
            this.collector.Start();
        }

        public void Stop()
        {
            List<Exception> exceptions = new List<Exception>();
            
            this.watcher.EventRecordWritten -= this.OnEventWritten;
            this.watcher.Enabled = false;
            this.watcher.Dispose();

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

        private static string GetQueryText()
        {
            return
                "*[System[(EventID=2031) and TimeCreated[@SystemTime>'" +
                XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc) +
                "']]]";
        }

        private void OnEventWritten(object sender, EventRecordWrittenEventArgs e)
        {
            // Event has five properties:
            //  [0] Counter name
            //  [1] Counter value
            //  [2] Operator
            //  [3] Counter threshold
            //  [4] Message
            string operatorString = (string)e.EventRecord.Properties[2].Value;
            switch (operatorString[0])
            {
                case '<':
                    this.OnThresholdReached(true);
                    break;
                case '>':
                    this.OnThresholdReached(false);
                    break;
            }
        }

        private void OnThresholdReached(bool isLowerBound)
        {
            EventHandler<ThresholdEventArgs> handler = this.ThresholdReached;
            if (handler != null)
            {
                handler(this, new ThresholdEventArgs(isLowerBound));
            }
        }
    }
}
