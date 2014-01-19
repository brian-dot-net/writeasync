//-----------------------------------------------------------------------
// <copyright file="CalculatorClientWithActivity.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
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
            this.TraceStart();
            return this.TraceEnd(this.inner.AddAsync(x, y));
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            return this.inner.SubtractAsync(x, y);
        }

        public Task<double> SquareRootAsync(double x)
        {
            return this.inner.SquareRootAsync(x);
        }

        private void TraceStart()
        {
            this.eventSource.Request();
        }

        private Task<double> TraceEnd(Task<double> task)
        {
            return task.ContinueWith<double>(this.AfterRequest, TaskContinuationOptions.ExecuteSynchronously);
        }

        private double AfterRequest(Task<double> task)
        {
            this.eventSource.RequestCompleted();
            return task.Result;
        }
    }
}
