// <copyright file="AndExpressions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class AndExpressions
    {
        [Fact]
        public void TwoNumericOperands()
        {
            const string Input = @"10 A=A AND 1";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        A_n = (((int)(A_n)) & ((int)(1)));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeNumericOperands()
        {
            const string Input = @"10 A=A AND B AND 2";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        A_n = (((int)(((int)(A_n)) & ((int)(B_n)))) & ((int)(2)));
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
