//-----------------------------------------------------------------------
// <copyright file="CalculatorService.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.ServiceModel;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CalculatorService : ICalculator
    {
        public CalculatorService()
        {
        }

        public double Add(double x, double y)
        {
            return x + y;
        }

        public double Subtract(double x, double y)
        {
            return x - y;
        }

        public double SquareRoot(double x)
        {
            return Math.Sqrt(x);
        }
    }
}
