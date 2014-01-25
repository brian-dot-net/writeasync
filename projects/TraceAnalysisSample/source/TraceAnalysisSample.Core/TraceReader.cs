//-----------------------------------------------------------------------
// <copyright file="TraceReader.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace TraceAnalysisSample
{
    using System;
    using System.Threading.Tasks;
    using Tx.Windows;

    public class TraceReader
    {
        private readonly IObservable<EtwNativeEvent> eventStream;

        public TraceReader(string fileName)
        {
            this.eventStream = EtwObservable.FromFiles(fileName);
        }

        public async Task ReadAsync()
        {
            Subscriber subscriber = new Subscriber();
            using (this.eventStream.Subscribe(subscriber))
            {
                await subscriber.WaitAsync();
            }
        }

        private sealed class Subscriber : IObserver<EtwNativeEvent>
        {
            private static readonly Guid SampleProviderId = new Guid("{0745B9D3-BC9A-4C33-953C-77DE89336B0D}");

            private readonly TaskCompletionSource<bool> tcs;

            public Subscriber()
            {
                this.tcs = new TaskCompletionSource<bool>();
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
                            this.ReadRequest(ref value);
                            break;
                        case 21:
                            this.ReadRequestCompleted(ref value);
                            break;
                        case 22:
                            this.ReadRequestError(ref value);
                            break;
                    }
                }
            }

            private void ReadRequest(ref EtwNativeEvent value)
            {
                // TODO
            }

            private void ReadRequestCompleted(ref EtwNativeEvent value)
            {
                // TODO
            }

            private void ReadRequestError(ref EtwNativeEvent value)
            {
                // TODO
            }
        }
    }
}
