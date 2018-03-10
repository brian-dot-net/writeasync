// <copyright file="Calculation.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Four4
{
    using System.Collections.Generic;

    public static class Calculation
    {
        public static string FromString(string input)
        {
            Stack<string> operands = new Stack<string>();
            LinkedList<string> tokens = new LinkedList<string>(input.Split(' '));
            while (tokens.Count > 0)
            {
                string token = tokens.First.Value;
                tokens.RemoveFirst();
                switch (token)
                {
                    case "+":
                        var y = operands.Pop();
                        var x = operands.Pop();
                        tokens.AddFirst((int.Parse(x) + int.Parse(y)).ToString());
                        break;
                    default:
                        operands.Push(token);
                        break;
                }
            }

            return operands.Pop();
        }
    }
}
