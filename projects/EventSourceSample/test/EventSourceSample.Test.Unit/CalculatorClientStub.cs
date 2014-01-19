//-----------------------------------------------------------------------
// <copyright file="CalculatorClientStub.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal sealed class CalculatorClientStub : ICalculatorClientAsync
    {
        public CalculatorClientStub()
            : this(false)
        {
        }

        public CalculatorClientStub(bool useDefaultResult)
        {
            this.Operations = new List<Tuple<string, double, double>>();
            if (!useDefaultResult)
            {
                this.Results = new Queue<double>();
            }
        }

        public IList<Tuple<string, double, double>> Operations { get; private set; }

        public Queue<double> Results { get; private set; }

        public Task<double> AddAsync(double x, double y)
        {
            return this.Complete("Add", x, y);
        }

        public Task<double> SubtractAsync(double x, double y)
        {
            return this.Complete("Subtract", x, y);
        }

        public Task<double> SquareRootAsync(double x)
        {
            return this.Complete("SquareRoot", x, 0.0d);
        }

        private Task<double> Complete(string name, double x, double y)
        {
            this.Operations.Add(Tuple.Create(name, x, y));
            return Task.FromResult(this.NextResult());
        }

        private double NextResult()
        {
            double result = 0.0d;
            if (this.Results != null)
            {
                result = this.Results.Dequeue();
            }

            return result;
        }
    }
}
