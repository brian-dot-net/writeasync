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
        public void AssignNumOneD()
        {
            const string Input = @"10 A(1)=2";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_na[1] = (2);
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void AssignStrOneD()
        {
            const string Input = @"10 A$(1)=""x""";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_sa[1] = (""x"");
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void AssignStrTwoD()
        {
            const string Input = @"10 A$(1,B)=C$";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        A_sa[1, B_n] = (C_s);
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
