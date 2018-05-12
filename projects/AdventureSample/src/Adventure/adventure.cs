//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by GWBas2CS.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Adventure;

internal sealed class adventure
{
    private const int NumberOfRooms = 19;
    private const int NumberOfDirections = 6;
    private const int MaxInventoryItems = 5;

    private readonly TextReader input;
    private readonly TextWriter output;
    private Queue DATA;
    private string[] roomDescriptions;
    private string[] directions;
    private int[,] map;
    private int currentRoom;
    private int inventoryItems;
    private bool saltPoured;
    private bool formulaPoured;
    private float mixtureCount;
    private bool wearingGloves;
    private int I_n;
    private int FL_n;
    private int RO_n;

    private Objects objects;

    public adventure(TextReader input, TextWriter output)
    {
        this.input = (input);
        this.output = (output);
    }

    public void Run()
    {
        while (this.Main() == VerbResult.RestartGame)
        {
        }
    }

    private void Init()
    {
        DATA = (new Queue());

        // LIVING ROOM
        DATA.Enqueue(4);
        DATA.Enqueue(3);
        DATA.Enqueue(2);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // KITCHEN
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(1);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // LIBRARY
        DATA.Enqueue(1);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // FRONT YARD
        DATA.Enqueue(0);
        DATA.Enqueue(1);
        DATA.Enqueue(0);
        DATA.Enqueue(5);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // GARAGE
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(4);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // OPEN FIELD
        DATA.Enqueue(9);
        DATA.Enqueue(7);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // EDGE OF FOREST
        DATA.Enqueue(6);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // BRANCH OF TREE
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(7);

        // LONG, WINDING ROAD (1)
        DATA.Enqueue(0);
        DATA.Enqueue(6);
        DATA.Enqueue(10);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // LONG, WINDING ROAD (2)
        DATA.Enqueue(11);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(9);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // LONG, WINDING ROAD (3)
        DATA.Enqueue(0);
        DATA.Enqueue(10);
        DATA.Enqueue(0);
        DATA.Enqueue(12);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // SOUTH BANK OF RIVER
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(11);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // BOAT
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // NORTH BANK OF RIVER
        DATA.Enqueue(15);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // WELL-TRAVELED ROAD
        DATA.Enqueue(16);
        DATA.Enqueue(14);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // SOUTH OF CASTLE
        DATA.Enqueue(128);
        DATA.Enqueue(15);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);

        // NARROW HALL
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(18);
        DATA.Enqueue(0);

        // LARGE HALL
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(17);

        // TOP OF TREE
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(0);
        DATA.Enqueue(8);

        currentRoom = (0);
        inventoryItems = (0);
        mixtureCount = (0);
        I_n = (0);
        FL_n = (0);
        RO_n = (0);
        roomDescriptions = new string[11];
        directions = new string[11];
    }

    private void CLS()
    {
        this.output.Write('\f');
        Console.Clear();
    }

    private static void DIM1_sa(out string[] a, int d1)
    {
        a = (new string[(d1) + (1)]);
        Array.Fill(a, "");
    }

    private void PRINT(string expression)
    {
        this.output.WriteLine(expression);
    }

    private string INPUT_s(string prompt)
    {
        this.output.Write((prompt) + ("? "));
        string v = this.input.ReadLine();
        return v.Trim();
    }

    private void PRINT_n(string expression)
    {
        this.output.Write(expression);
    }

    private int READ_n()
    {
        return (int)DATA.Dequeue();
    }

    private string READ_s()
    {
        return (string)(DATA.Dequeue());
    }

    private float INPUT_n(string prompt)
    {
        while (true)
        {
            this.output.Write((prompt) + ("? "));
            string v = this.input.ReadLine();
            v = (v.Trim());
            if ((v.Length) == (0))
            {
                return 0;
            }

            float r;
            if (float.TryParse(v, out r))
            {
                return r;
            }

            this.output.WriteLine("?Redo from start");
        }
    }

    private void PrintDirections()
    {
        PRINT_n("YOU CAN GO: ");
        for (int i = 0; i <= 5; ++i)
        {
            if (map[currentRoom, i] > 0)
            {
                PRINT_n(directions[i] + " ");
            }
        }

        PRINT("");
    }

    private void PrintObjects()
    {
        PRINT("YOU CAN SEE: ");
        bool atLeastOne = false;
        foreach (string name in objects.Here(currentRoom))
        {
            PRINT(" " + name);
            atLeastOne = true;
        }

        if (!atLeastOne)
        {
            PRINT(" NOTHING OF INTEREST");
        }
    }

    private void PrintDescription()
    {
        PRINT("");
        PRINT((("") + ("YOU ARE ")) + (roomDescriptions[currentRoom]));
    }

    private void FindRoomForObject(string noun)
    {
        FL_n = 0;
        for (I_n = 0; I_n < Objects.NumberOfObjects; ++I_n)
        {
            if (objects.objectTags[I_n] == noun)
            {
                FL_n = 1;
                RO_n = objects.objectRooms[I_n];
                if (RO_n > 127)
                {
                    RO_n -= 128;
                }

                break;
            }
        }
    }

    private void InitMap()
    {
        directions[0] = "NORTH";
        directions[1] = "SOUTH";
        directions[2] = "EAST";
        directions[3] = "WEST";
        directions[4] = "UP";
        directions[5] = "DOWN";

        for (int i = 1; i <= NumberOfRooms; ++i)
        {
            for (int j = 0; j < NumberOfDirections; ++j)
            {
                map[i, j] = READ_n();
            }
        }
    }

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

        for (int i = 0; i < Objects.NumberOfObjects; ++i)
        {
            ObjectRef obj = d[i];

            objects.objectNames[i] = obj.Name;
            objects.objectTags[i] = obj.Tag;
            objects.objectRooms[i] = obj.RawRoom;
        }
    }


    private void InitDescriptions()
    {
        roomDescriptions[1] = "IN YOUR LIVING ROOM.";
        roomDescriptions[2] = "IN THE KITCHEN.";
        roomDescriptions[3] = "IN THE LIBRARY.";
        roomDescriptions[4] = "IN THE FRONT YARD.";
        roomDescriptions[5] = "IN THE GARAGE.";
        roomDescriptions[6] = "IN AN OPEN FIELD.";
        roomDescriptions[7] = "AT THE EDGE OF A FOREST.";
        roomDescriptions[8] = "ON A BRANCH OF A TREE.";
        roomDescriptions[9] = "ON A LONG, WINDING ROAD.";
        roomDescriptions[10] = "ON A LONG, WINDING ROAD.";
        roomDescriptions[11] = "ON A LONG, WINDING ROAD.";
        roomDescriptions[12] = "ON THE SOUTH BANK OF A RIVER.";
        roomDescriptions[13] = "INSIDE THE WOODEN BOAT.";
        roomDescriptions[14] = "ON THE NORTH BANK OF A RIVER.";
        roomDescriptions[15] = "ON A WELL-TRAVELED ROAD.";
        roomDescriptions[16] = "IN FRONT OF A LARGE CASTLE.";
        roomDescriptions[17] = "IN A NARROW HALL.";
        roomDescriptions[18] = "IN A LARGE HALL.";
        roomDescriptions[19] = "ON THE TOP OF A TREE.";
    }

    private void PrintIntro()
    {
        PRINT("ALL YOUR LIFE YOU HAD HEARD THE STORIES");
        PRINT("ABOUT YOUR CRAZY UNCLE SIMON. HE WAS AN");
        PRINT("INVENTOR, WHO KEPT DISAPPEARING FOR");
        PRINT("LONG PERIODS OF TIME, NEVER TELLING");
        PRINT("ANYONE WHERE HE HAD BEEN.");
        PRINT("");
        PRINT("YOU NEVER BELIEVED THE STORIES, BUT");
        PRINT("WHEN YOUR UNCLE DIED AND LEFT YOU HIS");
        PRINT("DIARY, YOU LEARNED THAT THEY WERE TRUE.");
        PRINT("YOUR UNCLE HAD DISCOVERED A MAGIC");
        PRINT("LAND, AND A SECRET FORMULA THAT COULD");
        PRINT("TAKE HIM THERE. IN THAT LAND WAS A");
        PRINT("MAGIC RUBY, AND HIS DIARY CONTAINED");
        PRINT("THE INSTRUCTIONS FOR GOING THERE TO");
        PRINT("FIND IT.");
        INPUT_n("");
    }

    private VerbResult Main()
    {
        this.Init();
        ; // ** THE QUEST **
        ; // **
        ; // ** An adventure game
        ; // 
        CLS();

        DIM1_sa(out roomDescriptions, NumberOfRooms);
        objects = new Objects();
        map = new int[NumberOfRooms + 1, NumberOfDirections + 1];
        PRINT(("") + ("Please stand by .... "));
        PRINT("");
        PRINT("");

        InitMap();

        InitObjects();

        InitDescriptions();

        currentRoom = 1;
        inventoryItems = 0;
        saltPoured = false;
        formulaPoured = false;
        mixtureCount = 1;
        wearingGloves = false;

        PrintIntro();

        CLS();

        VerbRoutines verbRoutines = new VerbRoutines(UnknownVerb);
        InitHandlers(verbRoutines);

        while (true)
        {
            PrintDescription();

            PrintDirections();

            PrintObjects();

            while (true)
            {
                string cmd = "";
                do
                {
                    PRINT("");
                    cmd = (INPUT_s("WHAT NOW"));
                }
                while (cmd == "");

                Command command = Command.Parse(cmd);

                string noun = command.Noun;
                if (noun == "SHA")
                {
                    noun = "SAL";
                }

                if (noun == "FOR")
                {
                    noun = "BOT";
                }

                VerbResult ret = verbRoutines.Handle(command.Verb, noun);
                if (ret == VerbResult.Idle)
                {
                    // NO-OP
                }
                else if (ret == VerbResult.Proceed)
                {
                    break;
                }
                else
                {
                    return ret;
                }
            }
        }
    }

    private void InitHandlers(VerbRoutines verbRoutines)
    {
        verbRoutines.Add("GO", Go);
        verbRoutines.Add("GET", Get);
        verbRoutines.Add("TAK", Get);
        verbRoutines.Add("DRO", Drop);
        verbRoutines.Add("THR", Drop);
        verbRoutines.Add("INV", Inventory);
        verbRoutines.Add("I", Inventory);
        verbRoutines.Add("LOO", Look);
        verbRoutines.Add("L", Look);
        verbRoutines.Add("EXA", Examine);
        verbRoutines.Add("QUI", Quit);
        verbRoutines.Add("REA", Read);
        verbRoutines.Add("OPE", Open);
        verbRoutines.Add("POU", Pour);
        verbRoutines.Add("CLI", Climb);
        verbRoutines.Add("JUM", Jump);
        verbRoutines.Add("DIG", Dig);
        verbRoutines.Add("ROW", Row);
        verbRoutines.Add("WAV", Wave);
        verbRoutines.Add("LEA", Leave);
        verbRoutines.Add("EXI", Leave);
        verbRoutines.Add("FIG", Fight);
        verbRoutines.Add("WEA", Wear);
    }

    private VerbResult UnknownVerb(string noun)
    {
        PRINT("I DON'T KNOW HOW TO DO THAT");
        return VerbResult.Idle;
    }

    private VerbResult Go(string noun)
    {
        int dir;
        if (noun == "NOR")
        {
            dir = 0;
        }
        else if (noun == "SOU")
        {
            dir = 1;
        }
        else if (noun == "EAS")
        {
            dir = 2;
        }
        else if (noun == "WES")
        {
            dir = 3;
        }
        else if (noun == "UP")
        {
            dir = 4;
        }
        else if (noun == "DOW")
        {
            dir = 5;
        }
        else if ((noun == "BOA") && (objects.objectRooms[11] == (currentRoom + 128)))
        {
            currentRoom = 13;
            return VerbResult.Proceed;
        }
        else
        {
            PRINT("YOU CAN'T GO THERE!");
            return VerbResult.Idle;
        }

        return Move(dir);
    }

    private VerbResult Move(int dir)
    {
        int next = map[currentRoom, dir];
        if ((next > 0) && (next < 128))
        {
            currentRoom = next;
            return VerbResult.Proceed;
        }

        if (next == 128)
        {
            PRINT("THE GUARD WON'T LET YOU!");
        }
        else
        {
            PRINT("YOU CAN'T GO THERE!");
        }

        return VerbResult.Idle;
    }

    private VerbResult Get(string noun)
    {
        FindRoomForObject(noun);

        if (FL_n == 0)
        {
            PRINT("YOU CAN'T GET THAT!");
        }
        else if (RO_n == -1)
        {
            PRINT("YOU ALREADY HAVE IT!");
        }
        else if (objects.objectRooms[I_n] > 127)
        {
            PRINT("YOU CAN'T GET THAT!");
        }
        else if (RO_n != currentRoom)
        {
            PRINT("THAT'S NOT HERE!");
        }
        else if (inventoryItems > MaxInventoryItems)
        {
            PRINT("YOU CAN'T CARRY ANY MORE.");
        }
        else if ((currentRoom == 18) && (noun == "RUB"))
        {
            PRINT("CONGRATULATIONS! YOU'VE WON!");
            return PlayAgain();
        }
        else
        {
            ++inventoryItems;
            objects.objectRooms[I_n] = -1;
            PRINT("TAKEN.");
        }

        return VerbResult.Idle;
    }

    private VerbResult Drop(string noun)
    {
        FindRoomForObject(noun);

        if ((FL_n == 0) || (RO_n != -1))
        {
            PRINT("YOU DON'T HAVE THAT!");
        }
        else
        {
            --inventoryItems;
            objects.objectRooms[I_n] = currentRoom;
            PRINT("DROPPED.");
        }

        return VerbResult.Idle;
    }

    private VerbResult Inventory(string noun)
    {
        bool atLeastOne = false;
        PRINT("YOU ARE CARRYING:");
        foreach (string name in objects.Carrying())
        {
            PRINT(" " + name);
            atLeastOne = true;
        }

        if (!atLeastOne)
        {
            PRINT(" NOTHING");
        }

        return VerbResult.Idle;
    }

    private VerbResult Look(string noun)
    {
        if (noun == "")
        {
            return VerbResult.Proceed;
        }

        Examine(noun);
        return VerbResult.Idle;
    }

    private VerbResult Examine(string noun)
    {
        if (noun == "GRO")
        {
            if (currentRoom != 6)
            {
                PRINT("IT LOOKS LIKE GROUND!");
            }
            else
            {
                PRINT("IT LOOKS LIKE SOMETHING'S BURIED HERE.");
            }
        }
        else
        {
            FindRoomForObject(noun);

            if ((RO_n != currentRoom) && (RO_n != -1))
            {
                PRINT("IT'S NOT HERE!");
            }
            else if (noun == "BOT")
            {
                PRINT("THERE'S SOMETHING WRITTEN ON IT!");
            }
            else if (noun == "CAS")
            {
                PRINT("THERE'S A JEWEL INSIDE!");
            }
            else if (noun == "BAR")
            {
                PRINT("IT'S FILLED WITH RAINWATER.");
            }
            else
            {
                PRINT("YOU SEE NOTHING UNUSUAL.");
            }
        }

        return VerbResult.Idle;
    }

    private VerbResult Quit(string noun)
    {
        PRINT_n("ARE YOU SURE YOU WANT TO QUIT (Y/N)");
        string quit = INPUT_s("");
        if (quit != "N")
        {
            return PlayAgain();
        }

        return VerbResult.Idle;
    }

    private VerbResult PlayAgain()
    {
        while (true)
        {
            PRINT_n("WOULD YOU LIKE TO PLAY AGAIN (Y/N)");
            string quit = INPUT_s("");
            if (quit == "Y")
            {
                return VerbResult.RestartGame;
            }

            if (quit == "N")
            {
                return VerbResult.EndGame;
            }
        }
    }

    private VerbResult Read(string noun)
    {
        if (noun == "DIA")
        {
            if (!objects.IsHere(0, currentRoom))
            {
                PRINT("THERE'S NO DIARY HERE!");
            }
            else
            {
                PRINT("IT SAYS: 'ADD SODIUM CHLORIDE PLUS THE");
                PRINT("FORMULA TO RAINWATER, TO REACH THE");
                PRINT("OTHER WORLD.' ");
            }
        }
        else if (noun == "DIC")
        {
            if (!objects.IsHere(4, currentRoom))
            {
                PRINT("YOU DON'T SEE A DICTIONARY!");
            }
            else
            {
                PRINT("IT SAYS: SODIUM CHLORIDE IS");
                PRINT("COMMON TABLE SALT.");
            }
        }
        else if (noun == "BOT")
        {
            if (!objects.IsHere(6, currentRoom))
            {
                PRINT("THERE'S NO BOTTLE HERE!");
            }
            else
            {
                PRINT("IT READS: 'SECRET FORMULA'.");
            }
        }
        else
        {
            PRINT("YOU CAN'T READ THAT!");
        }

        return VerbResult.Idle;
    }

    private VerbResult Open(string noun)
    {
        if (noun == "BOX")
        {
            if (!objects.IsHere(1, currentRoom))
            {
                PRINT("THERE'S NO BOX HERE!");
            }
            else
            {
                objects.objectRooms[6] = currentRoom;
                PRINT("SOMETHING FELL OUT!");
            }
        }
        else if (noun == "CAB")
        {
            if (currentRoom != 2)
            {
                PRINT("THERE'S NO CABINET HERE!");
            }
            else
            {
                PRINT("THERE'S SOMETHING INSIDE!");
                objects.objectRooms[3] = 2;
            }
        }
        else if (noun == "CAS")
        {
            if (currentRoom != 18)
            {
                PRINT("THERE'S NO CASE HERE!");
            }
            else if (!wearingGloves)
            {
                PRINT("THE CASE IS ELECTRIFIED!");
            }
            else
            {
                PRINT("THE GLOVES INSULATE AGAINST THE");
                PRINT("ELECTRICITY! THE CASE OPENS!");
                objects.objectRooms[15] = 18;
            }
        }
        else
        {
            PRINT("YOU CAN'T OPEN THAT!");
        }

        return VerbResult.Idle;
    }

    private VerbResult Pour(string noun)
    {
        bool poured;
        if (noun == "SAL")
        {
            poured = PourSalt();
        }
        else if (noun == "BOT")
        {
            poured = PourFormula();
        }
        else
        {
            PRINT("YOU CAN'T POUR THAT!");
            poured = false;
        }

        if (poured)
        {
            poured = PourMixture();
        }

        if (poured)
        {
            return VerbResult.Proceed;
        }

        return VerbResult.Idle;
    }

    private bool PourFormula()
    {
        if (!objects.IsHere(6, currentRoom))
        {
            PRINT("YOU DON'T HAVE THE BOTTLE!");
            return false;
        }
        else if (formulaPoured)
        {
            PRINT("THE BOTTLE IS EMPTY!");
            return false;
        }
        else
        {
            formulaPoured = true;
            return true;
        }
    }

    private bool PourSalt()
    {
        if (!objects.IsHere(3, currentRoom))
        {
            PRINT("YOU DON'T HAVE THE SALT!");
            return false;
        }
        else if (saltPoured)
        {
            PRINT("THE SHAKER IS EMPTY!");
            return false;
        }
        else
        {
            saltPoured = true;
            return true;
        }
    }

    private bool PourMixture()
    {
        if (currentRoom == 5)
        {
            ++mixtureCount;
        }

        PRINT("POURED!");

        if (mixtureCount < 3)
        {
            return false;
        }

        PRINT("THERE IS AN EXPLOSION!");
        PRINT("EVERYTHING GOES BLACK!");
        PRINT("SUDDENLY YOU ARE ... ");
        PRINT(" ... SOMEWHERE ELSE!");

        currentRoom = 6;
        return true;
    }

    private VerbResult Climb(string noun)
    {
        if (noun == "TRE")
        {
            if (currentRoom != 7)
            {
                PRINT("THERE'S NO TREE HERE!");
            }
            else
            {
                PRINT("YOU CAN'T REACH THE BRANCHES!");
            }
        }
        else if (noun == "LAD")
        {
            if (!objects.IsHere(7, currentRoom))
            {
                PRINT("YOU DON'T HAVE THE LADDER!");
            }
            else if (currentRoom != 7)
            {
                PRINT("WHATEVER FOR?");
            }
            else
            {
                PRINT("THE LADDER SINKS UNDER YOUR WEIGHT!");
                PRINT("IT DISAPPEARS INTO THE GROUND!");
                objects.objectRooms[7] = 0;
            }
        }
        else
        {
            PRINT("IT WON'T DO ANY GOOD.");
        }

        return VerbResult.Idle;
    }

    private VerbResult Jump(string noun)
    {
        if ((currentRoom != 7) && (currentRoom != 8))
        {
            PRINT("WHEE! THAT WAS FUN!");
            return VerbResult.Idle;
        }
        else if (currentRoom == 8)
        {
            PRINT("YOU GRAB A HIGHER BRANCH ON THE");
            PRINT("TREE AND PULL YOURSELF UP....");
            currentRoom = 19;
            return VerbResult.Proceed;
        }
        else
        {
            PRINT("YOU GRAB THE LOWEST BRANCH OF THE");
            PRINT("TREE AND PULL YOURSELF UP....");
            currentRoom = 8;
            return VerbResult.Proceed;
        }
    }

    private VerbResult Dig(string noun)
    {
        if ((noun != "HOL") && (noun != "GRO") && (noun != ""))
        {
            PRINT("YOU CAN'T DIG THAT!");
        }
        else if (!objects.IsHere(8, currentRoom))
        {
            PRINT("YOU DON'T HAVE A SHOVEL!");
        }
        else if (currentRoom != 6)
        {
            PRINT("YOU DON'T FIND ANYTHING.");
        }
        else if (objects.objectRooms[10] != 0)
        {
            PRINT("THERE'S NOTHING ELSE THERE!");
        }
        else
        {
            PRINT("THERE'S SOMETHING THERE!");
            objects.objectRooms[10] = 6;
        }

        return VerbResult.Idle;
    }

    private VerbResult Row(string noun)
    {
        if ((noun != "BOA") && (noun != ""))
        {
            PRINT("HOW CAN YOU ROW THAT?");
        }
        else if (currentRoom != 13)
        {
            PRINT("YOU'RE NOT IN THE BOAT!");
        }
        else
        {
            PRINT("YOU DON'T HAVE AN OAR!");
        }

        return VerbResult.Idle;
    }

    private VerbResult Wave(string noun)
    {
        if (noun != "FAN")
        {
            PRINT("YOU CAN'T WAVE THAT!");
        }
        else if (!objects.IsHere(12, currentRoom))
        {
            PRINT("YOU DON'T HAVE THE FAN!");
        }
        else if (currentRoom != 13)
        {
            PRINT("YOU FEEL A REFRESHING BREEZE!");
        }
        else
        {
            PRINT("A POWERFUL BREEZE PROPELS THE BOAT");
            PRINT("TO THE OPPOSITE SHORE!");
            if (objects.objectRooms[11] == 140)
            {
                objects.objectRooms[11] = 142;
            }
            else
            {
                objects.objectRooms[11] = 140;
            }
        }

        return VerbResult.Idle;
    }

    private VerbResult Leave(string noun)
    {
        if (currentRoom != 13)
        {
            PRINT("PLEASE GIVE A DIRECTION!");
            return VerbResult.Idle;
        }
        else if ((noun != "BOA") && (noun != ""))
        {
            PRINT("HUH?");
            return VerbResult.Idle;
        }
        else
        {
            currentRoom = objects.objectRooms[11] - 128;
            return VerbResult.Proceed;
        }
    }

    private VerbResult Fight(string noun)
    {
        if (noun == "")
        {
            PRINT("WHOM DO YOU WANT TO FIGHT?");
        }
        else if (noun != "GUA")
        {
            PRINT(("") + ("YOU CAN'T FIGHT HIM!"));
        }
        else if (currentRoom != 16)
        {
            PRINT("THERE'S NO GUARD HERE!");
        }
        else if (!objects.Carrying(10))
        {
            PRINT("YOU DON'T HAVE A WEAPON!");
        }
        else
        {
            PRINT("THE GUARD, NOTICING YOUR SWORD,");
            PRINT("WISELY RETREATS INTO THE CASTLE.");
            map[16, 0] = 17;
            objects.objectRooms[13] = 0;
        }

        return VerbResult.Idle;
    }

    private VerbResult Wear(string noun)
    {
        if (noun != "GLO")
        {
            PRINT("YOU CAN'T WEAR THAT!");
        }
        else if (!objects.IsHere(16, currentRoom))
        {
            PRINT("YOU DON'T HAVE THE GLOVES.");
        }
        else
        {
            PRINT("YOU ARE NOW WEARING THE GLOVES.");
            wearingGloves = true;
        }

        return VerbResult.Idle;
    }

    private sealed class ObjectRef
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

        public int RawRoom { get; private set; }

        public int Room => this.RawRoom & 127;
    }

    private sealed class Objects
    {
        public const int NumberOfObjects = 17;

        public string[] objectNames;
        public string[] objectTags;
        public int[] objectRooms;

        public Objects()
        {
            this.objectRooms = new int[NumberOfObjects + 1];
            DIM1_sa(out this.objectNames, NumberOfObjects);
            DIM1_sa(out this.objectTags, NumberOfObjects);
        }

        public bool Carrying(int id) => this.objectRooms[id] == -1;

        public IEnumerable<string> Carrying()
        {
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                if (this.Carrying(i))
                {
                    yield return this.objectNames[i];
                }
            }
        }

        public IEnumerable<string> Here(int currentRoom)
        {
            for (int i = 0; i < NumberOfObjects; ++i)
            {
                if (currentRoom == (this.objectRooms[i] & 127))
                {
                    yield return this.objectNames[i];
                }
            }
        }

        public bool IsHere(int id, int currentRoom)
        {
            return (this.objectRooms[id] == currentRoom) || this.Carrying(id);
        }
    }
}