// <copyright file="SourceCodeStream.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class SourceCodeStream : IDisposable
    {
        private readonly StreamReader reader;

        public SourceCodeStream(Stream input)
        {
            this.reader = new StreamReader(input);
        }

        public async Task TranslateAsync(string name, Stream output)
        {
            BasicProgram program = new BasicProgram(name);

            string line = await this.reader.ReadLineAsync();
            string[] numberAndStatement = line.Split(new char[] { ' ' }, 2);
            string[] keywordAndRest = numberAndStatement[1].Split(new char[] { ' ' }, 2);
            string comment = keywordAndRest[1];

            program.AddComment(comment);

            string outputCode = program.ToString();

            byte[] rawOutput = Encoding.UTF8.GetBytes(outputCode.ToString());
            await output.WriteAsync(rawOutput, 0, rawOutput.Length);
        }

        public void Dispose()
        {
            this.reader.Close();
        }
    }
}
