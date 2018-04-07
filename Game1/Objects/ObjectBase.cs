using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arkabound.Objects
{
    /// <summary>
    /// The Base class, where every game object inherits its properties
    /// </summary>
    public abstract class ObjectBase
    {
        // Constructor
        public ObjectBase(string name) {
            Name = name;
            EditorVisibility = true;
            CampaignOnlyObject = false;
            Breakable = true;
            AnimationType = AnimationTypes.Static;
        }

        // Basic Properties
        public string Name { get; set; }
        public bool EditorVisibility { get; set; }
        public bool CampaignOnlyObject { get; set; }
        public AnimationTypes AnimationType { get; set; }

        // Graphics
        public Texture2D Graphic { get; set; }
        public Point Location { get; set; }
        public Point Dimensions { get; set; }
        public Color Tint { get; set; }
        public bool UseCustomDimensions { get; set; }
        
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

        // O
        public void Remove() { Console.Write("Unimplemented"); }
        public void Draw(SpriteBatch spritebatch) {
            spritebatch.Begin();
            if (UseCustomDimensions)
                spritebatch.Draw(Graphic, new Rectangle(Location, Dimensions), Tint);
            else
                spritebatch.Draw(Graphic, Location.ToVector2(), Tint);
            spritebatch.End();
        }
    }

    public enum AnimationTypes { Static, Animated };
    public enum Speed { SuperSlow, Slow, Normal, Fast, VeryFast };
}
