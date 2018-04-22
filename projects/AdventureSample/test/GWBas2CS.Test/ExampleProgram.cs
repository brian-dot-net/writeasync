// <copyright file="ExampleProgram.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWBas2CS.Test
{
    using FluentAssertions;
    using Xunit;

    public sealed class ExampleProgram
    {
        [Fact]
        public void FullClass()
        {
            const string Input = @"10 REM My first BASIC program
20 PRINT ""HELLO, WORLD!""
30 A$=""a string""
40 A$=""same string""
50 B1$=""new string""
60 A=2
70 A=20
80 B1=3
90 DIM A$(5)
100 DIM B1$(6)
110 DIM A(7)
120 DIM B1(8)
200 GOTO 20";
            const string Expected = @"using System;
using System.IO;

internal sealed class MyProg
{
    private readonly TextReader input;
    private readonly TextWriter output;
    private string[] A_sa;
    private string[] B1_sa;
    private float[] A_na;
    private float[] B1_na;
    private string A_s;
    private string B1_s;
    private float A_n;
    private float B1_n;
    public MyProg(TextReader input, TextWriter output)
    {
        this.input = (input);
        this.output = (output);
    }

    public void Run()
    {
        while (this.Main())
        {
        }
    }

    private void Init()
    {
        A_s = ("""");
        B1_s = ("""");
        A_n = (0);
        B1_n = (0);
    }

    private void PRINT(string expression)
    {
        this.output.WriteLine(expression);
    }

    private void DIM_sa(out string[] a, float d1)
    {
        a = (new string[((int)(d1)) + (1)]);
    }

    private void DIM_na(out float[] a, float d1)
    {
        a = (new float[((int)(d1)) + (1)]);
    }

    private bool Main()
    {
        this.Init();
        // My first BASIC program
        L20:
            ;
        PRINT(""HELLO, WORLD!"");
        A_s = (""a string"");
        A_s = (""same string"");
        B1_s = (""new string"");
        A_n = (2);
        A_n = (20);
        B1_n = (3);
        DIM_sa(out A_sa, 5);
        DIM_sa(out B1_sa, 6);
        DIM_na(out A_na, 7);
        DIM_na(out B1_na, 8);
        goto L20;
        return false;
    }
}";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Be(Expected);
        }
    }
}
