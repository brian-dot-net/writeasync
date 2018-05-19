// <copyright file="Verb.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    internal abstract class Verb
    {
        protected Verb(GameState state, Action<string> print)
        {
            this.State = state;
            this.Print = print;
        }

        protected GameState State { get; private set; }

        protected Action<string> Print { get; private set; }
    }
}
