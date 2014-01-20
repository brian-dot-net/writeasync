//-----------------------------------------------------------------------
// <copyright file="ClientWrapperActivityTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample.Test.Unit
{
    using System;

    public class ClientWrapperActivityTest : ClientWrapperTest
    {
        public ClientWrapperActivityTest()
        {
        }

        protected override ICalculatorClientAsync CreateClient(CalculatorClientStub clientStub)
        {
            return new CalculatorClientWithActivity(clientStub, ClientEventSource.Instance, Guid.Empty);
        }
    }
}
