// <copyright file="ObjectRef.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    internal sealed class ObjectRef
    {
        public ObjectRef(ObjectId id, string name, string tag, RoomId room, bool canGet = true)
        {
            this.Id = id;
            this.Name = name;
            this.Tag = tag;
            this.Room = room;
            this.CanGet = canGet;
        }

        public ObjectId Id { get; private set; }

        public string Name { get; private set; }

        public string Tag { get; private set; }

        public bool CanGet { get; private set; }

        public RoomId Room { get; set; }

        public bool Carrying => this.Room == RoomId.Inventory;
    }
}