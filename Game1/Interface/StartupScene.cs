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
    public class StartupScene : SceneBase
    {
        public StartupScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneName = "Startup";
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        private float Rotation = 0f;
        private float opacity = 1f;
        public override void Draw(GameTime gameTime)
        {
           game.GraphicsDevice.Clear(Color.Black);

           spriteBatch.Begin();
           Vector2 ScreenCenter = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width / 2, game.GraphicsDevice.Viewport.Bounds.Height / 2);
           for (int i = 0; i < 4; i++)
               spriteBatch.DrawString(fonts["symbol"], "•", ScreenCenter, Color.DarkGoldenrod * opacity, Rotation+i, new Vector2(0,0), 1f, SpriteEffects.None, 1f);
           spriteBatch.DrawString(fonts["default"], "Loading", new Vector2(ScreenCenter.X - 27, ScreenCenter.Y + 50), Color.DarkGoldenrod * opacity, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
           spriteBatch.End();

           base.Draw(gameTime);
        }
        private bool[] isTimerCreated = new bool[2];
        private bool fadeNow;
        public override void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();
            
            Timer.Create(.1f, () => Rotation += 1f);
            if (opacity <= 0f) Timer.Create(.5f, () => sceneManager.currentScene = new MainMenuScene(sceneManager));
            if (!isTimerCreated[0])
            {
                Timer.Create(3, () => fadeNow = true);
                isTimerCreated[0] = true;
            }
            if (!isTimerCreated[1] && fadeNow)
            {
                for (int i = 0; i < 25; i++) Timer.Create(i * .1f, () => opacity -= 0.1f);
                isTimerCreated[1] = true;
            }

            base.Update(gameTime);
        }
    }
}
