//-----------------------------------------------------------------------
// <copyright file="CalculatorService.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventSourceSample
{
    using System;
    using System.ServiceModel;
    using System.Threading;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CalculatorService : ICalculator
    {
        private readonly TimeSpan delay;

        public CalculatorService(TimeSpan delay)
        {
            this.delay = delay;
        }

        public double Add(double x, double y)
        {
            Thread.CurrentThread.Join(this.delay);
            return x + y;
        }

        public double Subtract(double x, double y)
        {
            Thread.CurrentThread.Join(this.delay);
            return x - y;
        }

        public double SquareRoot(double x)
        {
            Thread.CurrentThread.Join(this.delay);
            return Math.Sqrt(x);
        }
    }
}
