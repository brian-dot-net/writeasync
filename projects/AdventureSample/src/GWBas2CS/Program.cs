// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System.IO;
    using System.Threading.Tasks;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            using (SourceCodeStream input = new SourceCodeStream(new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)))
            using (Stream output = new FileStream(args[1], FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await input.TranslateAsync(Path.GetFileNameWithoutExtension(args[0]), output);
            }
        }
    }
}
