//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace WatchedManagedApp
{
    using System;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                switch (args[0].ToUpperInvariant())
                {
                    case "RUN":
                        Run(args[1]);
                        break;
                    case "KILL":
                        Kill(args[1]);
                        break;
                    default:
                        Usage();
                        break;
                }
            }
            else
            {
                Usage();
            }
        }

        private static void Run(string signalName)
        {
            using (WatchdogThread thread = new WatchdogThread(signalName))
            {
                Console.WriteLine("Waiting for signal '{0}'. Press ENTER to quit.", signalName);
                Console.ReadLine();
            }
        }

        private static void Kill(string signalName)
        {
            Console.WriteLine("Sending signal '{0}'...", signalName);
            WatchdogThread.Signal(signalName);
        }

        private static void Usage()
        {
            Console.WriteLine("Specify 'run' or 'kill' followed by a signal name.");
        }
    }
}
