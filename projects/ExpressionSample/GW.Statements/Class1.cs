// <copyright file="Class1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace GW.Statements
{
    public class Class1
    {
        private readonly string v;

        public Class1(string v)
        {
            this.v = v;
        }

        public override string ToString() => this.v;
    }
}
