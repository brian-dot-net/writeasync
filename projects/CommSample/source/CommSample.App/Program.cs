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
            for (int i = 0; i < 8; ++i)
            {
                int senderCount = i + 1;
                ReceiveLoopTest test = new ReceiveLoopTest(logger, senderCount, duration, true, 256);
                test.Run();
            }
        }
    }
}
