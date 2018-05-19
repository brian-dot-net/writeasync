// <copyright file="Jump.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Verbs
{
    using System;

    internal sealed class Jump : Verb
    {
        public Jump(GameState state, Action<string> print)
            : base(state, print)
        {
        }

        public VerbResult Any()
        {
            if ((this.State.Map.CurrentRoom == RoomId.EdgeOfForest) || (this.State.Map.CurrentRoom == RoomId.BranchOfTree))
            {
                return this.Tree();
            }

            return this.Unknown();
        }

        private VerbResult Unknown()
        {
            this.Print("WHEE! THAT WAS FUN!");
            return VerbResult.Idle;
        }

        private VerbResult Tree()
        {
            if (this.State.Map.CurrentRoom == RoomId.BranchOfTree)
            {
                this.Print("YOU GRAB A HIGHER BRANCH ON THE");
                this.Print("TREE AND PULL YOURSELF UP....");
                this.State.Map.CurrentRoom = RoomId.TopOfTree;
                return VerbResult.Proceed;
            }

            this.Print("YOU GRAB THE LOWEST BRANCH OF THE");
            this.Print("TREE AND PULL YOURSELF UP....");
            this.State.Map.CurrentRoom = RoomId.BranchOfTree;
            return VerbResult.Proceed;
        }
    }
}
