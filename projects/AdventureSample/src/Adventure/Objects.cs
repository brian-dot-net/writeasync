// <copyright file="Objects.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;

    internal sealed class Objects
    {
        private const int NumberOfObjects = 17;

        private readonly string[] objectNames;
        private readonly string[] objectTags;
        private readonly int[] objectRooms;

        public Objects()
        {
            this.objectRooms = new int[NumberOfObjects + 1];
            this.objectNames = new string[NumberOfObjects + 1];
            this.objectTags = new string[NumberOfObjects + 1];

            this.InitObjects();
        }

        public bool Carrying(int id) => this.objectRooms[id] == -1;

        public IEnumerable<string> Carrying()
        {
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                if (this.Carrying(i))
                {
                    yield return this.Ref(i).Name;
                }
            }
        }

        public IEnumerable<string> Here(int currentRoom)
        {
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                if (currentRoom == this.Ref(i).Room)
                {
                    yield return this.Ref(i).Name;
                }
            }
        }

        public bool IsHere(int id, int currentRoom)
        {
            return (this.Ref(id).RawRoom == currentRoom) || this.Carrying(id);
        }

        public ObjectRef Find(string noun)
        {
            for (int i = 0; i < Objects.NumberOfObjects; ++i)
            {
                if (this.Ref(i).Tag == noun)
                {
                    return this.Ref(i);
                }
            }

            return null;
        }

        public ObjectRef Ref(int id) => new ObjectRef(id, this.objectNames[id], this.objectTags[id], this.objectRooms[id]);

        public void Show(int id, int room) => this.objectRooms[id] = room;

        public void Hide(int id) => this.Show(id, 0);

        public void Take(int id) => this.Drop(id, -1);

        public void Drop(int id, int room) => this.objectRooms[id] = room;

        private void InitObjects()
        {
            ObjectRef[] d = new ObjectRef[]
            {
                new ObjectRef(0, "AN OLD DIARY", "DIA", 1),
                new ObjectRef(1, "A SMALL BOX", "BOX", 1),
                new ObjectRef(2, "CABINET", "CAB", 130),
                new ObjectRef(3, "A SALT SHAKER", "SAL", 0),
                new ObjectRef(4, "A DICTIONARY", "DIC", 3),
                new ObjectRef(5, "WOODEN BARREL", "BAR", 133),
                new ObjectRef(6, "A SMALL BOTTLE", "BOT", 0),
                new ObjectRef(7, "A LADDER", "LAD", 4),
                new ObjectRef(8, "A SHOVEL", "SHO", 5),
                new ObjectRef(9, "A TREE", "TRE", 135),
                new ObjectRef(10, "A GOLDEN SWORD", "SWO", 0),
                new ObjectRef(11, "A WOODEN BOAT", "BOA", 140),
                new ObjectRef(12, "A MAGIC FAN", "FAN", 8),
                new ObjectRef(13, "A NASTY-LOOKING GUARD", "GUA", 144),
                new ObjectRef(14, "A GLASS CASE", "CAS", 146),
                new ObjectRef(15, "A GLOWING RUBY", "RUB", 0),
                new ObjectRef(16, "A PAIR OF RUBBER GLOVES", "GLO", 19)
            };

            for (int i = 0; i < NumberOfObjects; ++i)
            {
                ObjectRef obj = d[i];

                this.objectNames[i] = obj.Name;
                this.objectTags[i] = obj.Tag;
                this.objectRooms[i] = obj.RawRoom;
            }
        }
    }
}