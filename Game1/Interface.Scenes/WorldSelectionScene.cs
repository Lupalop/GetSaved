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
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
                { "mb2", new MenuButton("mb", sceneManager)
                {
                    Text = String.Format("Difficulty: {0}", difficulty),
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => ModifyDifficulty()
                }},
                { "mb3", new MenuButton("mb", sceneManager)
                {
                    Text = "Falling Objects",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.FallingObjects, difficulty)
                }},
                { "mb4", new MenuButton("mb", sceneManager)
                {
                    Text = "Escape - Earthquake",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.EscapeEarthquake, difficulty)
                }},
                { "mb5", new MenuButton("mb", sceneManager)
                {
                    Text = "Escape - Fire",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.EscapeFire, difficulty)
                }},
                { "mb6", new MenuButton("mb", sceneManager)
                {
                    Text = "Dino-Like - Fire",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.RunningForTheirLives, difficulty)
                }},
                { "mb7", new MenuButton("mb", sceneManager)
                {
                    Text = "Aid 'Em - Earthquake",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.HelpOthersNow, difficulty)
                }},
                { "mb8", new MenuButton("mb", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Tint = Color.Transparent,
                    Font = fonts["default"]
                }},
                { "mb9", new MenuButton("mb", sceneManager)
                {
                    Text = "Random Game",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager)
                }},
            };
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
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MenuButton dfBtn = (MenuButton)Objects["mb2"];
            dfBtn.Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5);
            dfBtn.Text = String.Format("Difficulty: {0}", difficulty);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
