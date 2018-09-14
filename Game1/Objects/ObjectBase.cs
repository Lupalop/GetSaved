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
            Tint = Color.White;
            Scale = 1f;
            AlignToCenter = true;
            MessageHolder = new List<object>();
        }

        // Basic Properties
        public string Name { get; set; }
        public bool EditorVisibility { get; set; }
        public bool CampaignOnlyObject { get; set; }
        public List<object> MessageHolder { get; set; }

        // Graphics
        public Texture2D Graphic { get; set; }
        public Vector2 Location { get; set; }
        public Point Dimensions { get; set; }
        public Point DimensionsOverride { get; set; }
        public Color Tint { get; set; }
        public float Rotation { get; set; }
        public Vector2 RotationOrigin { get; set; }
        public float Scale { get; set; }
        public bool UseCustomDimensions { get; set; }
        public Rectangle DestinationRectangle { get; set; }
        public Rectangle SourceRectangle { get; set; }

        // UI (should not be here and should have its own object, i.e. UIObjectBase)
        public bool AlignToCenter { get; set; }

        public Rectangle Bounds { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        // O
        public virtual void Draw(GameTime gameTime)
        {
            if (Graphic != null)
            {
                if (DestinationRectangle != Rectangle.Empty)
                {
                    if (SourceRectangle != Rectangle.Empty)
                        spriteBatch.Draw(Graphic, DestinationRectangle, SourceRectangle, Tint, Rotation, RotationOrigin, SpriteEffects.None, 1f);
                    else
                        spriteBatch.Draw(Graphic, DestinationRectangle, null, Tint, Rotation, RotationOrigin, SpriteEffects.None, 1f);
                }
                else if (Rotation != 0 || Scale != 0)
                {
                    if (SourceRectangle != Rectangle.Empty)
                        spriteBatch.Draw(Graphic, Location, SourceRectangle, Tint, Rotation, RotationOrigin, Scale, SpriteEffects.None, 1f);
                    else
                        spriteBatch.Draw(Graphic, Location, null, Tint, Rotation, RotationOrigin, Scale, SpriteEffects.None, 1f);
                }
                else
                {
                    spriteBatch.Draw(Graphic, Bounds, Tint);
                }
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            Point dimens = new Point(0, 0);
            if (UseCustomDimensions)
                dimens = Dimensions;
            else if (Graphic != null)
                dimens = new Point(Graphic.Width, Graphic.Height);
            else
                dimens = DimensionsOverride;

            Bounds = new Rectangle(Location.ToPoint(), dimens);
        }
    }
}
