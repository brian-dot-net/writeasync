// <copyright file="Postfix.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System.Collections.Generic;

    public static class Postfix
    {
        public static string ToInfix(string input)
        {
            Stack<string> operands = new Stack<string>();
            foreach (string token in input.Split(' '))
            {
                switch (token)
                {
                    case "!":
                        operands.Push(One(operands) + "!");
                        break;
                    case "R":
                        operands.Push("sqrt" + One(operands));
                        break;
                    default:
                        operands.Push(token);
                        break;
                }
            }

            return operands.Pop();
        }

        private static string One(Stack<string> operands)
        {
            return "(" + operands.Pop() + ")";
        }
    }
}
