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
            Alert alert = new Alert("AlertSample", 10.0d, 15.0d);
            string childName = "child";
            ChildProcess childProcess = new ChildProcess(childName);
            try
            {
                logger.WriteInfo("Starting child...");
                childProcess.Start();

                TimeSpan sendInterval = TimeSpan.FromMilliseconds(500.0d);
                SendLoop loop = new SendLoop(logger, childName);
                loop.SendInterval = sendInterval;

                alert.ThresholdReached += delegate(object sender, ThresholdEventArgs e)
                {
                    logger.WriteInfo("Threshold reached; is lower bound? {0}", e.IsLowerBound);
                    if (e.IsLowerBound)
                    {
                        sendInterval = TimeSpan.FromMilliseconds(sendInterval.TotalMilliseconds / 2.0);
                    }
                    else
                    {
                        sendInterval = TimeSpan.FromMilliseconds(sendInterval.TotalMilliseconds * 2.0);
                    }

                    loop.SendInterval = sendInterval;
                    logger.WriteInfo("Send interval updated to {0:0.0} ms.", sendInterval.TotalMilliseconds);
                };

                logger.WriteInfo("Starting alert...");
                alert.Start();

                loop.Start();

                Thread.Sleep(60000);

                loop.Stop();
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

            ReceiveLoop loop = new ReceiveLoop(logger, name);
            loop.Start();

            Console.ReadLine();

            loop.Stop();

            logger.WriteInfo("Child exiting.");
            return 0;
        }
    }
}
