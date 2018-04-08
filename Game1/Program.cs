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
            {
                Console.Title = "Arkabound Debug Console";
                Console.WriteLine("/*");
                Console.WriteLine(" * Arkabound v" + AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version.ToString());
                Console.WriteLine(" * Debug Console");
                Console.WriteLine(" */");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
            using (var game = new MainGame())
                game.Run();
        }

        public static bool UseConsole = false;
        public static bool VerboseMessages = false;
    }
#endif
}
