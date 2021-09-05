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
        EpicFail,
        Random,
        Demo
    }
    public enum Games
    {
        FallingObjects,
        EscapeEarthquake,
        EscapeFire,
        RunningForTheirLives,
        HelpOthersNow,
        Random
    }
    public enum GameEndStates
    {
        TimesUp,
        GameOver,
        GameWon
    }
}
