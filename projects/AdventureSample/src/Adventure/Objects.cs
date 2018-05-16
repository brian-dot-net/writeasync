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

        public bool Carrying(ObjectId id) => this.Ref(id).Carrying;

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

        public IEnumerable<string> Here(RoomId currentRoom)
        {
            foreach (ObjectRef obj in this.objects)
            {
                if (currentRoom == obj.Room)
                {
                    yield return obj.Name;
                }
            }
        }

        public bool IsHere(ObjectId id, RoomId currentRoom)
        {
            return (this.Ref(id).RawRoom == (int)currentRoom) || this.Carrying(id);
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

        public ObjectRef Ref(ObjectId id) => this.objects[(int)id];

        public void Hide(ObjectId id) => this.Drop(id, RoomId.None);

        public void Take(ObjectId id) => this.Drop(id, RoomId.Inventory);

        public void Drop(ObjectId id, RoomId room) => this.Ref(id).RawRoom = (int)room;

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