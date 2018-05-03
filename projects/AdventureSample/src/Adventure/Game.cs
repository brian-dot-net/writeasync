// <copyright file="Game.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.IO;
    using System.Text;

    public static class Game
    {
        public static void Run(TextReader input, StringBuilder outputText)
        {
            TextWriter output = new WrappedWriter(Console.Out, outputText);
            new adventure(input, output).Run();
        }
    }
}
