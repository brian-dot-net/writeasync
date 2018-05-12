// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            string fileBase = null;
            if (args.Length == 1)
            {
                fileBase = args[0];
            }

            List<string> inputLines = new List<string>();
            TextReader input = new WrappedReader(Console.In, inputLines);
            StringBuilder outputText = new StringBuilder();

            Game.Run(input, outputText);

            if (fileBase != null)
            {
                File.WriteAllLines(fileBase + ".in", inputLines);
                File.WriteAllText(fileBase + ".out", outputText.ToString());
            }
        }
    }
}
