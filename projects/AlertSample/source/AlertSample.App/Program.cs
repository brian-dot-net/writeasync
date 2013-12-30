//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    internal sealed class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                return RunParent();
            }
            else if (args.Length == 1)
            {
                return RunChild(args[0]);
            }
            else
            {
                Console.WriteLine("Invalid arguments.");
                return 1;
            }
        }

        private static int RunParent()
        {
            Alert alert = new Alert("AlertSample", 5.0d, 10.0d);
            ChildProcess childProcess = new ChildProcess("child");
            try
            {
                childProcess.Start();
                alert.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e);
                throw;
            }
            finally
            {
                childProcess.Stop();
                alert.Stop();
            }

            return 0;
        }

        private static int RunChild(string name)
        {
            Console.WriteLine("Child started.");
            Console.ReadLine();
            return 0;
        }
    }
}
