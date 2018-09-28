using System;
using System.Reflection;

namespace Maquina
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
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "--v":
                            VerboseMessages = true;
                            break;
                        case "--restartOnExit":
                            PromptForRestart = true;
                            break;
                        default:
                            // Ignore other arguments passed
                            break;
                    }
                }
            }

            WriteHeader();
            RunGame();

            while (PromptForRestart)
            {
                Console.WriteLine("Game execution has ended, would you like to restart? Y = Yes, Other keys = No");
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    WriteHeader();
                    RunGame();
                }
                else
                    return;
            }
        }

        private static void WriteHeader()
        {
            Console.Clear();
            Console.Title = String.Format("{0} Debug Console", GameName);
            Console.WriteLine("/*");
            Console.WriteLine(" * {0} v{1}", GameName, AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version.ToString());
            Console.WriteLine(" * Debug Console");
            Console.WriteLine(" */");
            Console.WriteLine();
        }

        private static void RunGame()
        {
            using (var game = new MainGame())
                game.Run();
        }

        public static bool OutputMessages = true;
        public static bool VerboseMessages = false;
        public static bool PromptForRestart = false;
        public static string GameName = "Get Saved";
    }
#endif
}
