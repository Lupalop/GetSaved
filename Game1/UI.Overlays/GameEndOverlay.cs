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
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameEndOverlay : OverlayBase
    {
        public GameEndOverlay(Games currentGame,
            Collection<GenericElement> passedMessage, SceneBase parentScene, Difficulty gameDifficulty)
            : base("Game End Overlay", parentScene)
        {
            CurrentGame = currentGame;
            ParentScene = parentScene;
            Objects = new Dictionary<string, GenericElement> {
                { "Background", new Image("Background")
                {
                    Graphic = Game.Content.Load<Texture2D>("overlayBG"),
                    ControlAlignment = ControlAlignment.Fixed,
                    OnUpdate = (element) => {
                        element.DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                    },
                }},
                { "TimesUp", new Image("TimesUp")
                {
                    Graphic = Game.Content.Load<Texture2D>("timesUp"),
                }},
                { "NextRoundBtn", new MenuButton("NextRoundBtn")
                {
                    Tooltip = "Proceed to the next game",
                    Text = "Next Round",
                    Font = Fonts["default_m"],
                    LeftClickAction = () =>
                    {
                        SceneManager.SwitchToScene(new NextGameScene());
                        SceneManager.Overlays.Remove("GameEnd");
                    }
                }},
                { "TryAgainBtn", new MenuButton("TryAgainBtn")
                {
                    Tooltip = "Having a hard time?\nTry this game again!",
                    Text = "Try Again",
                    Font = Fonts["default_m"],
                    LeftClickAction = () =>
                    {
                        SceneManager.SwitchToScene(new NextGameScene(CurrentGame, gameDifficulty));
                        SceneManager.Overlays.Remove("GameEnd");
                    }
                }},
                { "MainMenuBtn", new MenuButton("MainMenuBtn")
                {
                    Tooltip = "Back",
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5, 5),
                    ControlAlignment = ControlAlignment.Fixed,
                    Font = Fonts["default_m"],
                    LeftClickAction = () =>
                    {
                        SceneManager.SwitchToScene(new MainMenuScene());
                        SceneManager.Overlays.Remove("GameEnd");
                    }
                }}
            };

            switch (CurrentGame)
            {
                case Games.FallingObjects:
                    Game1End(passedMessage);
                    break;
                case Games.EscapeEarthquake:
                case Games.EscapeFire:
                    Game2End();
                    break;
                case Games.RunningForTheirLives:
                    Game3End();
                    break;
                case Games.HelpOthersNow:
                    Game4End(passedMessage);
                    break;
            }
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            // Show a fade effect in order for this overlay's appearance to be not abrupt
            if (!SceneManager.Overlays.ContainsKey("fade-gameEnd"))
                SceneManager.Overlays.Add("fade-gameEnd", new FadeOverlay("fade-gameEnd"));
        }

        Games CurrentGame;
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }

        public void SetGameEndGraphic(GameEndStates endState)
        {
            Image TmUp = (Image)Objects["TimesUp"];

            switch (endState)
            {
                case GameEndStates.TimesUp:
                default:
                    TmUp.Graphic = Game.Content.Load<Texture2D>("timesUp");
                    break;
                case GameEndStates.GameOver:
                    TmUp.Graphic = Game.Content.Load<Texture2D>("gameOver");
                    break;
                case GameEndStates.GameWon:
                    TmUp.Graphic = Game.Content.Load<Texture2D>("gameWin");
                    break;
            }
        }

        public void Game2End()
        {
            // Cast
            GameTwoScene scene = (GameTwoScene)ParentScene;
            //
            if (scene.IsTimedOut)
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
                SetPointsEarned(0);
                return;
            }
            //
            if (!scene.IsLevelPassed)
            {
                SetGameEndGraphic(GameEndStates.GameOver);
                SetPointsEarned(0);
                return;
            }
            SetGameEndGraphic(GameEndStates.GameWon);
            SetPointsEarned(100 * MathHelper.Clamp((int)scene.TimeLeft, 1, int.MaxValue));
        }

        public void Game3End()
        {
            // Cast
            GameThreeScene scene = (GameThreeScene)ParentScene;
            // Hardcoded to show Game over, no timer, no finish line
            SetGameEndGraphic(GameEndStates.GameOver);
            SetPointsEarned((int)scene.Score);
        }

        public void Game1End(Collection<GenericElement> CollectedObjects)
        {
            int correctItems = 0;
            int incorrectItems = 0;
            int totalItems = 0;
            // Count correct item
            foreach (FallingItem item in CollectedObjects)
            {
                if (!item.IsEmergencyItem)
                {
                    incorrectItems++;
                    continue;
                }
                correctItems++;
            }

            totalItems = (correctItems - incorrectItems);

            if (incorrectItems <= 1)
            {
                SetGameEndGraphic(GameEndStates.GameWon);
            }
            else
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
            }

            Objects.Add("IncorrectItem", new Label("IncorrectItem")
            {
                Text = "Incorrect items: " + incorrectItems,
                Font = Fonts["default_m"]
            });
            Objects.Add("CorrectItem", new Label("CorrectItem")
            {
                Text = "Correct items: " + correctItems,
                Font = Fonts["default_m"]
            });
            Objects.Add("container-items", new ElementContainer("cr")
            {
                ContainerAlignment = ContainerAlignment.Horizontal
            });

            ElementContainer itemContainer = Objects["container-items"] as ElementContainer;

            int[] IncorrectItemIDs = new int[100];

            foreach (FallingItem item in CollectedObjects)
            {
                if (item.IsEmergencyItem)
                {
                    continue;
                }

                IncorrectItemIDs[item.ItemID] += 1;
            }

            Collection<string> AvailableItems = new Collection<string> {
			    "Medkit", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "Car",
			    "Donut", "Shoes", "Jewelry", "Ball", "Wall Clock", "Chair", "Bomb"
			};

            for (int i = 0; i < IncorrectItemIDs.Length; i++)
            {
                if (IncorrectItemIDs[i] == 0)
                {
                    continue;
                }

                itemContainer.Children.Add("elemContainer" + i, new ElementContainer("cr")
                {
                    ContainerAlignment = ContainerAlignment.Vertical,
                    Children = new Dictionary<string,GenericElement>() {
                        {"icon", new MenuButton("icon")
                        {
                            SpriteType = SpriteType.None,
                            Tooltip = AvailableItems[i],
                            Graphic = Game.Content.Load<Texture2D>("falling-object/" + AvailableItems[i])
                        }},
                        {"count", new Label("lb")
                        {
                            Text = String.Format("x{0}", IncorrectItemIDs[i])
                        }}
                    }
                });
            }

            SetPointsEarned(50 * MathHelper.Clamp(totalItems, 0, int.MaxValue));
        }

        public void Game4End(Collection<GenericElement> CollectedObjects)
        {
            int peopleSaved = 0;
            int peopleDied = 0;
            int peopleTotal = 0;
            // Count item
            foreach (Helpman crap in CollectedObjects)
            {
                if (!crap.IsAlive)
                {
                    peopleDied++;
                    continue;
                }
                peopleSaved++;
            }

            peopleTotal = (peopleSaved - peopleDied);

            if (peopleDied <= 1)
            {
                SetGameEndGraphic(GameEndStates.GameWon);
            }
            else
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
            }
            Objects.Add("container-main", new ElementContainer("cr")
            {
                ContainerAlignment = ContainerAlignment.Horizontal,
                Children = new Dictionary<string,GenericElement>() {
                    { "container-list", new ElementContainer("cr")
                    {
                        Children = new Dictionary<string,GenericElement>() {
                            { "IncorrectCrap", new Label("InCorrectCrap")
                            {
                                Text = "People Died: " + peopleDied,
                                Font = Fonts["default_m"]
                            }},
                            { "CorrectCrap", new Label("CorrectCrap")
                            {
                                Text = "People Saved: " + peopleSaved,
                                Font = Fonts["default_m"]
                            }}
                        }
                    }},
                }
            });

            SetPointsEarned(50 * MathHelper.Clamp(peopleTotal, 0, int.MaxValue));
        }

        public void SetPointsEarned(int points)
        {
            UserGlobal.Score += points;
            Objects.Add("PointsEarned", new Label("points")
            {
                Text = String.Format("You earned {0} points!", points),
                Font = Fonts["o-default_m"]
            });
            Objects.Add("TotalPointsEarned", new Label("points")
            {
                Text = String.Format("{0}, you have {1} points in total.", UserGlobal.UserName, UserGlobal.Score),
                Font = Fonts["o-default"]
            });
        }
    }
}
