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
using Maquina.UI.Controls;
using Maquina.Objects;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameEndOverlay : OverlayBase
    {
        public GameEndOverlay(SceneManager SceneManager, Games cGame, Collection<GenericElement> passedMessage, SceneBase parentScene)
            : base(SceneManager, "Game End Overlay", parentScene)
        {
            currentGame = cGame;
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
                    Location = ScreenCenter,
                    SpriteBatch = this.SpriteBatch
                }},
                { "NextRoundBtn", new MenuButton("NextRoundBtn", SceneManager)
                {
                    Location = ScreenCenter,
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    Rows = 1,
                    Columns = 3,
                    Text = "Next Round",
                    Font = Fonts["default_m"],
                    LeftClickAction = () => { SceneManager.SwitchToScene(new NextGameScene(SceneManager)); SceneManager.Overlays.Remove("GameEnd"); }
                }},
                { "TryAgainBtn", new MenuButton("TryAgainBtn", SceneManager)
                {
                    Location = ScreenCenter,
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    Rows = 1,
                    Columns = 3,
                    Text = "Try Again",
                    Font = Fonts["default_m"],
                    LeftClickAction = () => { SceneManager.SwitchToScene(new NextGameScene(SceneManager, currentGame)); SceneManager.Overlays.Remove("GameEnd"); }
                }},
                { "MainMenuBtn", new MenuButton("MainMenuBtn", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    Rows = 1,
                    Columns = 3,
                    ControlAlignment = ControlAlignment.Fixed,
                    Font = Fonts["default_m"],
                    LeftClickAction = () => { SceneManager.SwitchToScene(new MainMenuScene(SceneManager)); SceneManager.Overlays.Remove("GameEnd"); }
                }}
            };

            switch (currentGame)
            {
                case Games.FallingObjects:
                    Game1End(passedMessage);
                    break;
                case Games.EscapeEarthquake:
                case Games.EscapeFire:
                    Game2End(passedMessage);
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
                SceneManager.Overlays.Add("fade-{0}", new Scenes.FadeOverlay(SceneManager, "fade-{0}"));
        }

        Games currentGame;
        public override void Draw(GameTime GameTime)
        {
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
                    TmUp.Graphic = Game.Content.Load<Texture2D>("GameOver");
                    break;
                case GameEndStates.GameWon:
                    TmUp.Graphic = Game.Content.Load<Texture2D>("GameWin");
                    break;
            }
        }

        public void Game2End(Collection<GenericElement> PassedMsg)
        {
            if (PassedMsg.Count != 0)
            {
                if ((bool)PassedMsg[0].MessageHolder[0] == false)
                    SetGameEndGraphic(GameEndStates.GameOver);
                else if ((bool)PassedMsg[0].MessageHolder[0] == true)
                    SetGameEndGraphic(GameEndStates.GameWon);
            }
            else
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
            }

        }

        public void Game3End()
        {
            // Hardcoded to show Game over, no timer, no finish line crap
            SetGameEndGraphic(GameEndStates.GameOver);
        }

        public void Game1End(Collection<GenericElement> CollectedObjects)
        {
            int correctCrap = 0;
            int wrongCrap = 0;
            // Count correct crap
            foreach (var crap in CollectedObjects)
            {
                if (crap.MessageHolder[0].ToString().Contains('!'))
                    wrongCrap++;
                else
                    correctCrap++;
            }

            if (wrongCrap <= 1)
                SetGameEndGraphic(GameEndStates.GameWon);
            else
                SetGameEndGraphic(GameEndStates.TimesUp);

            Objects.Add("CorrectCrap", new Label("CorrectCrap")
            {
                Location = ScreenCenter,
                SpriteBatch = this.SpriteBatch,
                Text = "Correct items: " + correctCrap,
                Font = Fonts["default_m"]
            });
            Objects.Add("IncorrectCrap", new Label("InCorrectCrap")
            {
                Location = ScreenCenter,
                SpriteBatch = this.SpriteBatch,
                Text = "Incorrect items: " + wrongCrap,
                Font = Fonts["default_m"]
            });
        }

        public void Game4End(Collection<GenericElement> CollectedObjects)
        {
            int peopleSaved = 0;
            int peopleDied = 0;
            // Count crap
            foreach (var crap in CollectedObjects)
            {
                if (crap.MessageHolder[0].ToString().Contains('!'))
                    peopleDied++;
                else
                    peopleSaved++;
            }

            if (peopleDied <= 1)
                SetGameEndGraphic(GameEndStates.GameWon);
            else
                SetGameEndGraphic(GameEndStates.TimesUp);

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
