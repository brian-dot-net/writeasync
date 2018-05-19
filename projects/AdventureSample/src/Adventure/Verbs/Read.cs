// <copyright file="Read.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Read : Verb
    {
        public Read(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Unknown(ObjectId id)
        {
            this.Print("YOU CAN'T READ THAT!");
            return VerbResult.Idle;
        }

        public VerbResult Bottle(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("THERE'S NO BOTTLE HERE!");
                return VerbResult.Idle;
            }

            this.Print("IT READS: 'SECRET FORMULA'.");
            return VerbResult.Idle;
        }

        public VerbResult Dictionary(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("YOU DON'T SEE A DICTIONARY!");
                return VerbResult.Idle;
            }

            this.Print("IT SAYS: SODIUM CHLORIDE IS");
            this.Print("COMMON TABLE SALT.");
            return VerbResult.Idle;
        }

        public VerbResult Diary(ObjectId id)
        {
            if (!this.State.Objects.IsHere(id, this.State.Map.CurrentRoom))
            {
                this.Print("THERE'S NO DIARY HERE!");
                return VerbResult.Idle;
            }

            this.Print("IT SAYS: 'ADD SODIUM CHLORIDE PLUS THE");
            this.Print("FORMULA TO RAINWATER, TO REACH THE");
            this.Print("OTHER WORLD.' ");
            return VerbResult.Idle;
        }
    }
}
