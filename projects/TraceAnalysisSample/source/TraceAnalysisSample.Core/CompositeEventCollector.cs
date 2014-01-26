//-----------------------------------------------------------------------
// <copyright file="CompositeEventCollector.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CompositeEventCollector : IEventCollector
    {
        private readonly IEventCollector[] innerCollectors;

        public CompositeEventCollector(params IEventCollector[] innerCollectors)
        {
            this.innerCollectors = innerCollectors.ToArray();
        }

        public void OnStart(int eventId, Guid instanceId, DateTime startTime)
        {
            foreach (IEventCollector inner in this.innerCollectors)
            {
                inner.OnStart(eventId, instanceId, startTime);
            }
        }

        public void OnEnd(int eventId, Guid instanceId, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
