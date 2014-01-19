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

        public CalculatorClientWithActivity(ICalculatorClientAsync inner)
        {
            this.inner = inner;
        }

        public Task<double> AddAsync(double x, double y)
        {
            return this.inner.AddAsync(x, y);
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            throw new NotImplementedException();
        }

        public Task<double> SquareRootAsync(double x)
        {
            throw new NotImplementedException();
        }
    }
}
