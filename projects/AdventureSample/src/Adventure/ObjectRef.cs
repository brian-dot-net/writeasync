// <copyright file="ObjectRef.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    internal sealed class ObjectRef
    {
        public ObjectRef(ObjectId id, string name, string tag, int room)
        {
            this.Id = id;
            this.Name = name;
            this.Tag = tag;
            this.RawRoom = room;
        }

        public ObjectId Id { get; private set; }

        public string Name { get; private set; }

        public string Tag { get; private set; }

        public int RawRoom { get; set; }

        public bool CanGet => this.RawRoom < 128;

        public RoomId Room
        {
            get
            {
                int r = this.RawRoom;
                if (!this.CanGet)
                {
                    r -= 128;
                }

                return (RoomId)r;
            }
        }

        public bool Carrying => this.Room == RoomId.Inventory;
    }
}