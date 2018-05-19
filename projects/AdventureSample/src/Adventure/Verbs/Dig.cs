// <copyright file="Dig.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Dig : Verb
    {
        public Dig(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T DIG THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Hole(ObjectId id)
        {
            if (!this.State.Objects.IsHere(ObjectId.Shovel, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T HAVE A SHOVEL!");
                return VerbResult.Idle;
            }

            if (this.State.Map.CurrentRoom != RoomId.OpenField)
            {
                this.Print("YOU DON'T FIND ANYTHING.");
                return VerbResult.Idle;
            }

            if (this.State.Objects.Ref(ObjectId.Sword).Room != RoomId.None)
            {
                this.Print("THERE'S NOTHING ELSE THERE!");
                return VerbResult.Idle;
            }

            this.Print("THERE'S SOMETHING THERE!");
            this.State.Objects.Drop(ObjectId.Sword, RoomId.OpenField);
            return VerbResult.Idle;
        }
    }
}
