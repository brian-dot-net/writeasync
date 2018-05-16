// <copyright file="Map.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;

    internal sealed class Map
    {
        private const int NumberOfRooms = 19;
        private const int NumberOfDirections = 6;

        private string[] roomDescriptions;
        private string[] directions;
        private RoomId[,] map;

        public Map()
        {
            this.directions = new string[11];
            this.InitMap();
            this.CurrentRoom = RoomId.LivingRoom;
        }

        public RoomId CurrentRoom { get; set; }

        public IEnumerable<string> Directions()
        {
            for (int i = 0; i <= 5; ++i)
            {
                if (this.map[(int)this.CurrentRoom, i] > RoomId.None)
                {
                    yield return this.directions[i];
                }
            }
        }

        public string Describe()
        {
            return this.roomDescriptions[(int)this.CurrentRoom];
        }

        public void SetMap(RoomId room, int dir, RoomId next)
        {
            this.map[(int)room, dir] = next;
        }

        public MoveResult Move(int dir)
        {
            RoomId next = this.map[(int)this.CurrentRoom, dir];
            if (next == RoomId.Blocked)
            {
                return MoveResult.Blocked;
            }

            if ((next <= 0) || (next > RoomId.Blocked))
            {
                return MoveResult.Invalid;
            }

            this.CurrentRoom = next;
            return MoveResult.OK;
        }

        private void InitMap()
        {
            this.map = new RoomId[NumberOfRooms + 1, NumberOfDirections + 1];

            this.directions[0] = "NORTH";
            this.directions[1] = "SOUTH";
            this.directions[2] = "EAST";
            this.directions[3] = "WEST";
            this.directions[4] = "UP";
            this.directions[5] = "DOWN";

            Queue<RoomId> data = new Queue<RoomId>();

            // LIVING ROOM
            data.Enqueue(RoomId.FrontYard);
            data.Enqueue(RoomId.Library);
            data.Enqueue(RoomId.Kitchen);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // KITCHEN
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.LivingRoom);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // LIBRARY
            data.Enqueue(RoomId.LivingRoom);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // FRONT YARD
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.LivingRoom);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.Garage);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // GARAGE
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.FrontYard);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // OPEN FIELD
            data.Enqueue(RoomId.LongWindingRoad1);
            data.Enqueue(RoomId.EdgeOfForest);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // EDGE OF FOREST
            data.Enqueue(RoomId.OpenField);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // BRANCH OF TREE
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.EdgeOfForest);

            // LONG, WINDING ROAD (1)
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.OpenField);
            data.Enqueue(RoomId.LongWindingRoad2);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // LONG, WINDING ROAD (2)
            data.Enqueue(RoomId.LongWindingRoad3);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.LongWindingRoad1);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // LONG, WINDING ROAD (3)
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.LongWindingRoad2);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.SouthBankOfRiver);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // SOUTH BANK OF RIVER
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.LongWindingRoad3);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // BOAT
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // NORTH BANK OF RIVER
            data.Enqueue(RoomId.WellTraveledRoad);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // WELL-TRAVELED ROAD
            data.Enqueue(RoomId.SouthOfCastle);
            data.Enqueue(RoomId.NorthBankOfRiver);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // SOUTH OF CASTLE
            data.Enqueue(RoomId.Blocked);
            data.Enqueue(RoomId.WellTraveledRoad);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);

            // NARROW HALL
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.LargeHall);
            data.Enqueue(RoomId.None);

            // LARGE HALL
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.NarrowHall);

            // TOP OF TREE
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.None);
            data.Enqueue(RoomId.BranchOfTree);

            for (int i = 1; i <= NumberOfRooms; ++i)
            {
                for (int j = 0; j < NumberOfDirections; ++j)
                {
                    this.SetMap((RoomId)i, j, data.Dequeue());
                }
            }

            this.InitDescriptions();
        }

        private void InitDescriptions()
        {
            this.roomDescriptions = new string[NumberOfRooms + 1]
            {
                string.Empty,
                "IN YOUR LIVING ROOM.",
                "IN THE KITCHEN.",
                "IN THE LIBRARY.",
                "IN THE FRONT YARD.",
                "IN THE GARAGE.",
                "IN AN OPEN FIELD.",
                "AT THE EDGE OF A FOREST.",
                "ON A BRANCH OF A TREE.",
                "ON A LONG, WINDING ROAD.",
                "ON A LONG, WINDING ROAD.",
                "ON A LONG, WINDING ROAD.",
                "ON THE SOUTH BANK OF A RIVER.",
                "INSIDE THE WOODEN BOAT.",
                "ON THE NORTH BANK OF A RIVER.",
                "ON A WELL-TRAVELED ROAD.",
                "IN FRONT OF A LARGE CASTLE.",
                "IN A NARROW HALL.",
                "IN A LARGE HALL.",
                "ON THE TOP OF A TREE."
            };
        }
    }
}