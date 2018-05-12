//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by GWBas2CS.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using Adventure;

internal sealed class adventure
{
    private readonly TextReader input;
    private readonly TextWriter output;
    private Queue DATA;
    private string[] roomDescriptions;
    private string[] objectNames;
    private string[] objectTags;
    private string[] directions;
    private int[] objectRooms;
    private int[,] map;
    private string noun;
    private int numberOfRooms;
    private int numberOfObjects;
    private int numberOfDirections;
    private int maxInventoryItems;
    private int currentRoom;
    private int inventoryItems;
    private float saltPoured;
    private float formulaPoured;
    private float mixtureCount;
    private float wearingGloves;
    private int I_n;
    private int FL_n;
    private int RO_n;

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

        // OBJECT #0
        DATA.Enqueue("AN OLD DIARY");
        DATA.Enqueue("DIA");
        DATA.Enqueue(1);

        // OBJECT #1
        DATA.Enqueue("A SMALL BOX");
        DATA.Enqueue("BOX");
        DATA.Enqueue(1);

        // OBJECT #2
        DATA.Enqueue("CABINET");
        DATA.Enqueue("CAB");
        DATA.Enqueue(130);

        // OBJECT #3
        DATA.Enqueue("A SALT SHAKER");
        DATA.Enqueue("SAL");
        DATA.Enqueue(0);

        // OBJECT #4
        DATA.Enqueue("A DICTIONARY");
        DATA.Enqueue("DIC");
        DATA.Enqueue(3);

        // OBJECT #5
        DATA.Enqueue("WOODEN BARREL");
        DATA.Enqueue("BAR");
        DATA.Enqueue(133);

        // OBJECT #6
        DATA.Enqueue("A SMALL BOTTLE");
        DATA.Enqueue("BOT");
        DATA.Enqueue(0);

        // OBJECT #7
        DATA.Enqueue("A LADDER");
        DATA.Enqueue("LAD");
        DATA.Enqueue(4);

        // OBJECT #8
        DATA.Enqueue("A SHOVEL");
        DATA.Enqueue("SHO");
        DATA.Enqueue(5);

        // OBJECT #9
        DATA.Enqueue("A TREE");
        DATA.Enqueue("TRE");
        DATA.Enqueue(135);

        // OBJECT #10
        DATA.Enqueue("A GOLDEN SWORD");
        DATA.Enqueue("SWO");
        DATA.Enqueue(0);

        // OBJECT #11
        DATA.Enqueue("A WOODEN BOAT");
        DATA.Enqueue("BOA");
        DATA.Enqueue(140);

        // OBJECT #12
        DATA.Enqueue("A MAGIC FAN");
        DATA.Enqueue("FAN");
        DATA.Enqueue(8);

        // OBJECT #13
        DATA.Enqueue("A NASTY-LOOKING GUARD");
        DATA.Enqueue("GUA");
        DATA.Enqueue(144);

        // OBJECT #14
        DATA.Enqueue("A GLASS CASE");
        DATA.Enqueue("CAS");
        DATA.Enqueue(146);

        // OBJECT #15
        DATA.Enqueue("A GLOWING RUBY");
        DATA.Enqueue("RUB");
        DATA.Enqueue(0);

        // OBJECT #16
        DATA.Enqueue("A PAIR OF RUBBER GLOVES");
        DATA.Enqueue("GLO");
        DATA.Enqueue(19);

        noun = ("");
        numberOfRooms = (0);
        numberOfObjects = (0);
        numberOfDirections = (0);
        maxInventoryItems = (0);
        currentRoom = (0);
        inventoryItems = (0);
        saltPoured = (0);
        formulaPoured = (0);
        mixtureCount = (0);
        wearingGloves = (0);
        I_n = (0);
        FL_n = (0);
        RO_n = (0);
        roomDescriptions = new string[11];
        objectNames = new string[11];
        objectTags = new string[11];
        directions = new string[11];
        objectRooms = new int[11];
    }

    private void CLS()
    {
        this.output.Write('\f');
        Console.Clear();
    }

    private void DIM1_sa(out string[] a, int d1)
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

    private string MID_s(string x, int n, int m)
    {
        if ((n) > (x.Length))
        {
            return "";
        }

        int l = ((x.Length) - (n)) + (1);
        if ((m) > (l))
        {
            m = (l);
        }

        return x.Substring((n) - (1), m);
    }

    private string LEFT_s(string x, int n)
    {
        if ((n) > (x.Length))
        {
            return x;
        }

        return x.Substring(0, n);
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
        for (int i = 0; i < numberOfObjects; ++i)
        {
            if (currentRoom == (objectRooms[i] & 127))
            {
                PRINT(" " + objectNames[i]);
                atLeastOne = true;
            }
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

    private void FindRoomForObject()
    {
        if (numberOfObjects == 0)
        {
            return;
        }

        FL_n = 0;
        for (I_n = 0; I_n < numberOfObjects; ++I_n)
        {
            if (objectTags[I_n] == noun)
            {
                FL_n = 1;
                RO_n = objectRooms[I_n];
                break;
            }
        }

        if (FL_n != 0)
        {
            RO_n = objectRooms[I_n];
            if (RO_n > 127)
            {
                RO_n -= 128;
            }
        }
    }

    private void InitMap()
    {
        if (numberOfRooms == 0)
        {
            return;
        }

        directions[0] = "NORTH";
        directions[1] = "SOUTH";
        directions[2] = "EAST";
        directions[3] = "WEST";
        directions[4] = "UP";
        directions[5] = "DOWN";

        for (int i = 1; i <= numberOfRooms; ++i)
        {
            for (int j = 0; j < numberOfDirections; ++j)
            {
                map[i, j] = READ_n();
            }
        }
    }

    private void InitObjects()
    {
        if (numberOfObjects == 0)
        {
            return;
        }

        for (int i = 0; i < numberOfObjects; ++i)
        {
            objectNames[i] = READ_s();
            objectTags[i] = READ_s();
            objectRooms[i] = READ_n();
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
        CLS() // Put a statement here to clear the screen. If you are using a Radio Shack Model I, III, or 4, add a CLEAR statement. (See text.)
        ;
        numberOfRooms = (19);
        numberOfObjects = (17);
        numberOfDirections = (6);
        maxInventoryItems = (5);
        DIM1_sa(out roomDescriptions, numberOfRooms);
        objectRooms = new int[numberOfObjects + 1];
        DIM1_sa(out objectNames, numberOfObjects);
        DIM1_sa(out objectTags, numberOfObjects);
        map = new int[numberOfRooms + 1, numberOfDirections + 1];
        PRINT(("") + ("Please stand by .... "));
        PRINT("");
        PRINT("");

        InitMap();

        InitObjects();

        InitDescriptions();

        currentRoom = (1);
        inventoryItems = (0);
        saltPoured = (0);
        formulaPoured = (0);
        mixtureCount = (1);
        wearingGloves = (0);

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
                string verb = Parser();

                VerbResult ret = verbRoutines.Handle(verb);
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

    private VerbResult UnknownVerb()
    {
        PRINT("I DON'T KNOW HOW TO DO THAT");
        return VerbResult.Idle;
    }

    private string Parser()
    {
        string command = "";
        do
        {
            PRINT("");
            command = (INPUT_s("WHAT NOW"));
        }
        while (command == "");

        int c = 0;
        string verb = "";
        noun = "";

        while (true)
        {
            c = c + 1;
            if (c > command.Length)
            {
                break;
            }

            string wordPart = MID_s(command, c, 1);
            if (wordPart == " ")
            {
                break;
            }

            verb += wordPart;
        }

        while (true)
        {
            c = c + 1;
            if (c > command.Length)
            {
                break;
            }

            string wordPart = MID_s(command, c, 1);
            if (wordPart == " ")
            {
                break;
            }

            noun += wordPart;
        }

        if (verb.Length > 3)
        {
            verb = LEFT_s(verb, 3);
        }

        if (noun.Length > 3)
        {
            noun = LEFT_s(noun, 3);
        }

        if (noun == "SHA")
        {
            noun = "SAL";
        }

        if (noun == "FOR")
        {
            noun = "BOT";
        }

        return verb;
    }

    private VerbResult Go()
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
        else if ((noun == "BOA") && (objectRooms[11] == (currentRoom + 128)))
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

    private VerbResult Get()
    {
        FindRoomForObject();

        if (FL_n == 0)
        {
            PRINT("YOU CAN'T GET THAT!");
        }
        else if (RO_n == -1)
        {
            PRINT("YOU ALREADY HAVE IT!");
        }
        else if (objectRooms[I_n] > 127)
        {
            PRINT("YOU CAN'T GET THAT!");
        }
        else if (RO_n != currentRoom)
        {
            PRINT("THAT'S NOT HERE!");
        }
        else if (inventoryItems > maxInventoryItems)
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
            objectRooms[I_n] = -1;
            PRINT("TAKEN.");
        }

        return VerbResult.Idle;
    }

    private VerbResult Drop()
    {
        FindRoomForObject();

        if ((FL_n == 0) || (RO_n != -1))
        {
            PRINT("YOU DON'T HAVE THAT!");
        }
        else
        {
            --inventoryItems;
            objectRooms[I_n] = currentRoom;
            PRINT("DROPPED.");
        }

        return VerbResult.Idle;
    }

    private VerbResult Inventory()
    {
        bool atLeastOne = false;
        PRINT("YOU ARE CARRYING:");
        for (int i = 0; i < numberOfObjects; ++i)
        {
            if (objectRooms[i] == -1)
            {
                PRINT(" " + objectNames[i]);
                atLeastOne = true;
            }
        }

        if (!atLeastOne)
        {
            PRINT(" NOTHING");
        }

        return VerbResult.Idle;
    }

    private VerbResult Look()
    {
        if (noun == "")
        {
            return VerbResult.Proceed;
        }

        Examine();
        return VerbResult.Idle;
    }

    private VerbResult Examine()
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
            FindRoomForObject();

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

    private VerbResult Quit()
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

    private VerbResult Read()
    {
        if (noun == "DIA")
        {
            if ((objectRooms[0] != currentRoom) && (objectRooms[0] != -1))
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
            if ((objectRooms[4] != currentRoom) && (objectRooms[4] != -1))
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
            if ((objectRooms[6] != currentRoom) && (objectRooms[6] != -1))
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

    private VerbResult Open()
    {
        if (noun == "BOX")
        {
            if ((objectRooms[1] != currentRoom) && (objectRooms[1] != -1))
            {
                PRINT("THERE'S NO BOX HERE!");
            }
            else
            {
                objectRooms[6] = currentRoom;
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
                objectRooms[3] = 2;
            }
        }
        else if (noun == "CAS")
        {
            if (currentRoom != 18)
            {
                PRINT("THERE'S NO CASE HERE!");
            }
            else if (wearingGloves != 1)
            {
                PRINT("THE CASE IS ELECTRIFIED!");
            }
            else
            {
                PRINT("THE GLOVES INSULATE AGAINST THE");
                PRINT("ELECTRICITY! THE CASE OPENS!");
                objectRooms[15] = 18;
            }
        }
        else
        {
            PRINT("YOU CAN'T OPEN THAT!");
        }

        return VerbResult.Idle;
    }

    private VerbResult Pour()
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
        if ((objectRooms[6] != currentRoom) && (objectRooms[6] != -1))
        {
            PRINT("YOU DON'T HAVE THE BOTTLE!");
            return false;
        }
        else if (formulaPoured == 1)
        {
            PRINT("THE BOTTLE IS EMPTY!");
            return false;
        }
        else
        {
            formulaPoured = 1;
            return true;
        }
    }

    private bool PourSalt()
    {
        if ((objectRooms[3] != currentRoom) && (objectRooms[3] != -1))
        {
            PRINT("YOU DON'T HAVE THE SALT!");
            return false;
        }
        else if (saltPoured == 1)
        {
            PRINT("THE SHAKER IS EMPTY!");
            return false;
        }
        else
        {
            saltPoured = 1;
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

    private VerbResult Climb()
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
            if ((objectRooms[7] != currentRoom) && (objectRooms[7] != -1))
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
                objectRooms[7] = 0;
            }
        }
        else
        {
            PRINT("IT WON'T DO ANY GOOD.");
        }

        return VerbResult.Idle;
    }

    private VerbResult Jump()
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

    private VerbResult Dig()
    {
        if ((noun != "HOL") && (noun != "GRO") && (noun != ""))
        {
            PRINT("YOU CAN'T DIG THAT!");
        }
        else if ((objectRooms[8] != currentRoom) && (objectRooms[8] != -1))
        {
            PRINT("YOU DON'T HAVE A SHOVEL!");
        }
        else if (currentRoom != 6)
        {
            PRINT("YOU DON'T FIND ANYTHING.");
        }
        else if (objectRooms[10] != 0)
        {
            PRINT("THERE'S NOTHING ELSE THERE!");
        }
        else
        {
            PRINT("THERE'S SOMETHING THERE!");
            objectRooms[10] = 6;
        }

        return VerbResult.Idle;
    }

    private VerbResult Row()
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

    private VerbResult Wave()
    {
        if (noun != "FAN")
        {
            PRINT("YOU CAN'T WAVE THAT!");
        }
        else if ((objectRooms[12] != currentRoom) && (objectRooms[12] != -1))
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
            if (objectRooms[11] == 140)
            {
                objectRooms[11] = 142;
            }
            else
            {
                objectRooms[11] = 140;
            }
        }

        return VerbResult.Idle;
    }

    private VerbResult Leave()
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
            currentRoom = objectRooms[11] - 128;
            return VerbResult.Proceed;
        }
    }

    private VerbResult Fight()
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
        else if (objectRooms[10] != -1)
        {
            PRINT("YOU DON'T HAVE A WEAPON!");
        }
        else
        {
            PRINT("THE GUARD, NOTICING YOUR SWORD,");
            PRINT("WISELY RETREATS INTO THE CASTLE.");
            map[16, 0] = 17;
            objectRooms[13] = 0;
        }

        return VerbResult.Idle;
    }

    private VerbResult Wear()
    {
        if (noun != "GLO")
        {
            PRINT("YOU CAN'T WEAR THAT!");
        }
        else if ((objectRooms[16] != currentRoom) && (objectRooms[16] != -1))
        {
            PRINT("YOU DON'T HAVE THE GLOVES.");
        }
        else
        {
            PRINT("YOU ARE NOW WEARING THE GLOVES.");
            wearingGloves = 1;
        }

        return VerbResult.Idle;
    }
}