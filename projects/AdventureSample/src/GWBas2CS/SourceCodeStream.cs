﻿// <copyright file="SourceCodeStream.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using GWParse.Lines;

    public static class SourceCodeStream
    {
        public static async Task TranslateAsync(string name, Stream input, Stream output)
        {
            BasicProgram program = new BasicProgram(name);
            foreach (BasicLine line in await BasicStream.ReadAsync(input))
            {
                line.Accept(program);
            }

            string outputCode = program.ToString();

            byte[] rawOutput = Encoding.UTF8.GetBytes(outputCode.ToString());
            await output.WriteAsync(rawOutput, 0, rawOutput.Length);
        }
    }
}
