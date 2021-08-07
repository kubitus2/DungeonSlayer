using System;

namespace Dungeon_Slayer
{
    class Utilities
    {

        //Title.
        const string TITLE = @"
▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄
██░▄▄▀█░██░█░▄▄▀█░▄▄▄█░▄▄█▀▄▄▀█░▄▄▀████░▄▄▄░█░██░▄▄▀█░██░█░▄▄█░▄▄▀
██░██░█░██░█░██░█░█▄▀█░▄▄█░██░█░██░████▄▄▄▀▀█░██░▀▀░█░▀▀░█░▄▄█░▀▀▄
██░▀▀░██▄▄▄█▄██▄█▄▄▄▄█▄▄▄██▄▄██▄██▄████░▀▀▀░█▄▄█▄██▄█▀▀▀▄█▄▄▄█▄█▄▄  
▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀
                              ";

        //Main menu.
        public static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine("\t\t1. Start Game");
            Console.WriteLine("\t\t2. How to Play");
            Console.WriteLine("\t\t3. Exit");
            Console.WriteLine("\n\nPress corresponding number (NOT numpad) on your keyboard to choose option.");
        }

        //Simple text line toggler.
        public static void UpdatingText(string[] text)
        {
            Console.Clear();

            foreach (string line in text)
            {
                Console.WriteLine(line);
                Console.ReadKey();
            }
        }

        //Quit prompt.
        public static void Quit()
        {
            bool loopControl = false;

            Console.Clear();
            Console.WriteLine("Do you really want to quit? (Y/N)");
            while (!loopControl)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.Y)
                    Environment.Exit(0);
                else if (input.Key == ConsoleKey.N)
                    loopControl = true;
                else continue;
            }

        }

        //Lost sequence.
        public static void Lost()
        {
            Console.Clear();
            Console.WriteLine("You've been killed by a goblin! \nThank you for playing.");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        //Win sequence.
        public static void Win()
        {
            Console.Clear();
            Console.WriteLine("You have cleared all of the rooms!");
            Console.WriteLine("Now we can reclaim our lost gold mines...");
            Console.WriteLine("Thank you for this. But I am afraid that what you are looking for is in another console window...");
            Console.WriteLine("Good luck!");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("Thank you for playing.\n\t\t Kuba");
            Console.ReadKey(true);
        }

        //Console window settings.
        public static void SetWindowSetting()
        {
            Console.Title = "Dungeon Slayer 1.0";
            Console.CursorVisible = false;
        }

        //Is cell inside console buffer bounds.
        public static bool IsInBufferBounds(Vector2DInt pos)
        {
            return (pos.x < Console.BufferWidth - 1) && (pos.y < Console.BufferHeight - 1);
        }
    }
}
