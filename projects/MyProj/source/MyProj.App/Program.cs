//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MyProj
{
    using System;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Class1 c = new Class1("world");
            string name = c.DoAsync().Result;
            Console.WriteLine("Hello, {0}!", name);
        }
    }
}
