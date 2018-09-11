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
using Arkabound.Objects;

namespace Arkabound.Interface.Scenes
{
    public class WorldSelectionScene : SceneBase
    {
        public WorldSelectionScene(SceneManager sceneManager)
            : base(sceneManager, "Game Selection")
        {
            Objects = new ObjectBase[] {
                new MenuButton("mb", sceneManager)
                {
                    Text = "Back",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                },
                new MenuButton("mb2", sceneManager)
                {
                    Text = "Falling Objects",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new GameOneScene(sceneManager)
                },
                new MenuButton("mb3", sceneManager)
                {
                    Text = "Escape - Earthquake",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                },
                new MenuButton("mb3", sceneManager)
                {
                    Text = "Escape - Fire",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                },
                new MenuButton("mb3", sceneManager)
                {
                    Text = "Helix - Fire",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                },
                new MenuButton("mb3", sceneManager)
                {
                    Text = "Heal/Help Others - Earthquake",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }
            };
        }

        private ObjectBase[] Objects;

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Red);
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
