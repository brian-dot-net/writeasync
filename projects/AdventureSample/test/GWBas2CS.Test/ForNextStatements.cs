// <copyright file="ForNextStatements.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class ForNextStatements
    {
        [Fact]
        public void TwoNested()
        {
            const string Input =
@"10 FOR I=1 TO X
20 FOR J=0 TO Y
30 A=I+J
40 NEXT J
50 NEXT I";
            const string Expected = @"*
    private int Main()
    {
        this.Init();
        I_n = (1);
        while ((I_n) <= (X_n))
        {
            J_n = (0);
            while ((J_n) <= (Y_n))
            {
                A_n = ((I_n) + (J_n));
                J_n = ((J_n) + (1));
            }

            I_n = ((I_n) + (1));
        }

        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
