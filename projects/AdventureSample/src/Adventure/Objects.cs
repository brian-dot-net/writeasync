// <copyright file="Objects.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;

    internal sealed class Objects
    {
        private readonly ObjectRef[] objects;

        public Objects()
        {
            this.objects = InitObjects();
        }

        public bool Carrying(int id) => this.Ref(id).Carrying;

        public IEnumerable<string> Carrying()
        {
            foreach (ObjectRef obj in this.objects)
            {
                if (obj.Carrying)
                {
                    yield return obj.Name;
                }
            }
        }

        public IEnumerable<string> Here(int currentRoom)
        {
            foreach (ObjectRef obj in this.objects)
            {
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
            foreach (ObjectRef obj in this.objects)
            {
                if (obj.Tag == noun)
                {
                    return obj;
                }
            }

            return null;
        }

        public ObjectRef Ref(int id) => this.objects[id];

        public void Hide(int id) => this.Drop(id, 0);

        public void Take(int id) => this.Drop(id, -1);

        public void Drop(int id, int room) => this.Ref(id).RawRoom = room;

        private static ObjectRef[] InitObjects()
        {
            return new ObjectRef[]
            {
                new ObjectRef(ObjectId.Diary, "AN OLD DIARY", "DIA", 1),
                new ObjectRef(ObjectId.Box, "A SMALL BOX", "BOX", 1),
                new ObjectRef(ObjectId.Cabinet, "CABINET", "CAB", 130),
                new ObjectRef(ObjectId.Salt, "A SALT SHAKER", "SAL", 0),
                new ObjectRef(ObjectId.Dictionary, "A DICTIONARY", "DIC", 3),
                new ObjectRef(ObjectId.Barrel, "WOODEN BARREL", "BAR", 133),
                new ObjectRef(ObjectId.Bottle, "A SMALL BOTTLE", "BOT", 0),
                new ObjectRef(ObjectId.Ladder, "A LADDER", "LAD", 4),
                new ObjectRef(ObjectId.Shovel, "A SHOVEL", "SHO", 5),
                new ObjectRef(ObjectId.Tree, "A TREE", "TRE", 135),
                new ObjectRef(ObjectId.Sword, "A GOLDEN SWORD", "SWO", 0),
                new ObjectRef(ObjectId.Boat, "A WOODEN BOAT", "BOA", 140),
                new ObjectRef(ObjectId.Fan, "A MAGIC FAN", "FAN", 8),
                new ObjectRef(ObjectId.Guard, "A NASTY-LOOKING GUARD", "GUA", 144),
                new ObjectRef(ObjectId.Case, "A GLASS CASE", "CAS", 146),
                new ObjectRef(ObjectId.Ruby, "A GLOWING RUBY", "RUB", 0),
                new ObjectRef(ObjectId.Gloves, "A PAIR OF RUBBER GLOVES", "GLO", 19)
            };
        }
    }
}