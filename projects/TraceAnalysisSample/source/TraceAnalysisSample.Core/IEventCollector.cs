//-----------------------------------------------------------------------
// <copyright file="IEventCollector.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;

    public interface IEventCollector
    {
        void OnStart(int eventId, Guid instanceId, DateTime startTime);

        void OnEnd(int eventId, Guid instanceId, DateTime endTime);
    }
}
