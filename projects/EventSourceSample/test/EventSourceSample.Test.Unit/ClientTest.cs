//-----------------------------------------------------------------------
// <copyright file="ClientTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ClientTest
    {
        public ClientTest()
        {
        }

        [Fact]
        public void Add_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(1.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub);

            VerifyResult(1.0d, client.AddAsync(2.0d, 3.0d));
            VerifyOperation("Add", 2.0d, 3.0d, clientStub);
        }

        [Fact]
        public void Subtract_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(4.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub);

            VerifyResult(4.0d, client.SubtractAsync(5.0d, 6.0d));
            VerifyOperation("Subtract", 5.0d, 6.0d, clientStub);
        }

        private static void VerifyResult(double expectedResult, Task<double> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(expectedResult, task.Result);            
        }

        private static void VerifyOperation(string expectedName, double expectedX, double expectedY, CalculatorClientStub clientStub)
        {
            Assert.Equal(1, clientStub.Operations.Count);
            Assert.Equal(expectedName, clientStub.Operations[0].Item1);
            Assert.Equal(expectedX, clientStub.Operations[0].Item2);
            Assert.Equal(expectedY, clientStub.Operations[0].Item3);
        }

        private sealed class CalculatorClientStub : ICalculatorClientAsync
        {
            public CalculatorClientStub()
            {
                this.Operations = new List<Tuple<string, double, double>>();
                this.Results = new Queue<double>();
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
                return Task.FromResult(this.Results.Dequeue());
            }
        }
    }
}
