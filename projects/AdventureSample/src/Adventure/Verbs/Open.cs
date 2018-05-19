// <copyright file="Open.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Open : Verb
    {
        public Open(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T OPEN THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Case(ObjectId id)
        {
            if (this.State.Map.CurrentRoom != RoomId.LargeHall)
            {
                this.Print("THERE'S NO CASE HERE!");
                return VerbResult.Idle;
            }

            if (!this.State.WearingGloves)
            {
                this.Print("THE CASE IS ELECTRIFIED!");
                return VerbResult.Idle;
            }

            this.Print("THE GLOVES INSULATE AGAINST THE");
            this.Print("ELECTRICITY! THE CASE OPENS!");
            this.State.Objects.Drop(ObjectId.Ruby, RoomId.LargeHall);
            return VerbResult.Idle;
        }

        public VerbResult Cabinet(ObjectId id)
        {
            if (this.State.Map.CurrentRoom != RoomId.Kitchen)
            {
                this.Print("THERE'S NO CABINET HERE!");
                return VerbResult.Idle;
            }

            this.Print("THERE'S SOMETHING INSIDE!");
            this.State.Objects.Drop(ObjectId.Salt, RoomId.Kitchen);
            return VerbResult.Idle;
        }

        public VerbResult Box(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("THERE'S NO BOX HERE!");
                return VerbResult.Idle;
            }

            this.State.Objects.Drop(ObjectId.Bottle, this.State.Map.CurrentRoom);
            this.Print("SOMETHING FELL OUT!");
            return VerbResult.Idle;
        }
    }
}
