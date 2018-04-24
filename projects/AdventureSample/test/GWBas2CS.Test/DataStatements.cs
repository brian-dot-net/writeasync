// <copyright file="DataStatements.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class DataStatements
    {
        [Fact]
        public void TwoValuesMixed()
        {
            const string Input =
@"10 DATA 234,abc
20 READ X,Y$";
            const string Expected = @"*
    private void Init()
    {
        DATA = (new Queue());
        DATA.Enqueue(234);
        DATA.Enqueue(""abc"");
        Y_s = ("""");
        X_n = (0);
    }
*
    private int Main()
    {
        this.Init();
        X_n = (READ_n());
        Y_s = (READ_s());
        return 2;
    }
*";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Match(Expected);
        }
    }
}
