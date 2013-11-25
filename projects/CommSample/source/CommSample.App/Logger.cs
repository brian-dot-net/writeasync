//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    internal sealed class Logger
    {
        private readonly Stopwatch stopwatch;

        public Logger()
        {
            this.stopwatch = Stopwatch.StartNew();
        }

        public void WriteLine(string format, params object[] args)
        {
            string message = format;
            if ((args != null) && (args.Length > 0))
            {
                message = string.Format(CultureInfo.InvariantCulture, format, args);
            }

            Console.WriteLine("[{0:0000.000}/T{1:00}] {2}", this.stopwatch.Elapsed.TotalSeconds, Thread.CurrentThread.ManagedThreadId, message);
        }
    }
}
