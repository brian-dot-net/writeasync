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

        public CalculatorClientWithEvents(ICalculatorClientAsync inner)
        {
            this.inner = inner;
        }

        public Task<double> AddAsync(double x, double y)
        {
            return this.inner.AddAsync(x, y);
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            return this.inner.SubtractAsync(x, y);
        }

        public Task<double> SquareRootAsync(double x)
        {
            throw new NotImplementedException();
        }
    }
}
