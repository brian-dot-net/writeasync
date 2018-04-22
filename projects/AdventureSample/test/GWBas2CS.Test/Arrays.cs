// <copyright file="Arrays.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class Arrays
    {
        [Fact]
        public void AssignNumOneDimenson()
        {
            const string Input = @"10 A(1)=2";
            const string Expected = @"*
    private bool Main()
    {
        this.Init();
        A_na[1] = (2);
        return false;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
