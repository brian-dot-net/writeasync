//-----------------------------------------------------------------------
// <copyright file="ICalculatorClientAsync.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System.ServiceModel;
    using System.Threading.Tasks;

    [ServiceContract(Name = "ICalculatorClient")]
    public interface ICalculatorClientAsync
    {
        [OperationContract(AsyncPattern = true)]
        Task<double> AddAsync(double x, double y);

        [OperationContract(AsyncPattern = true)]
        Task<double> SubtractAsync(double x, double y);

        [OperationContract(AsyncPattern = true)]
        Task<double> SquareRootAsync(double x);
    }
}
