//-----------------------------------------------------------------------
// <copyright file="KernelProcessSession.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TxSample
{
    using System;
    using Tx.Windows;

    public sealed class KernelProcessSession : IProcessEvents, IDisposable
    {
        private const ushort ProcessStartId = 1;
        private const ushort ProcessStopId = 2;
        private static readonly Guid KernelProcessProviderId = new Guid("{22FB2CD6-0E7B-422B-A0C7-2FAD1FD0E716}");

        private readonly string name;

        private ISessionController session;
        private IDisposable subscription;

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

            RealTimeTraceCollectorInfo info = new RealTimeTraceCollectorInfo(this.name);
            info.Providers.Add(new ProviderInfo(KernelProcessProviderId) { KeywordsAll = 0x10, Level = 4 });
            this.session = info.Create();
            this.session.Start();

            IObservable<EtwNativeEvent> stream = EtwObservable.FromSession(this.name);
            this.subscription = stream.Subscribe(e => this.OnNext(e));
        }

        public void Stop()
        {
            if (this.subscription == null)
            {
                throw new InvalidOperationException("The session is not started.");
            }

            this.subscription.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnNext(EtwNativeEvent traceEvent)
        {
            // An event is uniquely identified by 3 values -- provider ID, event ID, event version
            if (traceEvent.ProviderId == KernelProcessProviderId)
            {
                switch (traceEvent.Id)
                {
                    case ProcessStartId:
                        this.ReadProcessStartEvent(ref traceEvent);
                        break;
                    case ProcessStopId:
                        this.ReadProcessStopEvent(ref traceEvent);
                        break;
                }
            }
        }

        private void ReadProcessStartEvent(ref EtwNativeEvent traceEvent)
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
                    int processId = (int)traceEvent.ReadUInt32();
                    DateTime createTime = traceEvent.ReadFileTime();
                    traceEvent.ReadUInt32(); // ignore
                    traceEvent.ReadUInt32(); // ignore
                    string imageName = traceEvent.ReadUnicodeString();
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

        private void ReadProcessStopEvent(ref EtwNativeEvent traceEvent)
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
                        int processId = (int)traceEvent.ReadUInt32();
                        traceEvent.ReadFileTime(); // ignore
                        DateTime exitTime = traceEvent.ReadFileTime();
                        int exitCode = (int)traceEvent.ReadUInt32();
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
                using (this.subscription)
                {
                    if (this.session != null)
                    {
                        this.session.Stop();
                    }
                }
            }
        }
    }
}
