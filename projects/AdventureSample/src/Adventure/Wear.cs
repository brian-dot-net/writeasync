// <copyright file="Wear.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    internal sealed class Wear
    {
        private readonly GameState state;
        private readonly Action<string> print;

        public Wear(GameState state, Action<string> print)
        {
            this.state = state;
            this.print = print;
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.print("YOU CAN'T WEAR THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Gloves(ObjectId id)
        {
            if (!this.state.Objects.IsHere(id, this.state.Map.CurrentRoom))
            {
                this.print("YOU DON'T HAVE THE GLOVES.");
                return VerbResult.Idle;
            }

            this.print("YOU ARE NOW WEARING THE GLOVES.");
            this.state.WearingGloves = true;
            return VerbResult.Idle;
        }
    }
}
