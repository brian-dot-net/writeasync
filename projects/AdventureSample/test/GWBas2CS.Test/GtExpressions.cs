// <copyright file="GtExpressions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class GtExpressions
    {
        [Fact]
        public void TwoNumericOperands()
        {
            const string Input = @"10 A=B>2";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = (((B_n.CompareTo(2)) > (0)) ? (-1) : (0));
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void TwoStringOperands()
        {
            const string Input = @"10 A=B$>""x""";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = (((B_s.CompareTo(""x"")) > (0)) ? (-1) : (0));
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeNumericOperands()
        {
            const string Input = @"10 A=B>C>1";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_n = ((((((B_n.CompareTo(C_n)) > (0)) ? (-1) : (0)).CompareTo(1)) > (0)) ? (-1) : (0));
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
