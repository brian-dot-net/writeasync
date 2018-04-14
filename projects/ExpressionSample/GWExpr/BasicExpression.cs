// <copyright file="BasicExpression.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    using System;
    using Sprache;

    public abstract class BasicExpression
    {
        protected BasicExpression()
        {
        }

        public static BasicExpression FromString(string input)
        {
            try
            {
                return Expr.FromString(input);
            }
            catch (ParseException e)
            {
                throw new FormatException("Bad expression '" + input + "'.", e);
            }
        }
    }
}
