using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface;
using Arkabound.Interface.Controls;

namespace Arkabound.Interface.Scenes
{
    public class WorldSelectionScene : SceneBase
    {
        public WorldSelectionScene(SceneManager sceneManager)
            : base(sceneManager, "World Selection")
        {
            mb = new MenuButton("mb", sceneManager)
            {
                Text = "Back",
                Graphic = game.Content.Load<Texture2D>("menuBG"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Font = fonts["default"],
                ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
            };
        }
        MenuButton mb;
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Red);
            mb.Draw(gameTime);
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            mb.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
