// <copyright file="Precedence.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Precedence
    {
        [InlineData("1+2*3", "Add(NumL(1), Mult(NumL(2), NumL(3)))")]
        [InlineData("1+2/3", "Add(NumL(1), Div(NumL(2), NumL(3)))")]
        [InlineData("1-2*3", "Sub(NumL(1), Mult(NumL(2), NumL(3)))")]
        [InlineData("1-2/3", "Sub(NumL(1), Div(NumL(2), NumL(3)))")]
        [InlineData("(1+2)/3", "Div(Add(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1-2)/3", "Div(Sub(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1+2)*3", "Mult(Add(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1-2)*3", "Mult(Sub(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1*2)/3", "Div(Mult(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1*2)+3", "Add(Mult(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("(1/2)-3", "Sub(Div(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1*2^3", "Mult(NumL(1), Pow(NumL(2), NumL(3)))")]
        [InlineData("1^2/3", "Div(Pow(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1^2+3", "Add(Pow(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1^2-3", "Sub(Pow(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1+2^3", "Add(NumL(1), Pow(NumL(2), NumL(3)))")]
        [InlineData("1-2^3", "Sub(NumL(1), Pow(NumL(2), NumL(3)))")]
        [InlineData("(-1)^2+3", "Add(Pow(Neg(NumL(1)), NumL(2)), NumL(3))")]
        [InlineData("1^-2+3", "Add(Pow(NumL(1), Neg(NumL(2))), NumL(3))")]
        [InlineData("1^(-2)+3", "Add(Pow(NumL(1), Neg(NumL(2))), NumL(3))")]
        [InlineData("-1^2+3", "Add(Neg(Pow(NumL(1), NumL(2))), NumL(3))")]
        [Theory]
        public void Arithmetic(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 AND 2 OR 3", "Or(And(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1 AND (2 OR 3)", "And(NumL(1), Or(NumL(2), NumL(3)))")]
        [InlineData("1+2 AND 3", "And(Add(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("1*2 AND 3", "And(Mult(NumL(1), NumL(2)), NumL(3))")]
        [InlineData("NOT 1 AND 2 OR 3", "Or(And(Not(NumL(1)), NumL(2)), NumL(3))")]
        [Theory]
        public void LogicaNumL(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+2 AND 3=4", "And(Add(NumL(1), NumL(2)), Eq(NumL(3), NumL(4)))")]
        [InlineData("1*2 OR 3=4", "Or(Mult(NumL(1), NumL(2)), Eq(NumL(3), NumL(4)))")]
        [InlineData("1*2 OR 3<>4", "Or(Mult(NumL(1), NumL(2)), Ne(NumL(3), NumL(4)))")]
        [InlineData("1>2 AND 3<4", "And(Gt(NumL(1), NumL(2)), Lt(NumL(3), NumL(4)))")]
        [InlineData("1>2<3=4<>5", "Ne(Eq(Lt(Gt(NumL(1), NumL(2)), NumL(3)), NumL(4)), NumL(5))")]
        [InlineData("1>=2<=3=4<>5", "Ne(Eq(Le(Ge(NumL(1), NumL(2)), NumL(3)), NumL(4)), NumL(5))")]
        [InlineData("1>2>=3=4<>5", "Ne(Eq(Ge(Gt(NumL(1), NumL(2)), NumL(3)), NumL(4)), NumL(5))")]
        [InlineData("\"1\">\"2\" AND \"3\"<>\"4\"", "And(Gt(StrL(1), StrL(2)), Ne(StrL(3), StrL(4)))")]
        [Theory]
        public void RelationaNumL(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
