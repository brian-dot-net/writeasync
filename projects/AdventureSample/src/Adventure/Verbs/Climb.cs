// <copyright file="Climb.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Climb : Verb
    {
        public Climb(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("IT WON'T DO ANY GOOD.");
            return VerbResult.Idle;
        }

        public VerbResult Ladder(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T HAVE THE LADDER!");
                return VerbResult.Idle;
            }

            if (this.State.Map.CurrentRoom != RoomId.EdgeOfForest)
            {
                this.Print("WHATEVER FOR?");
                return VerbResult.Idle;
            }

            this.Print("THE LADDER SINKS UNDER YOUR WEIGHT!");
            this.Print("IT DISAPPEARS INTO THE GROUND!");
            this.State.Objects.Hide(id);
            return VerbResult.Idle;
        }

        public VerbResult Tree(ObjectId id)
        {
            if (this.State.Map.CurrentRoom != RoomId.EdgeOfForest)
            {
                this.Print("THERE'S NO TREE HERE!");
                return VerbResult.Idle;
            }

            this.Print("YOU CAN'T REACH THE BRANCHES!");
            return VerbResult.Idle;
        }
    }
}
