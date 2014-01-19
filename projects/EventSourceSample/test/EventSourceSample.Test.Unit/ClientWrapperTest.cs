//-----------------------------------------------------------------------
// <copyright file="ClientWrapperTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Threading.Tasks;
    using Xunit;

    public class ClientWrapperTest
    {
        public ClientWrapperTest()
        {
        }

        [Fact]
        public void Add_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(1.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub, ClientEventSource.Instance);

            VerifyResult(1.0d, client.AddAsync(2.0d, 3.0d));
            VerifyOperation("Add", 2.0d, 3.0d, clientStub);
        }

        [Fact]
        public void Subtract_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(4.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub, ClientEventSource.Instance);

            VerifyResult(4.0d, client.SubtractAsync(5.0d, 6.0d));
            VerifyOperation("Subtract", 5.0d, 6.0d, clientStub);
        }

        [Fact]
        public void SquareRoot_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(8.0d);
            CalculatorClientWithEvents client = new CalculatorClientWithEvents(clientStub, ClientEventSource.Instance);

            VerifyResult(8.0d, client.SquareRootAsync(7.0d));
            VerifyOperation("SquareRoot", 7.0d, 0.0d, clientStub);
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
    }
}
