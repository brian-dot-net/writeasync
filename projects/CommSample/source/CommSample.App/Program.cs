//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CommSample
{
    using System;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Logger logger = new Logger();
            TimeSpan duration = TimeSpan.FromSeconds(5.0d);
            for (int i = 1; i <= 8; ++i)
            {
                int senderCount = i;
                ReceiveLoopTest test = new ReceiveLoopTest(logger, senderCount, duration, true);
                test.Run();
            }
        }
    }
}
