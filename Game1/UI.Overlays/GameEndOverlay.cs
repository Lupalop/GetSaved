﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            Collection<GenericElement> passedMessage, SceneBase parentScene)
            : base("Game End Overlay", parentScene)
        {
            CurrentGame = currentGame;
            ParentScene = parentScene;
            Objects = new Dictionary<string, GenericElement> {
                { "Background", new Image("Background")
                {
                    Graphic = Game.Content.Load<Texture2D>("overlayBG"),
                    ControlAlignment = ControlAlignment.Fixed,
                    OnUpdate = () => {
                        Objects["Background"].DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                    },
                    SpriteBatch = this.SpriteBatch
                }},
                { "TimesUp", new Image("TimesUp")
                {
                    Graphic = Game.Content.Load<Texture2D>("timesUp"),
                    SpriteBatch = this.SpriteBatch
                }},
                { "NextRoundBtn", new MenuButton("NextRoundBtn", SceneManager)
                {
                    Tooltip = "Proceed to the next game",
                    SpriteBatch = this.SpriteBatch,
                    Text = "Next Round",
                    Font = Fonts["default_m"],
                    LeftClickAction = () =>
                    {
                        SceneManager.SwitchToScene(new NextGameScene());
                        SceneManager.Overlays.Remove("GameEnd");
                    }
                }},
                { "TryAgainBtn", new MenuButton("TryAgainBtn", SceneManager)
                {
                    Tooltip = "Having a hard time? Try this game again!",
                    SpriteBatch = this.SpriteBatch,
                    Text = "Try Again",
                    Font = Fonts["default_m"],
                    LeftClickAction = () =>
                    {
                        SceneManager.SwitchToScene(new NextGameScene(CurrentGame));
                        SceneManager.Overlays.Remove("GameEnd");
                    }
                }},
                { "MainMenuBtn", new MenuButton("MainMenuBtn", SceneManager)
                {
                    Tooltip = "Back",
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5, 5),
                    SpriteBatch = this.SpriteBatch,
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

        // TODO: Merge Game1 with Game4
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

            Objects.Add("CorrectItem", new Label("CorrectItem")
            {
                SpriteBatch = this.SpriteBatch,
                Text = "Correct items: " + correctItems,
                Font = Fonts["default_m"]
            });
            Objects.Add("IncorrectItem", new Label("IncorrectItem")
            {
                SpriteBatch = this.SpriteBatch,
                Text = "Incorrect items: " + incorrectItems,
                Font = Fonts["default_m"]
            });

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

            Objects.Add("CorrectCrap", new Label("CorrectCrap")
            {
                SpriteBatch = this.SpriteBatch,
                Text = "People Saved: " + peopleSaved,
                Font = Fonts["default_m"]
            });
            Objects.Add("IncorrectCrap", new Label("InCorrectCrap")
            {
                SpriteBatch = this.SpriteBatch,
                Text = "People Died: " + peopleDied,
                Font = Fonts["default_m"]
            });

            SetPointsEarned(50 * MathHelper.Clamp(peopleTotal, 0, int.MaxValue));
        }

        public void SetPointsEarned(int points)
        {
            UserGlobal.Score += points;
            Objects.Add("PointsEarned", new Label("points")
            {
                SpriteBatch = this.SpriteBatch,
                Text = String.Format("You earned {0} points!", points),
                Font = Fonts["o-default_m"]
            });
            Objects.Add("TotalPointsEarned", new Label("points")
            {
                SpriteBatch = this.SpriteBatch,
                Text = String.Format("{0}, you have {1} points in total.", UserGlobal.UserName, UserGlobal.Score),
                Font = Fonts["o-default"]
            });
        }
    }
}
