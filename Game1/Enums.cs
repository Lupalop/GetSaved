using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        EpicFail
    }
    public enum Games
    {
        FallingObjects,
        EscapeEarthquake,
        EscapeFire,
        RunningForTheirLives,
        HelpOthersNow
    }
    public enum GameEndStates
    {
        TimesUp,
        GameOver,
        GameWon
    }
}
