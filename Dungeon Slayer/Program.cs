using System;

namespace Dungeon_Slayer
{
    class Program
    {
        const int MAP_WIDTH = 60;
        const int MAP_HEIGHT = 30;
        const int MAP_FILL_DENSITY = 55;
        const int INITIAL_POINTS_AVAILABLE = 10;

        static void Main(string[] args)
        {

            Console.CursorVisible = false;
            Play();            

        }

        static void Play()
        {
            int numOfSteps = 0;
            Map level = new Map(MAP_WIDTH, MAP_HEIGHT, MAP_FILL_DENSITY);
            bool endGame = false;

            Player player = CreatePlayer();

            while (!endGame)
            {
                bool isLevelPassed = false;

                level.StartMap();               
                level.DrawMap();
                player.SetPosition(level.GetPlayerStart());
                player.RenderPlayer();
                bool portalOpened = false;

                while (!isLevelPassed)
                {
                    
                    if (level.GoblinCount == 0 && !portalOpened)
                    {
                        level.ActivatePortal();
                        portalOpened = true;
                    }
                        
                    ConsoleKeyInfo input = Console.ReadKey(true);

                    Vector2DInt target = player.GetPosition();

                    DisplaySideMenu(player, level.GoblinCount, numOfSteps);

                    if (input.Key == ConsoleKey.Escape)
                        Environment.Exit(0);


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
                    }

                    if (level.IsMovePermitted(target))
                    {
                        //remove player avatar from old pos
                        level.BlankCell(player.GetPosition());

                        //check if portal is reached
                        if (level.GetObjType(target) == 4)
                        {
                            isLevelPassed = true;

                        }
                        else if (level.GetObjType(target) == 5)
                        {
                            GoblinCombat(ref player);
                            if (player.GetHP() <= 0)
                            {
                                Lost();
                            }
                            level.RemoveGoblin();
                            level.DrawMap();
                        }

                    


                        //update map and player
                        level.WriteAt(player.GetPosition(), " ");
                        player.SetPosition(target);

                        //place player avatar in the new pos
                        level.WriteAt(target, "@");

                        numOfSteps++;
                    }

                }
                player.UpdateStats(StatsMenu(10, player));

                //

                player.LevelUp();
                endGame = player.IsLevelMaxed();
            }

            
        }
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

        static private Player CreatePlayer()
        {
            int pointsAvailable = 10;
            int agility = 0;
            int power = 0;
            int luck = 0;
            string name = string.Empty;

            Console.Clear();
            name = GetName();
            

            Console.Clear();


            Player player = new Player(new Vector2DInt (0, 0), name);
            Stats stats = StatsMenu(INITIAL_POINTS_AVAILABLE, player);
            player.UpdateStats(stats);

            return player;
        }

        static string GetName()
        {
            string name;

            Console.Clear();
            Console.WriteLine("What is your name traveller: \n");
            name = Console.ReadLine();

            if(name == "")
            {
                Console.WriteLine("Not a talkative type you are, aren't you? \n");
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

        static private void DisplayCombatMenu(Player player, Goblin goblin)
        {
            Console.SetCursorPosition(47, 0);
            Console.WriteLine("Your HP is {0}  ", player.GetHP());
            Console.SetCursorPosition(47, 1);
            Console.WriteLine("Goblin's HP is {0}   ", goblin.HP);

        }
        static void GoblinCombat(ref Player player)
        {
            int turn = 0;

            Console.Clear();
            Goblin goblin = new Goblin();
            Console.WriteLine("GOBLIN!");
            Console.ReadKey(true);

            while(goblin.HP > 0)
            {
                DisplayCombatMenu(player, goblin);

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

                if (player.GetHP() <= 0)
                    break;

                turn++;
            }

            player.LootGoblin();
        }

        static Stats StatsMenu(int points, Player player)
        {
            
            int a = player.Stats.agility;
            int p = player.Stats.power;
            int l = player.Stats.luck;
            int pts = points;
            bool accepted = false;

            while(!accepted)
            {
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

                switch(input.Key)
                {
                    case ConsoleKey.Enter:
                        accepted = true;
                        break;
                    case ConsoleKey.Q:
                        a = pts > 0 ? a + 1 : a;
                        pts = Clamp(0, points, pts - 1);
                        break;
                    case ConsoleKey.A:
                        a = pts < points ? a - 1 : a;
                        pts = Clamp(0, points, pts + 1);
                        break;
                    case ConsoleKey.W:
                        p = pts > 0 ? p + 1 : p;
                        pts = Clamp(0, points, pts - 1);
                        break;
                    case ConsoleKey.S:
                        p = pts < points ? p - 1 : p;
                        pts = Clamp(0, points, pts + 1);
                        break;
                    case ConsoleKey.E:
                        l = pts > 0 ? l + 1 : l;
                        pts = Clamp(0, points, pts - 1);
                        break;
                    case ConsoleKey.D:
                        p = pts < points ? l - 1 : l;
                        pts = Clamp(0, points, pts + 1);
                        break;
                }

                
            }
            Stats stats = new Stats(l, a, p);

            return stats;
        }

        static void Lost()
        {
            Console.Clear();
            Console.WriteLine("You've been killed by a goblin! \nThank you for playing.");
            Console.ReadKey(true);
            Environment.Exit(0);


        }

        static int Clamp(int min, int max, int value)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }
    }
}
