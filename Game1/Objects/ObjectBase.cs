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
            CurrentFrame = 0;
            TotalFrames = 0;
            SpriteType = SpriteTypes.None;
            GraphicEffects = SpriteEffects.None;
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
        public SpriteEffects GraphicEffects { get; set; }

        // For Animated Sprites
        public SpriteTypes SpriteType { get; set; }

        private int rows;
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                TotalFrames = value * Columns;
                rows = value;
            }
        }
        private int columns;
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                TotalFrames = Rows * value;
                columns = value;
            }
        }

        public int CurrentFrame;
        private int TotalFrames;

        // UI (should not be here and should have its own object, i.e. UIObjectBase)
        public bool AlignToCenter { get; set; }

        public Rectangle Bounds { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        // O
        public virtual void Draw(GameTime gameTime)
        {
            if (Graphic != null)
            {
                if (SpriteType != SpriteTypes.None)
                {
                    int width = Graphic.Width / Columns;
                    int height = Graphic.Height / Rows;
                    int row = (int)((float)CurrentFrame / (float)Columns);
                    int column = CurrentFrame % Columns;

                    DestinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, height);
                    SourceRectangle = new Rectangle(width * column, height * row, width, height);
                }
                if (DestinationRectangle != Rectangle.Empty)
                {
                    if (SourceRectangle != Rectangle.Empty)
                        spriteBatch.Draw(Graphic, DestinationRectangle, SourceRectangle, Tint, Rotation, RotationOrigin, GraphicEffects, 1f);
                    else
                        spriteBatch.Draw(Graphic, DestinationRectangle, null, Tint, Rotation, RotationOrigin, GraphicEffects, 1f);
                    return;
                }
                else if (Rotation != 0 || Scale != 0)
                {
                    if (SourceRectangle != Rectangle.Empty)
                        spriteBatch.Draw(Graphic, Location, SourceRectangle, Tint, Rotation, RotationOrigin, Scale, GraphicEffects, 1f);
                    else
                        spriteBatch.Draw(Graphic, Location, null, Tint, Rotation, RotationOrigin, Scale, GraphicEffects, 1f);
                    return;
                }
                else
                {
                    spriteBatch.Draw(Graphic, Bounds, Tint);
                }
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (SpriteType != SpriteTypes.None)
            {
                if (SpriteType == SpriteTypes.Animated)
                    CurrentFrame++;
                if (CurrentFrame == TotalFrames)
                    CurrentFrame = 0;
            }

            Point dimens = new Point(0, 0);
            if (UseCustomDimensions)
            {
                dimens = Dimensions;
            }
            else if (Graphic != null)
            {
                if (SourceRectangle != Rectangle.Empty)
                    dimens = new Point(SourceRectangle.Width, SourceRectangle.Height);
                else
                    dimens = new Point(Graphic.Width, Graphic.Height);
            }
            else
            {
                dimens = DimensionsOverride;
            }

            Bounds = new Rectangle(Location.ToPoint(), dimens);
        }
    }
    public enum SpriteTypes { Static, Animated, None };
}
