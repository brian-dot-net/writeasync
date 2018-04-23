// <copyright file="DimStatements.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class DimStatements
    {
        [Fact]
        public void OneDimensionComplexExpression()
        {
            const string Input = @"10 DIM A(B+2)";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        DIM1_na(out A_na, (B_n) + (2));
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void TwoDimensionsComplexExpression()
        {
            const string Input = @"10 DIM A(B+2, C)";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        DIM2_na(out A_na, (B_n) + (2), C_n);
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void TwoArrays()
        {
            const string Input = @"10 DIM A(1),B$(2)";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        DIM1_na(out A_na, 1);
        DIM1_sa(out B_sa, 2);
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
