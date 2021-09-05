using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class UserApplication
    {
        // Properties
        public static string UserName { get; set; }
        public static int Score { get; set; }

        // Methods
        public static void SaveCurrentUser()
        {
            Application.Preferences.SetString("game.username", UserName);
        }
        public static void SetNewHighscore()
        {
            // Iterate over high score board
            int ScoreIndex = -1;
            int NoOfScoresToStore = 10;
            for (int i = 1; i <= NoOfScoresToStore; i++)
            {
                if (Score > Application.Preferences.GetInt32(string.Format("game.highscore.score-{0}", i)))
                {
                    ScoreIndex = i;
                    break;
                }
            }

            if (ScoreIndex > -1)
            {
                for (int i = NoOfScoresToStore - 1; i > ScoreIndex; i--)
                {
                    int userID = i--;
                    Application.Preferences.SetString(
                        string.Format("game.highscore.user-{0}", userID),
                        Application.Preferences.GetString(string.Format("game.highscore.user-{0}", i)));
                    Application.Preferences.SetInt32(
                        string.Format("game.highscore.score-{0}", userID),
                        Application.Preferences.GetInt32(string.Format("game.highscore.score-{0}", i)));
                }

                Application.Preferences.SetString(
                    string.Format("game.highscore.user-{0}", ScoreIndex),
                    UserName);
                Application.Preferences.SetInt32(
                    string.Format("game.highscore.score-{0}", ScoreIndex),
                    Score);
            }
        }
    }
}
