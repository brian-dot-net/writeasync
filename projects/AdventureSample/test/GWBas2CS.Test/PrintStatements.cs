// <copyright file="PrintStatements.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class PrintStatements
    {
        [Fact]
        public void Empty()
        {
            const string Input = @"10 PRINT";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        PRINT("""");
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void OneNumExpr()
        {
            const string Input = @"10 PRINT A";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        PRINT(("""") + (A_n));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void OneStrExpr()
        {
            const string Input = @"10 PRINT A$";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        PRINT(("""") + (A_s));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void TwoMixedExpr()
        {
            const string Input = @"10 PRINT ""NUMBER"";A";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        PRINT((("""") + (""NUMBER"")) + (A_n));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeMixedExpr()
        {
            const string Input = @"10 PRINT A$;"" B "";C";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        PRINT(((("""") + (A_s)) + ("" B "")) + (C_n));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeMixedExprNoBreak()
        {
            const string Input = @"10 PRINT A$;"" B "";C;";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        PRINT_n(((("""") + (A_s)) + ("" B "")) + (C_n));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
