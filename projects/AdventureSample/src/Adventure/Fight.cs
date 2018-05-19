// <copyright file="Fight.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    internal sealed class Fight : Verb
    {
        public Fight(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Blank(ObjectId id)
        {
            this.Print("WHOM DO YOU WANT TO FIGHT?");
            return VerbResult.Idle;
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T FIGHT HIM!");
            return VerbResult.Idle;
        }

        public VerbResult Guard(ObjectId id)
        {
            if (this.State.Map.CurrentRoom != RoomId.SouthOfCastle)
            {
                this.Print("THERE'S NO GUARD HERE!");
                return VerbResult.Idle;
            }

            if (!this.State.Objects.Carrying(ObjectId.Sword))
            {
                this.Print("YOU DON'T HAVE A WEAPON!");
                return VerbResult.Idle;
            }

            this.Print("THE GUARD, NOTICING YOUR SWORD,");
            this.Print("WISELY RETREATS INTO THE CASTLE.");
            this.State.Map.SetMap(RoomId.SouthOfCastle, 0, RoomId.NarrowHall);
            this.State.Objects.Hide(id);
            return VerbResult.Idle;
        }
    }
}
