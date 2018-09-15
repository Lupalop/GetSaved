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
        public Action LeftClickAction { get; set; }
        public Action RightClickAction { get; set; }
        public bool Disabled { get; set; }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (Text != null)
                spriteBatch.DrawString(Font, Text, GraphicCenter, Tint, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
        }

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update(GameTime gameTime)
        {
            // TODO: Support touch events (I don't have a real touch device unfortunately)
            if (Text != null)
            {
                Vector2 TextLength = Font.MeasureString(Text);
                GraphicCenter = new Vector2(Location.X + (Bounds.Width / 2) - TextLength.X / 2, Location.Y + Bounds.Height / 4);
            }
            MsState = sceneManager.MsState;
            CurrentFrame = 0;

            // Don't respond to any event if button is disabled
            if (!Disabled)
            {
                // If mouse is on top of the button
                if (Bounds.Contains(MsOverlay.Bounds.Location) && SpriteType != SpriteTypes.None)
                {
                    CurrentFrame = 1;
                }

                // If the button was clicked
                if ((MsState.LeftButton == ButtonState.Pressed ||
                     MsState.RightButton == ButtonState.Pressed ||
                     MsState.MiddleButton == ButtonState.Pressed) && Bounds.Contains(MsOverlay.Bounds.Location) && SpriteType != SpriteTypes.None)
                {
                    CurrentFrame = 2;
                }

                // Left Mouse Button Click Action
                if (LeftClickAction != null)
                {
                    if (MsState.LeftButton == ButtonState.Pressed && Bounds.Contains(MsOverlay.Bounds.Location))
                        LeftClickFired = true;
                    if (MsState.LeftButton == ButtonState.Pressed && !Bounds.Contains(MsOverlay.Bounds.Location))
                        LeftClickFired = false;
                    if (MsState.LeftButton == ButtonState.Released && LeftClickFired)
                    {
                        LeftClickAction.Invoke();
                        // In order to prevent the action from being fired again
                        LeftClickFired = false;
                    }
                }

                // Right Mouse Button Click Action
                if (RightClickAction != null)
                {
                    if (MsState.RightButton == ButtonState.Pressed && Bounds.Contains(MsOverlay.Bounds.Location))
                        RightClickFired = true;
                    if (MsState.RightButton == ButtonState.Pressed && !Bounds.Contains(MsOverlay.Bounds.Location))
                        RightClickFired = false;
                    if (MsState.RightButton == ButtonState.Released && RightClickFired)
                    {
                        RightClickAction.Invoke();
                        // In order to prevent the action from being fired again
                        RightClickFired = false;
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
