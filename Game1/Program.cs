using System;
using System.Reflection;

namespace Arkabound
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (UseConsole)
                writeHeader();

            runGame();

            if (UseConsole)
            {
                while (PromptForRestart)
                {
                    Console.WriteLine("Game execution has ended, would you like to restart? Y = Yes, Other keys = No");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        writeHeader();
                        runGame();
                    }
                    else
                        return;
                }
            }
        }

        static void writeHeader()
        {
            Console.Clear();
            Console.Title = "Arkabound Debug Console";
            Console.WriteLine("/*");
            Console.WriteLine(" * Arkabound v" + AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version.ToString());
            Console.WriteLine(" * Debug Console");
            Console.WriteLine(" */");
            Console.WriteLine();
        }

        static void runGame()
        {
            using (var game = new MainGame())
                game.Run();
        }

        public static bool UseConsole = true;
        public static bool VerboseMessages = false;
        public static bool PromptForRestart = false;
    }
#endif
}
