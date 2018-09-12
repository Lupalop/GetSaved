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
            Objects = new Dictionary<string, ObjectBase> {
                { "mb1", new MenuButton("mb", sceneManager)
                {
                    Text = "Back",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
                { "mb2", new MenuButton("mb", sceneManager)
                {
                    Text = String.Format("Difficulty: {0}", difficulty),
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => ModifyDifficulty()
                }},
                { "mb3", new MenuButton("mb", sceneManager)
                {
                    Text = "Falling Objects",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new GameOneScene(sceneManager, difficulty)
                }},
                { "mb4", new MenuButton("mb", sceneManager)
                {
                    Text = "Escape - Earthquake",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new GameTwoScene(sceneManager, difficulty)
                }},
                { "mb5", new MenuButton("mb", sceneManager)
                {
                    Text = "Escape - Fire",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new GameTwoScene(sceneManager, difficulty)
                }},
                { "mb6", new MenuButton("mb", sceneManager)
                {
                    Text = "Helix - Fire",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new GameThreeScene(sceneManager, difficulty)
                }},
                { "mb7", new MenuButton("mb", sceneManager)
                {
                    Text = "Heal/Help Others - Earthquake",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new GameFourScene(sceneManager, difficulty)
                }}
            };

            distanceFromTop = 100;
        }

        private Difficulty difficulty = Difficulty.Easy;

        private void ModifyDifficulty()
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    difficulty = Difficulty.Medium;
                    break;
                case Difficulty.Medium:
                    difficulty = Difficulty.Hard;
                    break;
                case Difficulty.Hard:
                    difficulty = Difficulty.EpicFail;
                    break;
                case Difficulty.EpicFail:
                    difficulty = Difficulty.Easy;
                    break;
            }
            Console.WriteLine(String.Format("Difficulty changed to: {0}", difficulty));
        }
        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MenuButton dfBtn = (MenuButton)Objects["mb2"];
            dfBtn.Text = String.Format("Difficulty: {0}", difficulty);
            base.UpdateObjects(gameTime, Objects);

        }
    }
}
