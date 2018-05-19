// <copyright file="Row.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Row : Verb
    {
        public Row(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("HOW CAN YOU ROW THAT?");
            return VerbResult.Idle;
        }

        public VerbResult Boat(ObjectId id)
        {
            if (this.State.Map.CurrentRoom != RoomId.Boat)
            {
                this.Print("YOU'RE NOT IN THE BOAT!");
                return VerbResult.Idle;
            }

            this.Print("YOU DON'T HAVE AN OAR!");
            return VerbResult.Idle;
        }
    }
}
