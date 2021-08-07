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
            Console.WriteLine("\t1. Start Game");
            Console.WriteLine("\t2. How to Play");
            Console.WriteLine("\t3. Exit");

        }
        public static void HowToPlay()
        {
            string[] text = 
            {
                "You explore immense caverns of Uelitschka gold mines.",
                "These were abandoned long ago and all full of blood-thirsty goblins.",
                "Your goal is to clean the rooms of all enemies for the royals of Caracov to seize the gold hidden within depths of the mines.",
                "By cleaning each room you are granted XP. You can spend it on three perks: Agility, Luck and Power.",
                "\tPower makes you stronger and more deadly to your oponents.",
                "\tAgility makes you more resistant and durable.",
                "\tLuck give you better chance of hitting your target or missing there attack. It also raises your chance of looting healing potion from defeated goblin.",
                "You win after clearing all the rooms.",
                "Godpede, brave warrior!"
            };
            Console.Clear();
            
            foreach(string line in text)
            {
                Console.WriteLine(line);
                Console.ReadKey(true);
            }
        }

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

        public static void SetWindowSetting()
        {
            Console.Title = "Dungeon Slayer 1.0";
            Console.CursorVisible = false;
            
        }
    }
}
