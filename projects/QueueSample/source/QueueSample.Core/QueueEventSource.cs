//-----------------------------------------------------------------------
// <copyright file="QueueEventSource.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace QueueSample
{
    using System;
    using System.Diagnostics.Tracing;

    [EventSource(Guid = ProviderIdString)]
    public class QueueEventSource : EventSource
    {
        public const string ProviderIdString = "{B1E1772F-11D9-4986-87BA-80A2A2AE7694}";
        public static readonly Guid ProviderId = new Guid(ProviderIdString);

        private static readonly QueueEventSource SingletonInstance = new QueueEventSource();

        private QueueEventSource()
        {
        }

        public static QueueEventSource Instance
        {
            get { return SingletonInstance; }
        }
    }
}
