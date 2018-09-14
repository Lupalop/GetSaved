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
    public class Label : ObjectBase
    {
        public Label(string ObjectName)
            : base (ObjectName)
        {
        }

        public Vector2 GraphicCenter;
        public SpriteFont Font { get; set; }
        public string Text { get; set; }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, Text, GraphicCenter, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            base.Draw(gameTime);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            GraphicCenter = new Vector2(Location.X, Location.Y);
            DimensionsOverride = Font.MeasureString(Text).ToPoint();
            base.Update(gameTime);
        }
    }
}
