// <copyright file="ExampleProgram.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class ExampleProgram
    {
        [Fact]
        public void WithOneCommentLine()
        {
            const string Input = "10 REM A comment";
            const string Expected = @"using System;

internal sealed class MyProg
{
    public void Run()
    {
        while (this.Main())
        {
        }
    }

    private bool Main()
    {
        // A comment
        return false;
    }
}";

            string actual = Translate("MyProg", Input);

            actual.Should().Be(Expected);
        }

        [Fact]
        public void WithOnePrintStatement()
        {
            const string Input = "10 PRINT \"An expression\"";
            const string Expected = @"using System;

internal sealed class MyProg
{
    public void Run()
    {
        while (this.Main())
        {
        }
    }

    private static void PRINT(string expression)
    {
        Console.WriteLine(expression);
    }

    private bool Main()
    {
        PRINT(""An expression"");
        return false;
    }
}";

            string actual = Translate("MyProg", Input);

            actual.Should().Be(Expected);
        }

        private static string Translate(string name, string inputCode)
        {
            WrappedMemoryStream stream = new WrappedMemoryStream(Encoding.UTF8.GetBytes(inputCode));
            string outputCode;
            using (SourceCodeStream source = new SourceCodeStream(stream))
            using (MemoryStream output = new MemoryStream())
            {
                Task task = source.TranslateAsync(name, output);

                task.Exception.Should().BeNull();
                task.IsCompletedSuccessfully.Should().BeTrue();
                outputCode = Encoding.UTF8.GetString(output.ToArray());
            }

            stream.DisposeCount.Should().Be(1);
            return outputCode;
        }

        private sealed class WrappedMemoryStream : MemoryStream
        {
            public WrappedMemoryStream(byte[] buffer)
                : base(buffer)
            {
            }

            public int DisposeCount { get; private set; }

            protected override void Dispose(bool disposing)
            {
                ++this.DisposeCount;
                base.Dispose(disposing);
            }
        }
    }
}
