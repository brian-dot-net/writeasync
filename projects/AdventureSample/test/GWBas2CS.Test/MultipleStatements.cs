// <copyright file="MultipleStatements.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class MultipleStatements
    {
        [Fact]
        public void TwoOnOneLine()
        {
            const string Input = @"10 PRINT ""HELLO, WORLD!"" : CLS";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        PRINT(("""") + (""HELLO, WORLD!""));
        CLS();
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeOnOneLine()
        {
            const string Input = @"10 PRINT ""HELLO, WORLD!"" : CLS : CLS";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        PRINT(("""") + (""HELLO, WORLD!""));
        CLS();
        CLS();
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void ThreeOnOneLineCommentEnd()
        {
            const string Input = @"10 PRINT ""HELLO, WORLD!"" : CLS : REM A comment";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        PRINT(("""") + (""HELLO, WORLD!""));
        CLS() // A comment
        ;
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }

        [Fact]
        public void IfThenWithTwo()
        {
            const string Input = @"10 IF A=1 THEN PRINT : CLS";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        if ((((A_n.CompareTo(1)) == (0)) ? (-1) : (0)) != (0))
        {
            PRINT("""");
            CLS();
        }

        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
