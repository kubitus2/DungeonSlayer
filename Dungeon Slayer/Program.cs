using System;

namespace Dungeon_Slayer
{
    class Program
    {
        //Map dimensions.
        const int MAP_WIDTH = 60;
        const int MAP_HEIGHT = 30;
        
        //Map fill density.
        const int MAP_FILL_DENSITY = 55;
        //Initial XP points to build character stats.
        const int INITIAL_POINTS_AVAILABLE = 10;



        static void Main(string[] args)
        {

            
            Utilities.SetWindowSetting();

            //Main menu controls.
            while(true)
            {
                Utilities.DisplayMainMenu();
                ConsoleKeyInfo option = Console.ReadKey(true);
                switch(option.Key)
                {
                    case ConsoleKey.D1:
                        Play();             //Start game.
                        break;
                    case ConsoleKey.D2:
                        Utilities.HowToPlay();        //Instructions.
                        break;
                    case ConsoleKey.D3:
                        Utilities.Quit();             //Quit game.
                        break;
                    default:
                        continue;
                }
            }          
        }

        //Main gameplay.
        static void Play()
        {
            //Steps counter.
            int numOfSteps = 0;

            //Create new map object.
            Map level = new Map(MAP_WIDTH, MAP_HEIGHT, MAP_FILL_DENSITY);

            bool endGame = true;

            //Create new player.
            Player player = CreatePlayer();

            //Main gameplay loop.
            while (!endGame)
            {
                bool isLevelPassed = false;

                //Initialise and render player and map.
                level.StartMap();               
                level.DrawMap();
                player.SetPosition(level.GetPlayerStart());
                player.RenderPlayer();

                //Portal is cloded for now.
                bool portalOpened = false;

                while (!isLevelPassed)
                {

                    //If no goblins left and portal is not active yet - activate it.
                    if (level.GoblinCount == 0 && !portalOpened)
                    {
                        level.ActivatePortal();
                        portalOpened = true;
                    }
                        
                    ConsoleKeyInfo input = Console.ReadKey(true);

                    //Keeps track of the target position.
                    Vector2DInt target = player.GetPosition();

                    //Display side info: HP, Goblin count, steps number etc.
                    DisplaySideMenu(player, level.GoblinCount, numOfSteps);

                    //Quit with Esc.
                    if (input.Key == ConsoleKey.Escape)
                    {
                        Utilities.Quit();
                        level.DrawMap();
                    }
                    
                    //Controls.
                    switch (input.Key)
                    {
                        case ConsoleKey.D: //move right
                            target += new Vector2DInt(1, 0);
                            break;
                        case ConsoleKey.A: //move left
                            target += new Vector2DInt(-1, 0);
                            break;
                        case ConsoleKey.W: //move up
                            target += new Vector2DInt(0, -1);
                            break;
                        case ConsoleKey.S: //move down
                            target += new Vector2DInt(0, 1);
                            break;
                        case ConsoleKey.Q: //move up-left
                            target += new Vector2DInt(-1, -1);
                            break;
                        case ConsoleKey.E: //move up-right
                            target += new Vector2DInt(1, -1);
                            break;
                        case ConsoleKey.Z: //move down-left
                            target += new Vector2DInt(-1, 1);
                            break;
                        case ConsoleKey.C: //move down-right
                            target += new Vector2DInt(1, 1);
                            break;

                    }

                    //Check if move is permitted.
                    if (level.IsMovePermitted(target))
                    {
                        //Remove player avatar from old position.
                        level.BlankCell(player.GetPosition());

                        //Check if active portal is reached and pass the level.
                        if (level.GetObjType(target) == 4)
                        {
                            isLevelPassed = true;
                        }
                        //Start combat sequence if goblin is encountered.
                        else if (level.GetObjType(target) == 5)
                        {
                            GoblinCombat(ref player);

                            //Clean render after battle.
                            level.RemoveGoblin();
                            level.DrawMap();
                            DisplaySideMenu(player, level.GoblinCount, numOfSteps);
                        }

                        //Update map and player.
                        level.WriteAt(player.GetPosition(), " ");
                        player.SetPosition(target);

                        //place player avatar in the new pos
                        level.WriteAt(target, "@");

                        //Step done and can be counted.
                        numOfSteps++;
                    }

                }
                //LevelUp!
                player.LevelUp();

                //Check if end game condition is met.
                endGame = player.IsLevelMaxed();

                //Update stats after everyt level. New 4 points of XP awarded.
                player.UpdateStats(StatsMenu(4, player));
            }

            //Win sequence.
            Utilities.Win();
        }

        //Side menu display.
        static private void DisplaySideMenu(Player player, int g, int s)
        {
            Console.SetCursorPosition(MAP_WIDTH + 5, 0);
            Console.Write("Number of steps done: {0}", s);
            Console.SetCursorPosition(MAP_WIDTH + 5, 1);
            Console.Write("HP left: {0}", player.GetHP());
            Console.SetCursorPosition(MAP_WIDTH + 5, 2);
            Console.Write("Goblins left: {0}", g);
            Console.SetCursorPosition(MAP_WIDTH + 5, 4);
            Console.Write("{0} is on {1} level.", player.Name, player.GetLevel());
        }

        //Player creation wizard.
        static private Player CreatePlayer()
        {
            //Get name from the player.
            Console.Clear();
            string name = GetName();
            Console.Clear();

            //Initiate player with initial values.
            Player player = new Player(new Vector2DInt (0, 0), name);

            //Let player set his/her/hxx stats.
            Stats stats = StatsMenu(INITIAL_POINTS_AVAILABLE, player);
            player.UpdateStats(stats);

            return player;
        }


        //Get name from the player.
        static string GetName()
        {
            string name;

            Console.Clear();
            Console.WriteLine("What is your name traveller: \n");
            name = Console.ReadLine();


            //Name the player nameless if no name provided. Greet him in the dungeon.
            if(name == "")
            {
                Console.WriteLine("Not a talkative type you are, aren't you? \n Anyway, welcome to the ancient dungeon, Nameless Monster Slayer!");
                Console.ReadLine();
                name = "Nameless";
            }
            else
            {
                Console.WriteLine("Welcome to the ancient dungeon, {0} the Monster Slayer!", name);
                Console.ReadLine();
            }

            return name;
        }

        //Upper combat menu display.
        static private void DisplayCombatMenu(Player player, Goblin goblin)
        {
            
            Console.WriteLine("Your HP is {0}  ", player.GetHP());
            Console.WriteLine("Goblin's HP is {0}   ", goblin.HP);
            Console.WriteLine("_______________________________________");

        }

        //Combat logic.
        static void GoblinCombat(ref Player player)
        {
            int turn = 0;

            Console.Clear();
            Goblin goblin = new Goblin();
            Console.WriteLine("GOBLIN!");
            Console.ReadKey(true);

            //Repeat until goblin is dead.
            while(goblin.HP > 0)
            {
                DisplayCombatMenu(player, goblin);

                //Player and the goblin are attacking each other in turns.
                if (turn % 2 == 0)
                {
                    Console.WriteLine("Your turn. Press any key to attempt attack!\n");
                    Console.ReadKey(true);
                    player.Attack(ref goblin);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Goblin turn");
                    Console.ReadKey(true);

                    goblin.Attack(ref player);
                    Console.Clear();
                }

                //Check if player is still alive.
                if (player.GetHP() <= 0)
                    break;

                //Next turn.
                turn++;
            }
            //Loose if dead.
            if (player.GetHP() <= 0)
                Utilities.Lost();

            //Loot goblin after battle.
            player.LootGoblin();
        }

        //Stats creation menu.
        static Stats StatsMenu(int points, Player player)
        {
            
            //Cache current player stats. Initial 0's cached at the start.
            int a = player.Stats.agility;
            int p = player.Stats.power;
            int l = player.Stats.luck;
            int pts = points;

            bool accepted = false;

            //While player is not happy with the stats, let modify.
            while(!accepted)
            {
                //Display the interface.
                Console.Clear();
                Console.WriteLine("Available XP: {0}\n", pts);

                Console.SetCursorPosition(0, 2);
                Console.Write("AGILITY");
                Console.SetCursorPosition(0, 3);
                Console.Write("[Q]");
                Console.SetCursorPosition(0, 5);
                Console.Write(a);
                Console.SetCursorPosition(0, 7);
                Console.Write("[A]");

                Console.SetCursorPosition(12, 2);
                Console.Write("POWER");
                Console.SetCursorPosition(12, 3);
                Console.Write("[W]");
                Console.SetCursorPosition(12, 5);
                Console.Write(p);
                Console.SetCursorPosition(12, 7);
                Console.Write("[S]");

                Console.SetCursorPosition(22, 2);
                Console.Write("LUCK");
                Console.SetCursorPosition(22, 3);
                Console.Write("[E]");
                Console.SetCursorPosition(22, 5);
                Console.Write(l);
                Console.SetCursorPosition(22, 7);
                Console.Write("[D]");

                Console.SetCursorPosition(0, 9);
                Console.Write("Press [Enter] to approve.");

                ConsoleKeyInfo input = Console.ReadKey(true);

                //Controls.
                switch(input.Key)
                {
                    case ConsoleKey.Enter:
                        accepted = true;
                        break;
                    case ConsoleKey.Q:
                        a = pts > 0 ? a + 1 : a;
                        pts = MathUtils.Clamp(0, points, pts - 1);
                        break;
                    case ConsoleKey.A:
                        a = pts < points ? a - 1 : a;
                        pts = MathUtils.Clamp(0, points, pts + 1);
                        break;
                    case ConsoleKey.W:
                        p = pts > 0 ? p + 1 : p;
                        pts = MathUtils.Clamp(0, points, pts - 1);
                        break;
                    case ConsoleKey.S:
                        p = pts < points ? p - 1 : p;
                        pts = MathUtils.Clamp(0, points, pts + 1);
                        break;
                    case ConsoleKey.E:
                        l = pts > 0 ? l + 1 : l;
                        pts = MathUtils.Clamp(0, points, pts - 1);
                        break;
                    case ConsoleKey.D:
                        p = pts < points ? l - 1 : l;
                        pts = MathUtils.Clamp(0, points, pts + 1);
                        break;
                }

                
            }

            //Create and return new stats values.
            Stats stats = new Stats(l, a, p);

            return stats;
        }








    }
}
