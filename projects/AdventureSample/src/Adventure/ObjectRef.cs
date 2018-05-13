// <copyright file="ObjectRef.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    internal sealed class ObjectRef
    {
        public ObjectRef(int id, string name, string tag, int room)
        {
            this.Id = id;
            this.Name = name;
            this.Tag = tag;
            this.RawRoom = room;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Tag { get; private set; }

        public int RawRoom { get; set; }

        public int Room
        {
            get
            {
                int r = this.RawRoom;
                if (r > 127)
                {
                    r -= 128;
                }

                return r;
            }
        }

        public bool Carrying => this.RawRoom == -1;
    }
}