using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class UserGlobal
    {
        // Properties
        public static string UserName { get; set; }
        public static int Score { get; set; }
        public static PreferencesManager PreferencesManager { get; set; }

        // Methods
        public static void SaveCurrentUser()
        {
            PreferencesManager.SetCharPref("game.username", UserName);
        }
        public static void SetNewHighscore()
        {
            // Iterate over high score board
            int ScoreIndex = -1;
            int NoOfScoresToStore = 10;
            for (int i = 1; i <= NoOfScoresToStore; i++)
            {
                if (Score > PreferencesManager.GetIntPref(String.Format("game.highscore.score-{0}", i)))
                {
                    ScoreIndex = i;
                    break;
                }
            }

            if (ScoreIndex > -1)
            {
                for (int i = NoOfScoresToStore - 1; i > ScoreIndex; i--)
                {
                    PreferencesManager.SetCharPref(
                        String.Format("game.highscore.user-{0}", i--),
                        PreferencesManager.GetCharPref(String.Format("game.highscore.user-{0}", i)));
                    PreferencesManager.SetIntPref(
                        String.Format("game.highscore.score-{0}", i--),
                        PreferencesManager.GetIntPref(String.Format("game.highscore.score-{0}", i)));
                }

                PreferencesManager.SetCharPref(
                    String.Format("game.highscore.user-{0}", ScoreIndex),
                    UserName);
                PreferencesManager.SetIntPref(
                    String.Format("game.highscore.score-{0}", ScoreIndex),
                    Score);
            }
        }
    }
}
