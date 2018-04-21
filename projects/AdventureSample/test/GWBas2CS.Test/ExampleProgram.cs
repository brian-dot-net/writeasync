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

    public sealed class ExampleProgram
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
        public void WithOneGotoStatement()
        {
            const string Input = "10 GOTO 10";
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
        L10:
            ;
        goto L10;
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

        [Fact]
        public void WithCommentPrintAndGotoStatement()
        {
            const string Input = @"10 REM My first BASIC program
20 PRINT ""HELLO, WORLD!""
30 GOTO 20";
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
        // My first BASIC program
        L20:
            ;
        PRINT(""HELLO, WORLD!"");
        goto L20;
        return false;
    }
}";

            string actual = Translate("MyProg", Input);

            actual.Should().Be(Expected);
        }

        private static string Translate(string name, string inputCode)
        {
            string outputCode;
            using (MemoryStream output = new MemoryStream())
            {
                WrappedMemoryStream input = new WrappedMemoryStream(Encoding.UTF8.GetBytes(inputCode));

                Task task = SourceCodeStream.TranslateAsync(name, input, output);

                task.IsCompletedSuccessfully.Should().BeTrue();
                outputCode = Encoding.UTF8.GetString(output.ToArray());
            }

            return outputCode;
        }
    }
}
