//-----------------------------------------------------------------------
// <copyright file="CalculatorClientWithActivity.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class CalculatorClientWithActivity : ICalculatorClientAsync
    {
        private readonly ICalculatorClientAsync inner;
        private readonly ClientEventSource eventSource;

        public CalculatorClientWithActivity(ICalculatorClientAsync inner, ClientEventSource eventSource)
        {
            this.inner = inner;
            this.eventSource = eventSource;
        }

        public Task<double> AddAsync(double x, double y)
        {
            using (RequestScope scope = this.TraceStart())
            {
                return this.TraceEnd(scope, this.inner.AddAsync(x, y));
            }
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            return this.inner.SubtractAsync(x, y);
        }

        public Task<double> SquareRootAsync(double x)
        {
            return this.inner.SquareRootAsync(x);
        }

        private RequestScope TraceStart()
        {
            RequestScope scope = RequestScope.Start();
            this.eventSource.Request();
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
                this.eventSource.RequestCompleted();
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
                Trace.CorrelationManager.ActivityId = this.id;
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
                Trace.CorrelationManager.ActivityId = this.previousId;
            }
        }
    }
}
