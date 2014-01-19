//-----------------------------------------------------------------------
// <copyright file="ClientWrapperActivityTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System.Threading.Tasks;
    using Xunit;

    public class ClientWrapperActivityTest
    {
        public ClientWrapperActivityTest()
        {
        }

        [Fact]
        public void Add_with_events_calls_inner()
        {
            CalculatorClientStub clientStub = new CalculatorClientStub();
            clientStub.Results.Enqueue(1.0d);
            CalculatorClientWithActivity client = new CalculatorClientWithActivity(clientStub);

            VerifyResult(1.0d, client.AddAsync(2.0d, 3.0d));
            clientStub.VerifyOperation("Add", 2.0d, 3.0d);
        }

        private static void VerifyResult(double expectedResult, Task<double> task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Equal(expectedResult, task.Result);            
        }
    }
}
