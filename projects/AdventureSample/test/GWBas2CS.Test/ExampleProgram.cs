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
130 DIM B2(9,10)
140 CLS
150 PRINT A$;
160 GOSUB 2000
170 GOSUB 2020
180 GOSUB 2030
190 INPUT A
200 IF A=1 THEN 190
210 READ A
220 READ A$
230 FOR I=1 TO 10
240 PRINT I
250 NEXT I
260 A$=MID$(A$,A,1)
270 A$=LEFT$(A$,A)
1000 GOTO 20
2000 CLS
2001 DATA 1
2010 RETURN
2020 RETURN
2030 GOSUB 2000
2040 RETURN
3000 DATA x";
            const string Expected = @"using System;
using System.Collections;
using System.IO;

internal sealed class MyProg
{
    private readonly TextReader input;
    private readonly TextWriter output;
    private Queue DATA;
    private string[] A_sa;
    private string[] B1_sa;
    private float[] A_na;
    private float[] B1_na;
    private float[, ] B2_na;
    private string A_s;
    private string B1_s;
    private float A_n;
    private float B1_n;
    private float I_n;
    public MyProg(TextReader input, TextWriter output)
    {
        this.input = (input);
        this.output = (output);
    }

    public void Run()
    {
        while ((this.Main()) == (1))
        {
        }
    }

    private void Init()
    {
        DATA = (new Queue());
        DATA.Enqueue(1);
        DATA.Enqueue(""x"");
        A_s = ("""");
        B1_s = ("""");
        A_n = (0);
        B1_n = (0);
        I_n = (0);
    }

    private void PRINT(string expression)
    {
        this.output.WriteLine(expression);
    }

    private void DIM1_sa(out string[] a, float d1)
    {
        a = (new string[((int)(d1)) + (1)]);
        Array.Fill(a, """");
    }

    private void DIM1_na(out float[] a, float d1)
    {
        a = (new float[((int)(d1)) + (1)]);
    }

    private void DIM2_na(out float[, ] a, float d1, float d2)
    {
        a = (new float[((int)(d1)) + (1), ((int)(d2)) + (1)]);
    }

    private void CLS()
    {
        this.output.Write('\f');
        Console.Clear();
    }

    private void PRINT_n(string expression)
    {
        this.output.Write(expression);
    }

    private float INPUT_n(string prompt)
    {
        while (true)
        {
            this.output.Write((prompt) + (""? ""));
            string v = this.input.ReadLine();
            float r;
            if (float.TryParse(v, out r))
            {
                return r;
            }

            this.output.WriteLine(""?Redo from start"");
        }
    }

    private float READ_n()
    {
        return (float)(DATA.Dequeue());
    }

    private string READ_s()
    {
        return (string)(DATA.Dequeue());
    }

    private string MID_s(string x, int n, int m)
    {
        if ((n) > (x.Length))
        {
            return """";
        }

        int l = ((x.Length) - (n)) + (1);
        if ((m) > (l))
        {
            m = (l);
        }

        return x.Substring((n) - (1), m);
    }

    private string LEFT_s(string x, int n)
    {
        if ((n) > (x.Length))
        {
            return x;
        }

        return x.Substring(0, n);
    }

    private int Sub_2000()
    {
        CLS();
        return 0;
    }

    private int Sub_2020()
    {
        return 0;
    }

    private int Sub_2030()
    {
        switch (Sub_2000())
        {
            case 1:
                return 1;
            case 2:
                return 2;
        }

        return 0;
    }

    private int Main()
    {
        this.Init();
        ; // My first BASIC program
        L20:
            ;
        PRINT(("""") + (""HELLO, WORLD!""));
        A_s = (""a string"");
        A_s = (""same string"");
        B1_s = (""new string"");
        A_n = (2);
        A_n = (20);
        B1_n = (3);
        DIM1_sa(out A_sa, 5);
        DIM1_sa(out B1_sa, 6);
        DIM1_na(out A_na, 7);
        DIM1_na(out B1_na, 8);
        DIM2_na(out B2_na, 9, 10);
        CLS();
        PRINT_n(("""") + (A_s));
        switch (Sub_2000())
        {
            case 1:
                return 1;
            case 2:
                return 2;
        }

        switch (Sub_2020())
        {
            case 1:
                return 1;
            case 2:
                return 2;
        }

        switch (Sub_2030())
        {
            case 1:
                return 1;
            case 2:
                return 2;
        }

        L190:
            ;
        A_n = (INPUT_n(""""));
        if ((((A_n.CompareTo(1)) == (0)) ? (-1) : (0)) != (0))
        {
            goto L190;
        }

        A_n = (READ_n());
        A_s = (READ_s());
        I_n = (1);
        while ((I_n) <= (10))
        {
            PRINT(("""") + (I_n));
            I_n = ((I_n) + (1));
        }

        A_s = (MID_s(A_s, (int)(A_n), (int)(1)));
        A_s = (LEFT_s(A_s, (int)(A_n)));
        goto L20;
        return 2;
    }
}";

            string actual = Test.Translate("MyProg", Input);

            actual.Should().Be(Expected);
        }
    }
}
