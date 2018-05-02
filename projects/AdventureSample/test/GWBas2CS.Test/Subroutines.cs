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

        [Fact]
        public void WithEnd()
        {
            const string Input =
@"10 GOSUB 30
20 GOTO 10
30 END
40 RETURN";
            const string Expected = @"*
    private int Sub_30()
    {
        return 2;
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

        [Fact]
        public void WithMultipleReturns()
        {
            const string Input =
@"10 GOSUB 30
20 GOTO 70
30 IF A=2 THEN RETURN
40 A=1
50 CLS
60 RETURN
70 PRINT";
            const string Expected = @"*
    private int Sub_30()
    {
        if ((((A_n.CompareTo(2)) == (0)) ? (-1) : (0)) != (0))
        {
            return 0;
        }

        A_n = (1);
        CLS();
        return 0;
    }

    private int Main()
    {
        this.Init();
        switch (Sub_30())
        {
            case 1:
                return 1;
            case 2:
                return 2;
        }

        goto L70;
        L70:
            ;
        PRINT("""");
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
