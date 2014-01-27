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

        [Event((int)QueueEventId.Enqueue, Level = EventLevel.Informational, Message = "Enqueue {0}.")]
        public void Enqueue(Guid id)
        {
            this.WriteEvent((int)QueueEventId.Enqueue, id);
        }

        [Event((int)QueueEventId.Dequeue, Level = EventLevel.Informational, Message = "Dequeue {0} started.")]
        public void Dequeue(Guid id)
        {
            this.WriteEvent((int)QueueEventId.Dequeue, id);
        }

        [Event((int)QueueEventId.DequeueCompleted, Level = EventLevel.Informational, Message = "Dequeue {0} completed.")]
        public void DequeueCompleted(Guid id)
        {
            this.WriteEvent((int)QueueEventId.DequeueCompleted, id);
        }

        [Event((int)QueueEventId.QueueDispose, Level = EventLevel.Informational, Message = "Queue dispose {0}.")]
        public void QueueDispose(Guid id)
        {
            this.WriteEvent((int)QueueEventId.QueueDispose, id);
        }
    }
}
