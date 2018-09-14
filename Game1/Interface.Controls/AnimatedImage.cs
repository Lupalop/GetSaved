using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Objects;
using Arkabound.Interface.Scenes;
using Arkabound.Components;

namespace Arkabound.Interface.Controls
{
    // Borrowed from http://rbwhitaker.wikidot.com/monogame-texture-atlases-1
    public class AnimatedImage : ObjectBase
    {
        public AnimatedImage(string ObjectName)
            : base(ObjectName)
        {
            CurrentFrame = 0;
            TotalFrames = 0;
        }

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

        public override void Draw(GameTime gameTime)
        {
            int width = Graphic.Width / Columns;
            int height = Graphic.Height / Rows;
            int row = (int)((float)CurrentFrame / (float)Columns);
            int column = CurrentFrame % Columns;

            DestinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, height);
            SourceRectangle = new Rectangle(width * column, height * row, width, height);

            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
            
        }

        public override void Update(GameTime gameTime)
        {
            if (SpriteType == SpriteTypes.Animated)
                CurrentFrame++;
            if (CurrentFrame == TotalFrames)
                CurrentFrame = 0;

            base.Update(gameTime);
        }
    }

    public enum SpriteTypes { Static, Animated };
}
