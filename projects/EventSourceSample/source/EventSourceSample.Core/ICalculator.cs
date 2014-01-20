//-----------------------------------------------------------------------
// <copyright file="ICalculator.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.ServiceModel;

    [ServiceContract(Name = "ICalculator")]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double x, double y);

        [OperationContract]
        double Subtract(double x, double y);

        [OperationContract]
        double SquareRoot(double x);
    }
}
