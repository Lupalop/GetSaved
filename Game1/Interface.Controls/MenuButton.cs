﻿using System;
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
        public Action LeftClickAction { get; set; }
        public Action RightClickAction { get; set; }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.DrawString(Font, Text, GraphicCenter, Color.White, 0f, new Vector2(0, 0), Scale, SpriteEffects.None, 1f);
            spriteBatch.End();
        }

        bool LeftClickFired;
        bool RightClickFired;
        public override void Update(GameTime gameTime)
        {
            // TODO: Support touch events (I don't have a real touch device unfortunately)
            Vector2 TextLength = Font.MeasureString(Text);
            GraphicCenter = new Vector2(Location.X + (Bounds.Width / 2) - TextLength.X / 2, Location.Y + Bounds.Height / 4);
            MsState = sceneManager.MsState;

            // Revert to white tint
            Tint = Color.White;

            // If mouse is on top of the button
            if (Bounds.Intersects(MsOverlay.mouseBox))
            {
                Tint = Color.Wheat;
            }

            // If the button was clicked
            if ((MsState.LeftButton == ButtonState.Pressed ||
                 MsState.RightButton == ButtonState.Pressed ||
                 MsState.MiddleButton == ButtonState.Pressed) && Bounds.Intersects(MsOverlay.mouseBox))
            {
                Tint = Color.Violet;
            }

            // Left Mouse Button Click Action
            if (LeftClickAction != null)
            {
                if (MsState.LeftButton == ButtonState.Pressed && Bounds.Intersects(MsOverlay.mouseBox))
                    LeftClickFired = true;
                if (MsState.LeftButton == ButtonState.Pressed && !Bounds.Intersects(MsOverlay.mouseBox))
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
                if (MsState.RightButton == ButtonState.Pressed && Bounds.Intersects(MsOverlay.mouseBox))
                    RightClickFired = true;
                if (MsState.RightButton == ButtonState.Pressed && !Bounds.Intersects(MsOverlay.mouseBox))
                    RightClickFired = false;
                if (MsState.RightButton == ButtonState.Released && RightClickFired)
                {
                    RightClickAction.Invoke();
                    // In order to prevent the action from being fired again
                    RightClickFired = false;
                }
            }

            base.Update(gameTime);
        }
    }
}
