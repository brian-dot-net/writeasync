// <copyright file="Wave.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    internal sealed class Wave : Verb
    {
        public Wave(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T WAVE THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Fan(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T HAVE THE FAN!");
                return VerbResult.Idle;
            }

            if (this.State.Map.CurrentRoom != RoomId.Boat)
            {
                this.Print("YOU FEEL A REFRESHING BREEZE!");
                return VerbResult.Idle;
            }

            this.Print("A POWERFUL BREEZE PROPELS THE BOAT");
            this.Print("TO THE OPPOSITE SHORE!");
            if (this.State.Objects.Ref(ObjectId.Boat).Room == RoomId.SouthBankOfRiver)
            {
                this.State.Objects.Drop(ObjectId.Boat, RoomId.NorthBankOfRiver);
                return VerbResult.Idle;
            }

            this.State.Objects.Drop(ObjectId.Boat, RoomId.SouthBankOfRiver);
            return VerbResult.Idle;
        }
    }
}
