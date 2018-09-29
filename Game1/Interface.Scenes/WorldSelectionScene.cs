using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Interface;
using Maquina.Interface.Controls;
using Maquina.Objects;

namespace Maquina.Interface.Scenes
{
    public class WorldSelectionScene : SceneBase
    {
        public WorldSelectionScene(SceneManager sceneManager)
            : base(sceneManager, "Game Selection")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, ObjectBase> {
                { "mb1", new MenuButton("mb", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
                { "mb2", new MenuButton("mb", sceneManager)
                {
                    Text = String.Format("Difficulty: {0}", difficulty),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => {
                        MenuButton dfBtn = (MenuButton)Objects["mb2"];
                        dfBtn.Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5);
                        dfBtn.Text = String.Format("Difficulty: {0}", difficulty);
                    },
                    LeftClickAction = () => ModifyDifficulty()
                }},
                { "mb3", new MenuButton("mb", sceneManager)
                {
                    Text = "The Safety Kit",
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.FallingObjects, difficulty)
                }},
                { "mb4", new MenuButton("mb", sceneManager)
                {
                    Text = "Earthquake Escape",
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.EscapeEarthquake, difficulty)
                }},
                { "mb5", new MenuButton("mb", sceneManager)
                {
                    Text = "Fire Escape",
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.EscapeFire, difficulty)
                }},
                { "mb6", new MenuButton("mb", sceneManager)
                {
                    Text = "Safety Jump - Fire",
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.RunningForTheirLives, difficulty)
                }},
                { "mb7", new MenuButton("mb", sceneManager)
                {
                    Text = "Aid 'Em - Earthquake",
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager, Games.HelpOthersNow, difficulty)
                }},
                { "mb8", new MenuButton("mb", sceneManager)
                {
                    spriteBatch = this.spriteBatch,
                    Tint = Color.Transparent
                }},
                { "mb9", new MenuButton("mb", sceneManager)
                {
                    Text = "Random Game",
                    spriteBatch = this.spriteBatch,
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
            Console.WriteLine(String.Format("GameDifficulty changed to: {0}", difficulty));
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
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
