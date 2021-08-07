using System;

namespace Dungeon_Slayer
{
    class Program
    {
        const int MAP_WIDTH = 60;
        const int MAP_HEIGHT = 30;
        const int MAP_FILL_DENSITY = 55;

        static void Main(string[] args)
        {

            Console.CursorVisible = false;
            Play();            

        }

        static void Play()
        {
            bool isLevelPassed = false;

            Map level = new Map(MAP_WIDTH, MAP_HEIGHT, MAP_FILL_DENSITY);
            level.StartMap();
            Player player = CreatePlayer(level);
            level.DrawMap();
            player.RenderPlayer();

            while (!isLevelPassed)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);

                Vector2DInt target = player.GetPosition();
                

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
                    if (level.GetObjType(target) == 3)
                    {
                        isLevelPassed = true;

                    }


                    //update map and player
                    level.WriteAt(player.GetPosition(), " ");
                    player.SetPosition(target);

                    //place player avatar in the new pos
                    level.WriteAt(target, "@");
                }

            }
        }

        static private Player CreatePlayer(Map map)
        {
            int pointsAvailable = 10;
            int agility = 0;
            int power = 0;
            int luck = 0;
            string name = string.Empty;

            Console.Clear();
            name = GetName();
            Stats stats = StatsMenu(10);

            Console.Clear();


            Player player = new Player(map.GetPlayerStart(), stats, name);

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
            }
            else
            {
                Console.WriteLine("Welcome to the ancient dungeon, {0} the Monster Slayer!", name);
                Console.ReadLine();
            }

            return name;
        }

        static Stats StatsMenu(int points)
        {
            
            int a = 0;
            int p = 0;
            int l = 0;
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

        static int Clamp(int min, int max, int value)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            else
                return value;
        }

        static void Increment(Vector2DInt sub, int a)
        {
            Vector2DInt av = new Vector2DInt(a, a);
            sub += av;

            Console.WriteLine(sub.x);
        }
    }
}
