//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EventHandlerSample
{
    using System;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            using (WorldClock worldClock = new WorldClock())
            {
                DateTime now = worldClock.GetTimeAsync().Result;
                Console.WriteLine(now);
            }
        }
    }
}
