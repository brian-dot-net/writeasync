//-----------------------------------------------------------------------
// <copyright file="TraceReader.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Tx.Windows;

    public class TraceReader
    {
        private readonly IObservable<EtwNativeEvent> eventStream;

        public TraceReader(string fileName)
        {
            this.eventStream = EtwObservable.FromFiles(fileName);
        }

        public async Task ReadAsync(EventWindowCollector collector)
        {
            Subscriber subscriber = new Subscriber(collector);
            using (this.eventStream.Subscribe(subscriber))
            {
                await subscriber.WaitAsync();
                collector.CloseWindow();
            }
        }

        private sealed class Subscriber : IObserver<EtwNativeEvent>
        {
            private static readonly Guid SampleProviderId = new Guid("{0745B9D3-BC9A-4C33-953C-77DE89336B0D}");

            private readonly TaskCompletionSource<bool> tcs;
            private readonly EventWindowCollector collector;

            public Subscriber(EventWindowCollector collector)
            {
                this.tcs = new TaskCompletionSource<bool>();
                this.collector = collector;
            }

            public Task WaitAsync()
            {
                return this.tcs.Task;
            }

            public void OnCompleted()
            {
                this.tcs.TrySetResult(false);
            }

            public void OnError(Exception error)
            {
                this.tcs.TrySetException(error);
            }

            public void OnNext(EtwNativeEvent value)
            {
                if (value.ProviderId == SampleProviderId)
                {
                    switch (value.Id)
                    {
                        case 20:
                            this.OnStarted(ref value);
                            break;
                        case 21:
                        case 22:
                            this.OnCompleted(ref value);
                            break;
                    }
                }
            }

            private void OnStarted(ref EtwNativeEvent value)
            {
                this.collector.OnStart(1, value.ActivityId, value.TimeStamp.DateTime);
            }

            private void OnCompleted(ref EtwNativeEvent value)
            {
                this.collector.OnEnd(1, value.ActivityId, value.TimeStamp.DateTime);
            }
        }
    }
}
