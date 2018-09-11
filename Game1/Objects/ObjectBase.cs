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
            Tint = Color.White;
            Scale = 1f;
            AlignToCenter = true;
            MessageHolder = new List<object>();
        }

        // Basic Properties
        public string Name { get; set; }
        public bool EditorVisibility { get; set; }
        public bool CampaignOnlyObject { get; set; }
        public AnimationTypes AnimationType { get; set; }
        public List<object> MessageHolder { get; set; }

        // Graphics
        public virtual Texture2D Graphic { get; set; }
        public Vector2 Location { get; set; }
        public Point Dimensions { get; set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public bool UseCustomDimensions { get; set; }
        public bool AlignToCenter { get; set; }

        public Rectangle Bounds { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        
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
        public virtual void Remove() { Console.Write("Unimplemented"); }
        public virtual void Draw(GameTime gameTime)
        {
            if (Graphic != null)
            {
                if (Rotation != 0 || Scale != 0)
                    spriteBatch.Draw(Graphic, Location, null, Tint, Rotation, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
                else
                    spriteBatch.Draw(Graphic, Bounds, Tint);
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (Graphic != null)
            {
                Point dimens = new Point(0, 0);
                if (UseCustomDimensions)
                    dimens = Dimensions;
                else if (Graphic != null)
                    dimens = new Point(Graphic.Width, Graphic.Height);

                Bounds = new Rectangle(Location.ToPoint(), dimens);
            }
        }
    }

    public enum AnimationTypes { Static, Animated };
    public enum Speed { SuperSlow, Slow, Normal, Fast, VeryFast };
}
