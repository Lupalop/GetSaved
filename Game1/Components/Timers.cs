using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkabound.Components
{
    /*
     * Solution presented here came from Blau, https://stackoverflow.com/users/906404/blau
     * Question at StackOverflow: https://stackoverflow.com/questions/7155347/delay-a-xna-game-with-timers
     */
    public class Timer
    {
        public Action Trigger;
        public float Interval;
        float Elapsed;

        public Timer() { }

        public void Update(float Seconds)
        {
            Elapsed += Seconds;
            if (Elapsed >= Interval)
            {
                Trigger.Invoke();
                Destroy();
            }
        }

        public void Destroy()
        {
            TimerManager.Remove(this);
        }

        public static void Create(float Interval, Action Trigger)
        {
            Timer Timer = new Timer() { Interval = Interval, Trigger = Trigger };
            TimerManager.Add(Timer);
        }
    }

    public class TimerManager : GameComponent
    {
        public static List<Timer> ToRemove = new List<Timer>();
        public static List<Timer> Timers = new List<Timer>();

        public TimerManager(Game game) : base(game) { }

        public static void Add(Timer timer)
        {
            Timers.Add(timer); 
        }
        public static void Remove(Timer timer) 
        { 
            ToRemove.Add(timer); 
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Timer timer in ToRemove) Timers.Remove(timer);
            ToRemove.Clear();
            foreach (Timer timer in Timers) timer.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

        }
    }
}
