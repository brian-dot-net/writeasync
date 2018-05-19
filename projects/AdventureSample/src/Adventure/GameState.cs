// <copyright file="GameState.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    internal sealed class GameState
    {
        public GameState()
        {
            this.Objects = new Objects();
            this.Map = new Map();
        }

        public int InventoryItems { get; set; }

        public bool SaltPoured { get; set; }

        public bool FormulaPoured { get; set; }

        public int MixtureCount { get; set; }

        public bool WearingGloves { get; set; }

        public Objects Objects { get; private set; }

        public Map Map { get; private set; }
    }
}
