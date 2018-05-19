// <copyright file="Leave.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Leave : Verb
    {
        public Leave(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Any(ObjectId id)
        {
            if (this.State.Map.CurrentRoom == RoomId.Boat)
            {
                if ((id == ObjectId.Boat) || (id == ObjectId.Blank))
                {
                    this.State.Map.CurrentRoom = this.State.Objects.Ref(ObjectId.Boat).Room;
                    return VerbResult.Proceed;
                }

                this.Print("HUH?");
                return VerbResult.Idle;
            }

            this.Print("PLEASE GIVE A DIRECTION!");
            return VerbResult.Idle;
        }
    }
}
