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
                Console.WriteLine("Please provide an input (.bas) and output (.cs) file path.");
                return 1;
            }

            MainAsync(args[0], args[1]).Wait();
            return 0;
        }

        private static async Task MainAsync(string inputPath, string outputPath)
        {
            using (var input = new SourceCodeStream(Open(inputPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            using (var output = Open(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await input.TranslateAsync(Path.GetFileNameWithoutExtension(inputPath), output);
            }
        }

        private static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStream(path, mode, access, share, 4096, true);
        }
    }
}
