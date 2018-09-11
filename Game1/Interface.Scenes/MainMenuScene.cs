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
using Arkabound.Objects;

namespace Arkabound.Interface.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager sceneManager)
            : base(sceneManager, "Main Menu")
        {
            Objects = new ObjectBase[] {
                new Image("logo") {
                    Graphic = game.Content.Load<Texture2D>("gameLogo"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch
                },
                new MenuButton("mb", sceneManager) {
                    Text = "Play", 
                    Graphic = game.Content.Load<Texture2D>("menuBG"), 
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"],
                    ClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                },
                new MenuButton("mb2", sceneManager) {
                    Text = "Help", 
                    Graphic = game.Content.Load<Texture2D>("menuBG"), 
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"],
                    ClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                },
                new MenuButton("mb4", sceneManager) {
                    Text = "Settings", 
                    Graphic = game.Content.Load<Texture2D>("menuBG"), 
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"],
                    ClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                },
                new MenuButton("mb5", sceneManager) {
                    Text = "High Scores", 
                    Graphic = game.Content.Load<Texture2D>("menuBG"), 
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"],
                    ClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                },
                new MenuButton("mb3", sceneManager) {
                    Text = "Exit",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_m"],
                    ClickAction = () => game.Exit()
                }
            };
        }

        private ObjectBase[] Objects;

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Blue);
            base.Draw(gameTime);
            base.DrawObjectsFromBase(gameTime, Objects);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.AlignObjectsToCenterUsingBase(gameTime, Objects);
        }
    }
}
