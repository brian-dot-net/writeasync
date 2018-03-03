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
            await this.TranslateInnerAsync(program);

            string outputCode = program.ToString();

            byte[] rawOutput = Encoding.UTF8.GetBytes(outputCode.ToString());
            await output.WriteAsync(rawOutput, 0, rawOutput.Length);
        }

        public void Dispose()
        {
            this.reader.Close();
        }

        private async Task TranslateInnerAsync(BasicProgram program)
        {
            while (true)
            {
                string line = await this.reader.ReadLineAsync();
                if (line == null)
                {
                    return;
                }

                string[] numberAndStatement = line.Split(new char[] { ' ' }, 2);
                int lineNumber = int.Parse(numberAndStatement[0]);
                string[] keywordAndRest = numberAndStatement[1].Split(new char[] { ' ' }, 2);

                switch (keywordAndRest[0])
                {
                    case "REM":
                        program.AddComment(lineNumber, keywordAndRest[1]);
                        break;
                    case "PRINT":
                        program.AddPrint(lineNumber, keywordAndRest[1].Substring(1, keywordAndRest[1].Length - 2));
                        break;
                    default:
                        program.AddGoto(lineNumber, int.Parse(keywordAndRest[1]));
                        break;
                }
            }
        }
    }
}
