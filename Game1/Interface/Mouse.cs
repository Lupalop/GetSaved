using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Components;

namespace Arkabound.Interface
{
    public class MouseScene : SceneBase
    {
        public MouseScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneName = "Mouse Overlay";
        }

        private Texture2D cursor;
        private Vector2 mousePos;
        private MouseState msState;
        private Color mouseTint = Color.White;
        private bool isTimerFired;

        public override void LoadContent()
        {
            cursor = game.Content.Load<Texture2D>("mouseCur");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(cursor, mousePos, mouseTint);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Get mouse state on every update
            msState = Mouse.GetState();
            // Make cursor follow mouse position
            mousePos = msState.Position.ToVector2();

            // Selected effect - when left button is pressed, cursor turns to green
            if ((msState.LeftButton == ButtonState.Pressed || msState.RightButton == ButtonState.Pressed) && !isTimerFired)
            {
                Timer.Create(.1f, () => mouseTint = Color.Green);
                Timer.Create(.3f, () => mouseTint = Color.White);
                Timer.Create(.3f, () => isTimerFired = false);
                isTimerFired = true;
            }

            base.Update(gameTime);
        }
    }
}
