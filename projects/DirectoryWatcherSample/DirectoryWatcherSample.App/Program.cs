﻿// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DirectoryWatcherSample
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    internal sealed class Program
    {
        private static readonly Stopwatch Watch = Stopwatch.StartNew();

        private static void Main()
        {
            Directory.CreateDirectory("inner1");
            Directory.CreateDirectory("inner2");

            using ILoggerFactory factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            using CancellationTokenSource cts = new CancellationTokenSource();
            string[] files = new string[] { @"inner1\file1.txt", @"inner2\file2.txt" };

            Task task = UpdateFilesAsync(files, cts.Token);

            RunWatcher(factory.CreateLogger<Program>(), files);

            Console.WriteLine("Press ENTER to quit.");
            Console.ReadLine();

            cts.Cancel();
            task.Wait();
        }

        private static void RunWatcher(ILogger log, string[] files)
        {
            DirectoryInfo root = new DirectoryInfo(".");
            using IDirectoryWatcher watcher = new DirectoryWatcherWithLogging(
                new DirectoryWatcherWithBatching(
                    new DirectoryTreeWatcher(root),
                    root.FullName,
                    new BatchedEvents<FileInfo>(async () => await Task.Delay(10))),
                root.FullName,
                log);

            Action<FileInfo> onUpdated = f => Log($"Got an update for '{f.Name}'");

            IDisposable lastFile = null;
            foreach (string file in files)
            {
                Console.WriteLine($"Subscribing to '{file}'");
                lastFile = watcher.Subscribe(file, onUpdated);
            }

            Console.WriteLine("Press ENTER unsubscribe last file.");
            Console.ReadLine();

            using (lastFile)
            {
            }

            Console.WriteLine("Press ENTER to dispose.");
            Console.ReadLine();
        }

        private static async Task UpdateFilesAsync(string[] files, CancellationToken token)
        {
            Random random = new Random();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    int millis = random.Next(1000);
                    await Task.Delay(millis, token);
                    UpdateFile(files, random);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private static void UpdateFile(string[] files, Random random)
        {
            int fileNum = random.Next(files.Length);
            string file = files[fileNum];
            double d = random.NextDouble();
            string contents = Stopwatch.GetTimestamp().ToString();
            if (d < 0.25)
            {
                Log($"** overwrite '{file}'");
                File.WriteAllText(file, contents);
            }
            else if (d < 0.50)
            {
                Log($"** move to '{file}'");
                File.WriteAllText(file + ".tmp", contents);
                File.Delete(file);
                File.Move(file + ".tmp", file);
            }
            else if (d < 0.75)
            {
                Log($"** append '{file}'");
                File.AppendAllText(file, contents);
            }
            else
            {
                Log($"** delete '{file}'");
                File.Delete(file);
            }
        }

        private static void Log(string message)
        {
            Console.WriteLine(
                "[{0:000.000}/T{1}] {2}",
                Watch.Elapsed.TotalSeconds,
                Thread.CurrentThread.ManagedThreadId,
                message);
        }
    }
}
