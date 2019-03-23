using System;
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
        public GameEndOverlay(SceneManager sceneManager,
            Games currentGame, Collection<GenericElement> passedMessage,
            SceneBase parentScene)
            : base(sceneManager, "Game End Overlay", parentScene)
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
                        SceneManager.SwitchToScene(new NextGameScene(SceneManager));
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
                        SceneManager.SwitchToScene(new NextGameScene(SceneManager, CurrentGame));
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
                    LeftClickAction = () => { SceneManager.SwitchToScene(new MainMenuScene(SceneManager)); SceneManager.Overlays.Remove("GameEnd"); }
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

            // Show a fade effect to hide first frame misposition
            if (!SceneManager.Overlays.ContainsKey("fade-{0}"))
                SceneManager.Overlays.Add("fade-{0}", new FadeOverlay(SceneManager, "fade-{0}"));
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
                return;
            }
            //
            if (!scene.IsLevelPassed)
            {
                SetGameEndGraphic(GameEndStates.GameOver);
                return;
            }
            SetGameEndGraphic(GameEndStates.GameWon);
        }

        public void Game3End()
        {
            // Hardcoded to show Game over, no timer, no finish line
            SetGameEndGraphic(GameEndStates.GameOver);
        }
        // TODO: Merge Game1 with Game4
        public void Game1End(Collection<GenericElement> CollectedObjects)
        {
            int correctItems = 0;
            int incorrectItems = 0;
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
                Location = ScreenCenter,
                SpriteBatch = this.SpriteBatch,
                Text = "Correct items: " + correctItems,
                Font = Fonts["default_m"]
            });
            Objects.Add("IncorrectItem", new Label("IncorrectItem")
            {
                Location = ScreenCenter,
                SpriteBatch = this.SpriteBatch,
                Text = "Incorrect items: " + incorrectItems,
                Font = Fonts["default_m"]
            });
        }

        public void Game4End(Collection<GenericElement> CollectedObjects)
        {
            int peopleSaved = 0;
            int peopleDied = 0;
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
                Location = ScreenCenter,
                SpriteBatch = this.SpriteBatch,
                Text = "People Saved: " + peopleSaved,
                Font = Fonts["default_m"]
            });
            Objects.Add("IncorrectCrap", new Label("InCorrectCrap")
            {
                Location = ScreenCenter,
                SpriteBatch = this.SpriteBatch,
                Text = "People Died: " + peopleDied,
                Font = Fonts["default_m"]
            });
        }
    }
}
