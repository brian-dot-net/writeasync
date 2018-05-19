// <copyright file="Look.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Look : Verb
    {
        public Look(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Blank(ObjectId id) => VerbResult.Proceed;

        public VerbResult Any(ObjectId id)
        {
            if (id == ObjectId.Ground)
            {
                return this.Ground();
            }

            if ((id == ObjectId.Invalid) || !this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("IT'S NOT HERE!");
                return VerbResult.Idle;
            }

            if (id == ObjectId.Bottle)
            {
                return this.Bottle();
            }

            if (id == ObjectId.Case)
            {
                return this.Case();
            }

            if (id == ObjectId.Barrel)
            {
                return this.Barrel();
            }

            return this.Unknown();
        }

        private VerbResult Unknown()
        {
            this.Print("YOU SEE NOTHING UNUSUAL.");
            return VerbResult.Idle;
        }

        private VerbResult Barrel()
        {
            this.Print("IT'S FILLED WITH RAINWATER.");
            return VerbResult.Idle;
        }

        private VerbResult Case()
        {
            this.Print("THERE'S A JEWEL INSIDE!");
            return VerbResult.Idle;
        }

        private VerbResult Bottle()
        {
            this.Print("THERE'S SOMETHING WRITTEN ON IT!");
            return VerbResult.Idle;
        }

        private VerbResult Ground()
        {
            if (this.State.Map.CurrentRoom != RoomId.OpenField)
            {
                this.Print("IT LOOKS LIKE GROUND!");
                return VerbResult.Idle;
            }

            this.Print("IT LOOKS LIKE SOMETHING'S BURIED HERE.");
            return VerbResult.Idle;
        }
    }
}
