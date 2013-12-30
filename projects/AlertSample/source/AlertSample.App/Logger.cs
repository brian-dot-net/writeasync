//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System.Diagnostics;

    internal sealed class Logger
    {
        private readonly TraceSource traceSource;

        public Logger()
        {
            this.traceSource = new TraceSource("AlertSample", SourceLevels.All);
            this.traceSource.Listeners.Add(new ConsoleTraceListener() { TraceOutputOptions = TraceOptions.ProcessId | TraceOptions.DateTime });
        }

        public void WriteInfo(string format, params object[] args)
        {
            this.traceSource.TraceInformation(format, args);
        }

        public void WriteError(string format, params object[] args)
        {
            this.traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
        }
    }
}
