using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface.Controls;
using Arkabound.Components;

namespace Arkabound.Interface.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager sceneManager)
            : base(sceneManager, "Main Menu")
        {
            mb = new MenuButton("mb", sceneManager)
            { 
                Text = "Play", 
                Graphic = game.Content.Load<Texture2D>("menuBG"), 
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch, 
                Font = fonts["default_m"],
                ClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
            };
            mb2 = new MenuButton("mb", sceneManager)
            {
                Text = "Exit",
                Graphic = game.Content.Load<Texture2D>("menuBG"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Font = fonts["default_m"],
                ClickAction = () => game.Exit()
            };
        }

        private Texture2D Logo;
        private MenuButton mb;
        private MenuButton mb2;

        public override void LoadContent()
        {
            Logo = game.Content.Load<Texture2D>("gameLogo");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Blue);
            spriteBatch.Begin();
            spriteBatch.Draw(Logo, new Vector2(200, 200), null, Color.White, 0.0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
            mb.Draw(gameTime);
            mb2.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            mb.Location = new Vector2(ScreenCenter.X - (mb.Graphic.Width / 2), (int)ScreenCenter.Y - (mb.Graphic.Height / 2));
            mb2.Location = new Vector2(ScreenCenter.X - (mb.Graphic.Width / 2), (int)ScreenCenter.Y - (mb.Graphic.Height / 2) + 70);
            mb.Update(gameTime);
            mb2.Update(gameTime);
        }
    }
}
