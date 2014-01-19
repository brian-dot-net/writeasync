//-----------------------------------------------------------------------
// <copyright file="ICalculatorClient.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.ServiceModel;

    [ServiceContract(Name = "ICalculatorClient")]
    public interface ICalculatorClient
    {
        [OperationContract]
        double Add(double x, double y);

        [OperationContract]
        double Subtract(double x, double y);

        [OperationContract]
        double SquareRootAsync(double x);
    }
}
