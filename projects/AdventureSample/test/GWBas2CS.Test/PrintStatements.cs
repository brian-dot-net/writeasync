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
    }
}
