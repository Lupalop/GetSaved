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
            Platform.RunGame = RunGame;
            Platform.StartEngine(args);
        }

        private static void RunGame()
        {
            using (var Game = new MainGame())
                Game.Run();
        }
    }
#endif
}
