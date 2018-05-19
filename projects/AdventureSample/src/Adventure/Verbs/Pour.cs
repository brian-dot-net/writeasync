// <copyright file="Pour.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Pour : Verb
    {
        public Pour(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T POUR THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Formula(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T HAVE THE BOTTLE!");
                return VerbResult.Idle;
            }

            if (this.State.FormulaPoured)
            {
                this.Print("THE BOTTLE IS EMPTY!");
                return VerbResult.Idle;
            }

            this.State.FormulaPoured = true;
            return this.Mixture();
        }

        public VerbResult Salt(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T HAVE THE SALT!");
                return VerbResult.Idle;
            }

            if (this.State.SaltPoured)
            {
                this.Print("THE SHAKER IS EMPTY!");
                return VerbResult.Idle;
            }

            this.State.SaltPoured = true;
            return this.Mixture();
        }

        private VerbResult Mixture()
        {
            if (this.State.Map.CurrentRoom == RoomId.Garage)
            {
                ++this.State.MixtureCount;
            }

            this.Print("POURED!");

            if (this.State.MixtureCount < 2)
            {
                return VerbResult.Idle;
            }

            this.Print("THERE IS AN EXPLOSION!");
            this.Print("EVERYTHING GOES BLACK!");
            this.Print("SUDDENLY YOU ARE ... ");
            this.Print(" ... SOMEWHERE ELSE!");

            this.State.Map.CurrentRoom = RoomId.OpenField;
            return VerbResult.Proceed;
        }
    }
}
