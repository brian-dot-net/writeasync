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
            return (this.Ref(id).Room == currentRoom) || this.Carrying(id);
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
                new ObjectRef(ObjectId.Diary, "AN OLD DIARY", "DIA", RoomId.LivingRoom),
                new ObjectRef(ObjectId.Box, "A SMALL BOX", "BOX", RoomId.LivingRoom),
                new ObjectRef(ObjectId.Cabinet, "CABINET", "CAB", RoomId.Kitchen, false),
                new ObjectRef(ObjectId.Salt, "A SALT SHAKER", "SAL", RoomId.None),
                new ObjectRef(ObjectId.Dictionary, "A DICTIONARY", "DIC", RoomId.Library),
                new ObjectRef(ObjectId.Barrel, "WOODEN BARREL", "BAR", RoomId.Garage, false),
                new ObjectRef(ObjectId.Bottle, "A SMALL BOTTLE", "BOT", RoomId.None),
                new ObjectRef(ObjectId.Ladder, "A LADDER", "LAD", RoomId.FrontYard),
                new ObjectRef(ObjectId.Shovel, "A SHOVEL", "SHO", RoomId.Garage),
                new ObjectRef(ObjectId.Tree, "A TREE", "TRE", RoomId.EdgeOfForest, false),
                new ObjectRef(ObjectId.Sword, "A GOLDEN SWORD", "SWO", RoomId.None),
                new ObjectRef(ObjectId.Boat, "A WOODEN BOAT", "BOA", RoomId.SouthBankOfRiver, false),
                new ObjectRef(ObjectId.Fan, "A MAGIC FAN", "FAN", RoomId.BranchOfTree),
                new ObjectRef(ObjectId.Guard, "A NASTY-LOOKING GUARD", "GUA", RoomId.SouthOfCastle, false),
                new ObjectRef(ObjectId.Case, "A GLASS CASE", "CAS", RoomId.LargeHall, false),
                new ObjectRef(ObjectId.Ruby, "A GLOWING RUBY", "RUB", RoomId.None),
                new ObjectRef(ObjectId.Gloves, "A PAIR OF RUBBER GLOVES", "GLO", RoomId.TopOfTree)
            };
        }
    }
}