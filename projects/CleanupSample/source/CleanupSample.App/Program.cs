//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CleanupSample
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                CleanupGuard guard = new CleanupGuard();
                Task task = guard.RunAsync(g => RunSampleAsync(g, cts.Token));

                Console.WriteLine("Press ENTER to stop.");
                Console.ReadLine();

                cts.Cancel();

                try
                {
                    task.Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: " + e);
                }
            }
        }

        private static async Task RunSampleAsync(CleanupGuard guard, CancellationToken token)
        {
            // Make sure we switch to another thread before starting the work.
            await Task.Yield();

            string timestamp = DateTime.Now.ToString("yyyyMMhhmmss");
            string filePrefix = Environment.ExpandEnvironmentVariables(@"%TEMP%\CleanupSample." + timestamp + ".");
            Random random = new Random();
            char[] alphabet = new char[] { 'z', 'x', 'c', 'v', 'b', 'n', 'm' };

            int index = 0;
            while (!token.IsCancellationRequested)
            {
                string fileName = filePrefix + index + ".txt";
                guard.Register(() => ZeroFileAsync(fileName));

                Console.WriteLine("Writing file '{0}'...", fileName);
                await WriteFileAsync(fileName, random, alphabet, token);

                Console.WriteLine("Starting 'notepad.exe {0}'...", fileName);
                Process process = Process.Start("notepad.exe", fileName);
                guard.Register(Blocking.Task(KillProcess, process));

                Console.WriteLine("Waiting...");
                await Task.Delay(3000, token);

                ++index;
            }
        }

        private static async Task ZeroFileAsync(string fileName)
        {
            Console.WriteLine("Zeroing out file '{0}'...", fileName);
            using (FileStream stream = CreateAsyncStream(fileName))
            {
                await stream.WriteAsync(new byte[0], 0, 0);
            }
        }

        private static async Task WriteFileAsync(string fileName, Random random, char[] alphabet, CancellationToken token)
        {
            using (FileStream stream = CreateAsyncStream(fileName))
            {
                int lineCount = random.Next(30) + 1;
                for (int i = 0; i < lineCount; ++i)
                {
                    token.ThrowIfCancellationRequested();
                    string line = CreateRandomLine(random, alphabet);
                    byte[] lineBytes = Encoding.ASCII.GetBytes(line.ToString());
                    await stream.WriteAsync(lineBytes, 0, lineBytes.Length);
                }
            }
        }

        private static string CreateRandomLine(Random random, char[] alphabet)
        {
            StringBuilder line = new StringBuilder();
            int lineLength = random.Next(80) + 1;
            for (int i = 0; i < lineLength; ++i)
            {
                int alphaIndex = random.Next(alphabet.Length);
                char c = alphabet[alphaIndex];
                line.Append(c);
            }

            line.AppendLine();
            return line.ToString();
        }

        private static FileStream CreateAsyncStream(string fileName)
        {
            return new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 1024, true);
        }

        private static void KillProcess(Process process)
        {
            Console.WriteLine("Killing process (args='{0}')...", process.StartInfo.Arguments);
            using (process)
            {
                process.Kill();
                process.WaitForExit();
            }
        }
    }
}
