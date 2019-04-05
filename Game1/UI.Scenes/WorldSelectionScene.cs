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
    public class WorldSelectionScene : SceneBase
    {
        public WorldSelectionScene() : base("Game Selection") {}

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "mb1", new MenuButton("mb")
                {
                    Tooltip = "Back",
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene())
                }},
                { "mb2", new MenuButton("mb")
                {
                    Tooltip = "Change the game's difficulty",
                    Text = String.Format("Difficulty: {0}", difficulty),
                    OnUpdate = () => {
                        MenuButton dfBtn = (MenuButton)Objects["mb2"];
                        dfBtn.Text = String.Format("Difficulty: {0}", difficulty);
                    },
                    LeftClickAction = () => ModifyDifficulty()
                }},
                { "container1", new ElementContainer("cr")
                {
                    ElementSpacing = 5,
                    ContainerAlignment = ContainerAlignment.Horizontal,
                    Children = new Dictionary<string,GenericElement>() {
                        { "mb3", new MenuButton("mb")
                        {
                            Text = "The Safety Kit",
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.FallingObjects, difficulty))
                        }},
                        { "mb4", new MenuButton("mb")
                        {
                            Text = "Earthquake Escape",
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.EscapeEarthquake, difficulty))
                        }},
                        { "mb5", new MenuButton("mb")
                        {
                            Text = "Fire Escape",
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.EscapeFire, difficulty))
                        }},
                    }
                }},
                { "container2", new ElementContainer("cr")
                {
                    ElementSpacing = 5,
                    ContainerAlignment = ContainerAlignment.Horizontal,
                    Children = new Dictionary<string,GenericElement>() {
                        { "mb6", new MenuButton("mb")
                        {
                            Text = "Safety Jump - Fire",
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.RunningForTheirLives, difficulty))
                        }},
                        { "mb7", new MenuButton("mb")
                        {
                            Text = "Aid 'Em - Earthquake",
                            LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(Games.HelpOthersNow, difficulty))
                        }},
                    }
                }},
                { "mb9", new MenuButton("mb")
                {
                    Tooltip = "Random Game",
                    SpriteType = SpriteType.None,
                    Graphic = Game.Content.Load<Texture2D>("htp/dice"),
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene())
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

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
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
