// <copyright file="IfThenStatements.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class IfThenStatements
    {
        [Fact]
        public void ManyStatementsWithTrailingIfThen()
        {
            const string Input = @"100 PRINT : CM$="""" : INPUT ""WHAT NOW"";CM$ : IF CM$="""" THEN 100";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        L100:
            ;
        PRINT("""");
        CM_s = ("""");
        CM_s = (INPUT_s(""WHAT NOW""));
        if ((((CM_s.CompareTo("""")) == (0)) ? (-1) : (0)) != (0))
        {
            goto L100;
        }

        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
