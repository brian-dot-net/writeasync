//-----------------------------------------------------------------------
// <copyright file="KernelProcessSession.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Diagnostics.Tracing;
    using Diagnostics.Tracing.Parsers;

    public sealed class KernelProcessSession : IProcessEvents, IDisposable
    {
        private const ushort ProcessStartId = 1;
        private const ushort ProcessStopId = 2;
        private static readonly Guid KernelProcessProviderId = new Guid("{22FB2CD6-0E7B-422B-A0C7-2FAD1FD0E716}");

        private readonly string name;

        private TraceEventSession session;
        private ETWTraceEventSource eventSource;
        private RegisteredTraceEventParser parser;
        private CancellationTokenSource cts;
        private Task processTask;

        public KernelProcessSession(string name)
        {
            this.name = name;
        }

        public event EventHandler<ProcessEventArgs> ProcessStarted;

        public event EventHandler<ProcessEventArgs> ProcessStopped;

        public void Start()
        {
            if (this.session != null)
            {
                throw new InvalidOperationException("The session is already started.");
            }

            CloseExistingSession(this.name);
            this.session = new TraceEventSession(this.name, null);
            this.session.StopOnDispose = true;
            this.session.EnableProvider(KernelProcessProviderId, TraceEventLevel.Informational, ulong.MaxValue, 0x10);
            this.eventSource = new ETWTraceEventSource(this.name, TraceEventSourceType.Session);
            this.parser = new RegisteredTraceEventParser(this.eventSource);
            this.parser.All += this.OnEventRead;

            // Process() blocks until the session is shut down, so we'll run this on another thread.
            this.cts = new CancellationTokenSource();
            this.processTask = Task.Factory.StartNew(() => this.eventSource.Process(), TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            if (this.cts == null)
            {
                throw new InvalidOperationException("The session is not started.");
            }

            this.cts.Cancel();
            this.eventSource.Close();
            this.processTask.Wait();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void CloseExistingSession(string name)
        {
            HashSet<string> sessionNames = new HashSet<string>(TraceEventSession.GetActiveSessionNames());
            if (sessionNames.Contains(name))
            {
                using (TraceEventSession existingSession = new TraceEventSession(name))
                {
                    existingSession.StopOnDispose = true;
                }
            }
        }

        private void OnEventRead(TraceEvent traceEvent)
        {
            if (this.cts.IsCancellationRequested)
            {
                this.eventSource.StopProcessing();
                return;
            }

            // An event is uniquely identified by 3 values -- provider ID, event ID, event version
            if (traceEvent.ProviderGuid == KernelProcessProviderId)
            {
                switch ((ushort)traceEvent.ID)
                {
                    case ProcessStartId:
                        this.ReadProcessStartEvent(traceEvent);
                        break;
                    case ProcessStopId:
                        this.ReadProcessStopEvent(traceEvent);
                        break;
                }
            }
        }

        private void ReadProcessStartEvent(TraceEvent traceEvent)
        {
            if (traceEvent.Version == 0)
            {
                // <data name="ProcessID" inType="win:UInt32" outType="win:PID"></data>
                // <data name="CreateTime" inType="win:FILETIME" outType="xs:dateTime"></data>
                // <data name="ParentProcessID" inType="win:UInt32" outType="win:PID"></data>
                // <data name="SessionID" inType="win:UInt32" outType="xs:unsignedInt"></data>
                // <data name="ImageName" inType="win:UnicodeString" outType="xs:string"></data>
                EventHandler<ProcessEventArgs> handler = this.ProcessStarted;
                if (handler != null)
                {
                    int processId = traceEvent.GetInt32At(0);
                    DateTime createTime = DateTime.FromFileTime(traceEvent.GetInt64At(4));
                    string imageName = traceEvent.GetUnicodeStringAt(20);
                    ProcessEventArgs e = new ProcessEventArgs()
                    {
                        Id = processId,
                        ImageName = imageName,
                        Timestamp = createTime
                    };

                    handler(this, e);
                }
            }
        }

        private void ReadProcessStopEvent(TraceEvent traceEvent)
        {
            switch (traceEvent.Version)
            {
                case 0:
                case 1:
                    // Both version 0 and version 1 have the same initial fields:
                    //
                    // <data name="ProcessID" inType="win:UInt32" outType="win:PID"></data>
                    // <data name="CreateTime" inType="win:FILETIME" outType="xs:dateTime"></data>
                    // <data name="ExitTime" inType="win:FILETIME" outType="xs:dateTime"></data>
                    // <data name="ExitCode" inType="win:UInt32" outType="xs:unsignedInt"></data>
                    EventHandler<ProcessEventArgs> handler = this.ProcessStopped;
                    if (handler != null)
                    {
                        int processId = traceEvent.GetInt32At(0);
                        DateTime exitTime = DateTime.FromFileTime(traceEvent.GetInt64At(12));
                        int exitCode = traceEvent.GetInt32At(20);
                        ProcessEventArgs e = new ProcessEventArgs()
                        {
                            ExitCode = exitCode,
                            Id = processId,
                            Timestamp = exitTime
                        };

                        handler(this, e);
                    }

                    break;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (this.session)
                using (this.eventSource)
                using (this.cts)
                {
                }
            }
        }
    }
}
