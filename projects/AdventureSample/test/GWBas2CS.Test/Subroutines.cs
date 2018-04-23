// <copyright file="Subroutines.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class Subroutines
    {
        [Fact]
        public void WithRun()
        {
            const string Input =
@"10 GOSUB 30
20 GOTO 10
30 RUN
40 RETURN";
            const string Expected = @"*
    private int Sub_30()
    {
        return 1;
        return 0;
    }

    private int Main()
    {
        this.Init();
        L10:
            ;
        switch (Sub_30())
        {
            case 1:
                return 1;
            case 2:
                return 2;
        }

        goto L10;
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
