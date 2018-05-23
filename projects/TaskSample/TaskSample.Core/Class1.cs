// <copyright file="Class1.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace TaskSample
{
    public class Class1
    {
        private readonly string name;

        public Class1(string name)
        {
            this.name = name;
        }

        public override string ToString() => this.name;
    }
}
