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
    public class MouseOverlay : SceneBase
    {
        public MouseOverlay(SceneManager sceneManager)
            : base(sceneManager, "Mouse Overlay")
        {
        }

        private Texture2D cursor;
        private Vector2 mousePos;
        private Color mouseTint = Color.White;
        private bool isTimerFired;
        public Rectangle mouseBox;

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
            // Make cursor follow mouse position
            mousePos = MsState.Position.ToVector2();

            mouseBox = new Rectangle(mousePos.ToPoint(), new Point(cursor.Width, cursor.Height));

            // Selected effect - when left button is pressed, cursor turns to green
            if ((MsState.LeftButton == ButtonState.Pressed || MsState.RightButton == ButtonState.Pressed) && !isTimerFired)
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
