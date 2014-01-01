//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AlertSample
{
    using System;
    using System.Threading;

    internal sealed class Program
    {
        private static int Main(string[] args)
        {
            Logger logger = new Logger();
            if (args.Length == 0)
            {
                return RunParent(logger);
            }
            else if (args.Length == 1)
            {
                return RunChild(logger, args[0]);
            }
            else
            {
                Console.WriteLine("Invalid arguments.");
                return 1;
            }
        }

        private static int RunParent(Logger logger)
        {
            logger.WriteInfo("Parent started.");
            Alert alert = new Alert("AlertSample", 5.0d, 10.0d);
            ChildProcess childProcess = new ChildProcess("child");
            try
            {
                logger.WriteInfo("Starting child...");
                childProcess.Start();

                alert.ThresholdReached += delegate(object sender, ThresholdEventArgs e)
                {
                    logger.WriteInfo("Threshold reached; is lower bound? {0}", e.IsLowerBound);
                };

                logger.WriteInfo("Starting alert...");
                alert.Start();

                Thread.Sleep(10000);
            }
            catch (Exception e)
            {
                logger.WriteError(e.ToString());
                throw;
            }
            finally
            {
                logger.WriteInfo("Stopping child...");
                childProcess.Stop();

                logger.WriteInfo("Stopping alert...");
                alert.Stop();
            }

            logger.WriteInfo("Parent exiting.");

            return 0;
        }

        private static int RunChild(Logger logger, string name)
        {
            logger.WriteInfo("Child started ('{0}').", name);

            Console.ReadLine();
            
            logger.WriteInfo("Child exiting.");
            return 0;
        }
    }
}
