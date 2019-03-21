using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maquina.Elements
{
    public abstract class GameElement : GenericElement
    {
        // Constructor
        public GameElement(string name)
            : base(name) {
            Breakable = true;
        }

        // Breakability-related properties
        public bool Breakable { get; set; }
        private int _HitsBeforeBreak;
        public int HitsBeforeBreak
        {
            get
            {
                return _HitsBeforeBreak;
            }
            set
            {
                // We don't accept negative values
                if (value < 0)
                    throw new Exception("Negative value was set for hits before break.");
                else
                    _HitsBeforeBreak = value;
            }
        }
    }

    public enum Speed { SuperSlow, Slow, Normal, Fast, VeryFast };
}
