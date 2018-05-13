// <copyright file="Objects.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;

    internal sealed class Objects
    {
        private const int NumberOfObjects = 17;

        private readonly ObjectRef[] objects;

        public Objects()
        {
            this.objects = InitObjects();
        }

        public bool Carrying(int id) => this.Ref(id).RawRoom == -1;

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
                ObjectRef obj = this.Ref(i);
                if (currentRoom == obj.Room)
                {
                    yield return obj.Name;
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
                ObjectRef obj = this.Ref(i);
                if (obj.Tag == noun)
                {
                    return obj;
                }
            }

            return null;
        }

        public ObjectRef Ref(int id) => this.objects[id];

        public void Show(int id, int room) => this.Ref(id).RawRoom = room;

        public void Hide(int id) => this.Show(id, 0);

        public void Take(int id) => this.Drop(id, -1);

        public void Drop(int id, int room) => this.Ref(id).RawRoom = room;

        private static ObjectRef[] InitObjects()
        {
            return new ObjectRef[]
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
        }
    }
}