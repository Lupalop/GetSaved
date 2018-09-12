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
    public class MenuButton : ObjectBase
    {
        public MenuButton(string ObjectName, SceneManager sceneManager)
            : base (ObjectName)
        {
            this.sceneManager = sceneManager;
            MsState = sceneManager.MsState;
            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
        }

        private SceneManager sceneManager;
        public Vector2 GraphicCenter;
        public SpriteFont Font { get; set; }
        public string Text { get; set; }
        public MouseState MsState { get; set; }
        public MouseOverlay MsOverlay { get; set; }
        public Action ClickAction { get; set; }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.DrawString(Font, Text, GraphicCenter, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            spriteBatch.End();
        }

        bool clickFired;
        public override void Update(GameTime gameTime)
        {
            Vector2 TextLength = Font.MeasureString(Text);
            GraphicCenter = new Vector2(Location.X + (Graphic.Bounds.Width / 2) - TextLength.X / 2, Location.Y + Graphic.Bounds.Height / 4);

            MsState = sceneManager.MsState;
            if ((MsState.LeftButton == ButtonState.Pressed) && (Bounds.Intersects(MsOverlay.mouseBox)))
                clickFired = true;
            if ((MsState.LeftButton == ButtonState.Pressed) && (!Bounds.Intersects(MsOverlay.mouseBox)))
                clickFired = false;
            if (MsState.LeftButton == ButtonState.Released && clickFired)
            {
                if (ClickAction != null)
                {
                    ClickAction.Invoke();
                    // In order to prevent the action from being fired again
                    clickFired = false;
                }
            }

            base.Update(gameTime);
        }
    }
}
