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
    using Xunit;

    public sealed class CalculatorClientStub : ICalculatorClientAsync
    {
        private readonly Func<Task<double>> getResult;

        public CalculatorClientStub()
            : this(null)
        {
        }

        public CalculatorClientStub(Func<Task<double>> getResult)
        {
            this.Operations = new List<Tuple<string, double, double>>();
            if (getResult != null)
            {
                this.getResult = getResult;
            }
            else
            {
                this.Results = new Queue<double>();
                this.getResult = this.NextResult;
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

        public void VerifyOperation(string expectedName, double expectedX, double expectedY)
        {
            Assert.Equal(1, this.Operations.Count);
            Assert.Equal(expectedName, this.Operations[0].Item1);
            Assert.Equal(expectedX, this.Operations[0].Item2);
            Assert.Equal(expectedY, this.Operations[0].Item3);
        }

        private Task<double> Complete(string name, double x, double y)
        {
            this.Operations.Add(Tuple.Create(name, x, y));
            return this.getResult();
        }

        private Task<double> NextResult()
        {
            double result = 0.0d;
            if (this.Results != null)
            {
                result = this.Results.Dequeue();
            }

            return Task.FromResult(result);
        }
    }
}
