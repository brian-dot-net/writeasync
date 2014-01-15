//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ProcessSample
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task task = StartAndWaitForProcessAsync(cts.Token);
                
                Console.WriteLine("Press ENTER to quit.");
                Console.ReadLine();

                cts.Cancel();
                try
                {
                    task.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Handle(e => e is OperationCanceledException);
                    Console.WriteLine("(Canceled.)");
                }
            }
        }

        private static async Task StartAndWaitForProcessAsync(CancellationToken token)
        {
            int exitCode;
            DateTime exitTime;

            using (Process process = await Task.Factory.StartNew(() => Process.Start("notepad.exe")))
            using (ProcessExitWatcher watcher = new ProcessExitWatcher(new ProcessExit(process)))
            {
                Console.WriteLine("Waiting for Notepad to exit...");
                await watcher.WaitForExitAsync(token);
                exitCode = watcher.Status.ExitCode;
                exitTime = watcher.Status.ExitTime;
            }

            Console.WriteLine("Done, exited with code {0} at {1}.", exitCode, exitTime);
        }
    }
}
