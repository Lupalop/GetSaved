using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkabound.Interface
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneName = "Main Menu";
        }

        private Texture2D Logo;

        public override void LoadContent()
        {
            Logo = game.Content.Load<Texture2D>("gameLogo");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(Logo, new Vector2(200, 200), null, Color.White, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
