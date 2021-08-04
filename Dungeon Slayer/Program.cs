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

                if (input.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
                
                level.StartMap();
                level.DrawMap();
               

            }
        }

        static private Player CreatePlayer(Map map)
        {
            //int pointsAvailable = 10;
            int agility = 10;
            int power = 10;
            int luck = 10;

            Player player = new Player(map.GetPlayerStart(), luck, agility, power);

            return player;
        }

    }
}
