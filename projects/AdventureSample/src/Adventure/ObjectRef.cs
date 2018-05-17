// <copyright file="ObjectRef.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;

    internal sealed class ObjectRef
    {
        private readonly HashSet<string> tags;

        public ObjectRef(ObjectId id, string name, IEnumerable<string> tags, RoomId room, bool canGet = true)
        {
            this.Id = id;
            this.Name = name;
            this.Room = room;
            this.CanGet = canGet;
            this.tags = new HashSet<string>(tags);
        }

        public ObjectId Id { get; private set; }

        public string Name { get; private set; }

        public bool CanGet { get; private set; }

        public RoomId Room { get; set; }

        public bool Carrying => this.Room == RoomId.Inventory;

        public bool Matches(string tag) => this.tags.Contains(tag);
    }
}