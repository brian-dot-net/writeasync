//-----------------------------------------------------------------------
// <copyright file="CalculatorClientWithActivity.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Eventing;
    using System.Threading.Tasks;

    public class CalculatorClientWithActivity : ICalculatorClientAsync
    {
        private readonly ICalculatorClientAsync inner;
        private readonly ClientEventSource eventSource;
        private readonly Guid clientId;

        public CalculatorClientWithActivity(ICalculatorClientAsync inner, ClientEventSource eventSource, Guid clientId)
        {
            this.inner = inner;
            this.eventSource = eventSource;
            this.clientId = clientId;
        }

        public Task<double> AddAsync(double x, double y)
        {
            using (RequestScope scope = this.TraceStart())
            {
                try
                {
                    return this.TraceEnd(scope, this.inner.AddAsync(x, y));
                }
                catch (Exception e)
                {
                    this.eventSource.RequestError(this.clientId, e.GetType().FullName, e.Message);
                    throw;
                }
            }
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            using (RequestScope scope = this.TraceStart())
            {
                return this.TraceEnd(scope, this.inner.SubtractAsync(x, y));
            }
        }

        public Task<double> SquareRootAsync(double x)
        {
            using (RequestScope scope = this.TraceStart())
            {
                return this.TraceEnd(scope, this.inner.SquareRootAsync(x));
            }
        }

        private RequestScope TraceStart()
        {
            RequestScope scope = RequestScope.Start();
            this.eventSource.Request(this.clientId);
            return scope;
        }

        private Task<double> TraceEnd(RequestScope scope, Task<double> task)
        {
            return task.ContinueWith<double>(this.AfterRequest, scope.Id, TaskContinuationOptions.ExecuteSynchronously);
        }

        private double AfterRequest(Task<double> task, object objId)
        {
            using (RequestScope scope = RequestScope.Resume((Guid)objId))
            {
                if (task.IsFaulted)
                {
                    Exception error = task.Exception.InnerException;
                    this.eventSource.RequestError(this.clientId, error.GetType().FullName, error.Message);
                }
                else
                {
                    this.eventSource.RequestCompleted(this.clientId);
                }

                return task.Result;
            }
        }

        private struct RequestScope : IDisposable
        {
            private readonly Guid previousId;
            private readonly Guid id;

            private RequestScope(Guid id)
            {
                this.previousId = Trace.CorrelationManager.ActivityId;
                this.id = id;
                EventProvider.SetActivityId(ref this.id);
            }

            public Guid Id
            {
                get { return this.id; }
            }

            public static RequestScope Start()
            {
                return new RequestScope(Guid.NewGuid());
            }

            public static RequestScope Resume(Guid id)
            {
                return new RequestScope(id);
            }

            public void Dispose()
            {
                Guid resetId = this.previousId;
                EventProvider.SetActivityId(ref resetId);
            }
        }
    }
}
