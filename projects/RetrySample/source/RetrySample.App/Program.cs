//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RetrySample
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static readonly Stopwatch LogStopwatch = Stopwatch.StartNew();

        private static void Main(string[] args)
        {
            RetryLoop loop = new RetryLoop(r => ReadFileAsync(r));
            loop.ShouldRetry = r => r.ElapsedTime < TimeSpan.FromMinutes(1.0d);
            loop.Succeeded = r => (r.Exception == null) && (r.Get<string>("Text") == "TEST");
            loop.BeforeRetry = r => BackoffAsync(r);

            loop.ExecuteAsync().Wait();
        }

        private static void Log(string format, params object[] args)
        {
            string message = format;
            if ((args != null) && (args.Length > 0))
            {
                message = string.Format(CultureInfo.CurrentCulture, format, args);
            }

            Console.WriteLine("[{0:000.000}/T={1}] {2}", LogStopwatch.Elapsed.TotalSeconds, Thread.CurrentThread.ManagedThreadId, message);
        }

        private static async Task ReadFileAsync(RetryContext context)
        {
            try
            {
                using (FileStream stream = new FileStream("test.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 256, true))
                {
                    byte[] buffer = new byte[4];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == buffer.Length)
                    {
                        string text = Encoding.ASCII.GetString(buffer);
                        Log("ReadFileAsync read '{0}'", text);
                        context.Add("Text", text);
                    }
                    else
                    {
                        Log("ReadFileAsync read only {0} bytes.", bytesRead);
                    }
                }
            }
            catch (Exception e)
            {
                Log("ReadFileAsync error: {0}: {1}", e.GetType().Name, e.Message);
                throw;
            }
        }

        private static Task BackoffAsync(RetryContext context)
        {
            TimeSpan delay = TimeSpan.FromMilliseconds(10 * context.Iteration);
            Log("Backing off for {0:0.000} seconds.", delay.TotalSeconds);
            return Task.Delay(delay);
        }
    }
}
