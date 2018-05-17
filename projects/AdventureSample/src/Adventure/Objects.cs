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

        public ObjectId IdOf(string noun)
        {
            if (noun == string.Empty)
            {
                return ObjectId.Blank;
            }

            ObjectRef obj = this.Find(noun);
            if (obj == null)
            {
                return ObjectId.Invalid;
            }

            return obj.Id;
        }

        public ObjectRef Find(string noun)
        {
            foreach (ObjectRef obj in this.objects)
            {
                if (obj.Matches(noun))
                {
                    return obj;
                }
            }

            return null;
        }

        public ObjectRef Ref(ObjectId id) => this.objects[(int)id];

        public void Hide(ObjectId id) => this.Drop(id, RoomId.None);

        public void Take(ObjectId id) => this.Drop(id, RoomId.Inventory);

        public void Drop(ObjectId id, RoomId room) => this.Ref(id).Room = room;

        private static ObjectRef[] InitObjects()
        {
            return new ObjectRef[]
            {
                new ObjectRef(ObjectId.Diary, "AN OLD DIARY", Tags("DIA"), RoomId.LivingRoom),
                new ObjectRef(ObjectId.Box, "A SMALL BOX", Tags("BOX"), RoomId.LivingRoom),
                new ObjectRef(ObjectId.Cabinet, "CABINET", Tags("CAB"), RoomId.Kitchen, false),
                new ObjectRef(ObjectId.Salt, "A SALT SHAKER", Tags("SAL", "SHA"), RoomId.None),
                new ObjectRef(ObjectId.Dictionary, "A DICTIONARY", Tags("DIC"), RoomId.Library),
                new ObjectRef(ObjectId.Barrel, "WOODEN BARREL", Tags("BAR"), RoomId.Garage, false),
                new ObjectRef(ObjectId.Bottle, "A SMALL BOTTLE", Tags("BOT", "FOR"), RoomId.None),
                new ObjectRef(ObjectId.Ladder, "A LADDER", Tags("LAD"), RoomId.FrontYard),
                new ObjectRef(ObjectId.Shovel, "A SHOVEL", Tags("SHO"), RoomId.Garage),
                new ObjectRef(ObjectId.Tree, "A TREE", Tags("TRE"), RoomId.EdgeOfForest, false),
                new ObjectRef(ObjectId.Sword, "A GOLDEN SWORD", Tags("SWO"), RoomId.None),
                new ObjectRef(ObjectId.Boat, "A WOODEN BOAT", Tags("BOA"), RoomId.SouthBankOfRiver, false),
                new ObjectRef(ObjectId.Fan, "A MAGIC FAN", Tags("FAN"), RoomId.BranchOfTree),
                new ObjectRef(ObjectId.Guard, "A NASTY-LOOKING GUARD", Tags("GUA"), RoomId.SouthOfCastle, false),
                new ObjectRef(ObjectId.Case, "A GLASS CASE", Tags("CAS"), RoomId.LargeHall, false),
                new ObjectRef(ObjectId.Ruby, "A GLOWING RUBY", Tags("RUB"), RoomId.None),
                new ObjectRef(ObjectId.Gloves, "A PAIR OF RUBBER GLOVES", Tags("GLO"), RoomId.TopOfTree)
            };
        }

        private static IEnumerable<string> Tags(params string[] tags) => tags;
    }
}