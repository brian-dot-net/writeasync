//-----------------------------------------------------------------------
// <copyright file="CalculatorClientWithEvents.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.Threading.Tasks;

    public class CalculatorClientWithEvents : ICalculatorClientAsync
    {
        private readonly ICalculatorClientAsync inner;
        private readonly ClientEventSource eventSource;

        public CalculatorClientWithEvents(ICalculatorClientAsync inner, ClientEventSource eventSource)
        {
            this.inner = inner;
            this.eventSource = eventSource;
        }

        public Task<double> AddAsync(double x, double y)
        {
            this.eventSource.Add(x, y);
            return this.inner.AddAsync(x, y);
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            return this.inner.SubtractAsync(x, y);
        }

        public Task<double> SquareRootAsync(double x)
        {
            return this.inner.SquareRootAsync(x);
        }
    }
}
