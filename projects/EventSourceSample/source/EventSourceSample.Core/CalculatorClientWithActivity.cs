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
            return this.DoWithActivityAsync(x, y, (xx, yy, p) => p.AddAsync(xx, yy));
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            return this.DoWithActivityAsync(x, y, (xx, yy, p) => p.SubtractAsync(xx, yy));
        }

        public Task<double> SquareRootAsync(double x)
        {
            return this.DoWithActivityAsync(x, 0.0d, (xx, yy, p) => p.SquareRootAsync(xx));
        }

        private Task<double> DoWithActivityAsync(double x, double y, Func<double, double, ICalculatorClientAsync, Task<double>> doAsync)
        {
            using (RequestScope scope = this.TraceStart())
            {
                try
                {
                    return this.TraceEnd(scope, doAsync(x, y, this.inner));
                }
                catch (Exception e)
                {
                    this.TraceError(e);
                    throw;
                }
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
                    this.TraceError(task.Exception.InnerException);
                }
                else
                {
                    this.eventSource.RequestCompleted(this.clientId);
                }

                return task.Result;
            }
        }

        private void TraceError(Exception error)
        {
            this.eventSource.RequestError(this.clientId, error.GetType().FullName, error.Message);
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
