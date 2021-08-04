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
            bool quit = false;
            while(!quit)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                quit = input.Key == ConsoleKey.Escape;
                //
                Map level = new Map(MAP_WIDTH, MAP_HEIGHT, MAP_FILL_DENSITY);
                level.StartMap();
                level.DrawMap();

                Player player = CreatePlayer(level);

                player.RenderPlayer();
            }
            

        }

        static private Player CreatePlayer(Map map)
        {
            Player player = new Player(map.GetPlayerStart(), 10, 10, 10);

            return player;
        }

    }
}
