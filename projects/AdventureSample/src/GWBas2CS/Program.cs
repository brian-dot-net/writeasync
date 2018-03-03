// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please provider an input (.bas) and output (.cs) file path.");
                return 1;
            }

            MainAsync(args[0], args[1]).Wait();
            return 0;
        }

        private static async Task MainAsync(string inputPath, string outputPath)
        {
            using (SourceCodeStream input = new SourceCodeStream(new FileStream(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)))
            using (Stream output = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await input.TranslateAsync(Path.GetFileNameWithoutExtension(inputPath), output);
            }
        }
    }
}
