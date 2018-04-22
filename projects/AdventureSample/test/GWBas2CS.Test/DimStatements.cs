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
    private bool Main()
    {
        this.Init();
        DIM1_na(out A_na, (B_n) + (2));
        return false;
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
    private bool Main()
    {
        this.Init();
        DIM2_na(out A_na, (B_n) + (2), C_n);
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
