// <copyright file="Precedence.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr.Test
{
    using Xunit;

    public sealed class Precedence
    {
        [InlineData("1+2*3", "Add(L(1), Mult(L(2), L(3)))")]
        [InlineData("1+2/3", "Add(L(1), Divide(L(2), L(3)))")]
        [InlineData("1-2*3", "Subtract(L(1), Mult(L(2), L(3)))")]
        [InlineData("1-2/3", "Subtract(L(1), Divide(L(2), L(3)))")]
        [InlineData("(1+2)/3", "Divide(Add(L(1), L(2)), L(3))")]
        [InlineData("(1-2)/3", "Divide(Subtract(L(1), L(2)), L(3))")]
        [InlineData("(1+2)*3", "Mult(Add(L(1), L(2)), L(3))")]
        [InlineData("(1-2)*3", "Mult(Subtract(L(1), L(2)), L(3))")]
        [InlineData("(1*2)/3", "Divide(Mult(L(1), L(2)), L(3))")]
        [InlineData("(1*2)+3", "Add(Mult(L(1), L(2)), L(3))")]
        [InlineData("(1/2)-3", "Subtract(Divide(L(1), L(2)), L(3))")]
        [InlineData("1*2^3", "Mult(L(1), Pow(L(2), L(3)))")]
        [InlineData("1^2/3", "Divide(Pow(L(1), L(2)), L(3))")]
        [InlineData("1^2+3", "Add(Pow(L(1), L(2)), L(3))")]
        [InlineData("1^2-3", "Subtract(Pow(L(1), L(2)), L(3))")]
        [InlineData("1+2^3", "Add(L(1), Pow(L(2), L(3)))")]
        [InlineData("1-2^3", "Subtract(L(1), Pow(L(2), L(3)))")]
        [InlineData("(-1)^2+3", "Add(Pow(Neg(L(1)), L(2)), L(3))")]
        [InlineData("1^-2+3", "Add(Pow(L(1), Neg(L(2))), L(3))")]
        [InlineData("1^(-2)+3", "Add(Pow(L(1), Neg(L(2))), L(3))")]
        [InlineData("-1^2+3", "Add(Neg(Pow(L(1), L(2))), L(3))")]
        [Theory]
        public void Arithmetic(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1 AND 2 OR 3", "Or(And(L(1), L(2)), L(3))")]
        [InlineData("1 AND (2 OR 3)", "And(L(1), Or(L(2), L(3)))")]
        [InlineData("1+2 AND 3", "And(Add(L(1), L(2)), L(3))")]
        [InlineData("1*2 AND 3", "And(Mult(L(1), L(2)), L(3))")]
        [InlineData("NOT 1 AND 2 OR 3", "Or(And(Not(L(1)), L(2)), L(3))")]
        [Theory]
        public void Logical(string input, string output)
        {
            Test.Good(input, output);
        }

        [InlineData("1+2 AND 3=4", "And(Add(L(1), L(2)), Eq(L(3), L(4)))")]
        [InlineData("1*2 OR 3=4", "Or(Mult(L(1), L(2)), Eq(L(3), L(4)))")]
        [InlineData("1*2 OR 3<>4", "Or(Mult(L(1), L(2)), Ne(L(3), L(4)))")]
        [InlineData("1>2 AND 3<4", "And(Gt(L(1), L(2)), Lt(L(3), L(4)))")]
        [InlineData("1>2<3=4<>5", "Ne(Eq(Lt(Gt(L(1), L(2)), L(3)), L(4)), L(5))")]
        [InlineData("1>=2<=3=4<>5", "Ne(Eq(Le(Ge(L(1), L(2)), L(3)), L(4)), L(5))")]
        [InlineData("1>2>=3=4<>5", "Ne(Eq(Ge(Gt(L(1), L(2)), L(3)), L(4)), L(5))")]
        [Theory]
        public void Relational(string input, string output)
        {
            Test.Good(input, output);
        }
    }
}
