// <copyright file="Class1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GWExpr
{
    public class Class1
    {
        private readonly string x;

        public Class1(string x)
        {
            this.x = x;
        }

        public override string ToString() => this.x;
    }
}
