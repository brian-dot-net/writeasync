// <copyright file="LenExpressions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class LenExpressions
    {
        [Fact]
        public void OneStringVar()
        {
            const string Input = @"10 A=LEN(A$)";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = (A_s.Length);
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void OneStringExpr()
        {
            const string Input = @"10 A=LEN(A$+""x"")";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = (((A_s) + (""x"")).Length);
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
