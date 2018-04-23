// <copyright file="SubtractExpressions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class SubtractExpressions
    {
        [Fact]
        public void TwoNumericOperands()
        {
            const string Input = @"10 A=A-1";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = ((A_n) - (1));
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeNumericOperands()
        {
            const string Input = @"10 A=A-B-2";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = (((A_n) - (B_n)) - (2));
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
