//-----------------------------------------------------------------------
// <copyright file="ClientEventSource.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Diagnostics.Tracing;

    [EventSource(Guid = ProviderIdString)]
    public class ClientEventSource : EventSource
    {
        public const string ProviderIdString = "{0745B9D3-BC9A-4C33-953C-77DE89336B0D}";
        public static readonly Guid ProviderId = new Guid(ProviderIdString);

        private static readonly ClientEventSource SingletonInstance = new ClientEventSource();

        private ClientEventSource()
        {
        }

        public static ClientEventSource Instance
        {
            get { return SingletonInstance; }
        }

        [Event((int)ClientEventId.Add, Level = EventLevel.Informational, Keywords = Keywords.Basic, Message = "Adding {0} and {1}.")]
        public void Add(double x, double y)
        {
            this.WriteEvent((int)ClientEventId.Add, x, y);
        }

        [Event((int)ClientEventId.Subtract, Level = EventLevel.Informational, Keywords = Keywords.Basic, Message = "Subtracting {0} and {1}.")]
        public void Subtract(double x, double y)
        {
            this.WriteEvent((int)ClientEventId.Subtract, x, y);
        }

        [Event((int)ClientEventId.SquareRoot, Level = EventLevel.Informational, Keywords = Keywords.Advanced, Message = "Finding square root of {0}.")]
        public void SquareRoot(double x)
        {
            this.WriteEvent((int)ClientEventId.SquareRoot, x);
        }

        [Event((int)ClientEventId.Request, Level = EventLevel.Informational, Keywords = Keywords.Request, Message = "Request invoked.")]
        public void Request()
        {
            this.WriteEvent((int)ClientEventId.Request);
        }

        [Event((int)ClientEventId.RequestCompleted, Level = EventLevel.Informational, Keywords = Keywords.Request, Message = "Request invoked.")]
        public void RequestCompleted()
        {
            this.WriteEvent((int)ClientEventId.RequestCompleted);
        }

        public static class Keywords
        {
            public const EventKeywords Basic = (EventKeywords)0x1;
            public const EventKeywords Advanced = (EventKeywords)0x2;
            public const EventKeywords Request = (EventKeywords)0x4;
        }
    }
}
