using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.UI.Controls;
using Maquina.Objects;

namespace Maquina.UI.Scenes
{
    public class WorldSelectionScene : SceneBase
    {
        public WorldSelectionScene(SceneManager SceneManager)
            : base(SceneManager, "Game Selection")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "mb1", new MenuButton("mb", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},
                { "mb2", new MenuButton("mb", SceneManager)
                {
                    Text = String.Format("Difficulty: {0}", difficulty),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        MenuButton dfBtn = (MenuButton)Objects["mb2"];
                        dfBtn.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - 305, 5);
                        dfBtn.Text = String.Format("Difficulty: {0}", difficulty);
                    },
                    LeftClickAction = () => ModifyDifficulty()
                }},
                { "mb3", new MenuButton("mb", SceneManager)
                {
                    Text = "The Safety Kit",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager, Games.FallingObjects, difficulty))
                }},
                { "mb4", new MenuButton("mb", SceneManager)
                {
                    Text = "Earthquake Escape",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager, Games.EscapeEarthquake, difficulty))
                }},
                { "mb5", new MenuButton("mb", SceneManager)
                {
                    Text = "Fire Escape",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager, Games.EscapeFire, difficulty))
                }},
                { "mb6", new MenuButton("mb", SceneManager)
                {
                    Text = "Safety Jump - Fire",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager, Games.RunningForTheirLives, difficulty))
                }},
                { "mb7", new MenuButton("mb", SceneManager)
                {
                    Text = "Aid 'Em - Earthquake",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager, Games.HelpOthersNow, difficulty))
                }},
                { "mb8", new MenuButton("mb", SceneManager)
                {
                    SpriteBatch = this.SpriteBatch,
                    Tint = Color.Transparent
                }},
                { "mb9", new MenuButton("mb", SceneManager)
                {
                    Text = "Random Game",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager))
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

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin();
            base.Draw(GameTime);
            base.DrawObjects(GameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateObjects(GameTime, Objects);
        }
    }
}
