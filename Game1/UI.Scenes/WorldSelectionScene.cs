using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.Elements;

namespace Maquina.UI.Scenes
{
    public class WorldSelectionScene : Scene
    {
        public WorldSelectionScene() : base("Game Selection") {}

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "mb1", new MenuButton("mb")
                {
                    Tooltip = "Back",
                    Graphic = Global.Textures["back-btn"],
                    Location = new Vector2(5,5),
                    ControlAlignment = Alignment.Fixed,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene())
                }},
                { "mb2", new MenuButton("mb")
                {
                    Tooltip = "Change the game's difficulty",
                    Text = String.Format("Difficulty: {0}", difficulty),
                    OnUpdate = (element) => {
                        MenuButton mb = (MenuButton)element;
                        mb.Text = String.Format("Difficulty: {0}", difficulty);
                    },
                    LeftClickAction = () => ModifyDifficulty()
                }},
                { "container1", new StackPanel("cr")
                {
                    ElementMargin = new Region(0, 5, 0, 0),
                    Orientation = Orientation.Horizontal,
                    Children = {
                        { "mb3", new MenuButton("mb")
                        {
                            Graphic = Global.Textures["worldselection-one"],
                            Rows = 1,
                            Columns = 2,
                            Tooltip = "The Safety Kit",
                            Scale = 0.7f,
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.FallingObjects, difficulty))
                        }},
                        { "mb4", new MenuButton("mb")
                        {
                            Graphic = Global.Textures["worldselection-two"],
                            Rows = 1,
                            Columns = 2,
                            Tooltip = "Earthquake Escape",
                            Scale = 0.7f,
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.EscapeEarthquake, difficulty))
                        }},
                        { "mb5", new MenuButton("mb")
                        {
                            Graphic = Global.Textures["worldselection-three"],
                            Rows = 1,
                            Columns = 2,
                            Tooltip = "Fire Escape",
                            Scale = 0.7f,
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.EscapeFire, difficulty))
                        }},
                    }
                }},
                { "container2", new StackPanel("cr")
                {
                    ElementMargin = new Region(0, 5, 0, 0),
                    Orientation = Orientation.Horizontal,
                    Children = {
                        { "mb6", new MenuButton("mb")
                        {
                            Graphic = Global.Textures["worldselection-four"],
                            Rows = 1,
                            Columns = 2,
                            Tooltip = "Safety Jump - Fire",
                            Scale = 0.7f,
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.RunningForTheirLives, difficulty))
                        }},
                        { "mb7", new MenuButton("mb")
                        {
                            Graphic = Global.Textures["worldselection-five"],
                            Rows = 1,
                            Columns = 2,
                            Tooltip = "Aid 'Em - Earthquake",
                            Scale = 0.7f,
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.HelpOthersNow, difficulty))
                        }},
                    }
                }},
                { "mb9", new MenuButton("mb")
                {
                    Tooltip = "Random Game",
                    SpriteType = SpriteType.None,
                    Graphic = Global.Textures["htp-dice"],
                    OnUpdate = (Dice) => {
                        Dice.RotationOrigin = new Vector2(Dice.Graphic.Width / 2, Dice.Graphic.Height / 2);
                        Dice.Location = new Vector2(Dice.Location.X + (Dice.Bounds.Width / 2), Dice.Location.Y + (Dice.Bounds.Height / 2));
                        Dice.Rotation += .05f;
                    },
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene())
                }},
            };

            BackgroundGameScene = new GameOneScene(Difficulty.Demo);
            BackgroundGameScene.LoadContent();
            BackgroundGameScene.DelayLoadContent();

            difficulty = Difficulty.Easy;
        }

        private GameOneScene BackgroundGameScene;
        private Difficulty difficulty;

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
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            BackgroundGameScene.Draw(gameTime);
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            BackgroundGameScene.Update(gameTime);
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }

        public override void Unload()
        {
            base.Unload();
            BackgroundGameScene.Unload();
        }
    }
}
