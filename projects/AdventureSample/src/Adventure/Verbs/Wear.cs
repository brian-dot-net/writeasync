// <copyright file="Wear.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Wear : Verb
    {
        public Wear(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T WEAR THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Gloves(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T HAVE THE GLOVES.");
                return VerbResult.Idle;
            }

            this.Print("YOU ARE NOW WEARING THE GLOVES.");
            this.State.WearingGloves = true;
            return VerbResult.Idle;
        }
    }
}
