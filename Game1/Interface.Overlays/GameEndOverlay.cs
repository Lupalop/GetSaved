using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface;
using Arkabound.Interface.Controls;
using Arkabound.Objects;

namespace Arkabound.Interface.Scenes
{
    public class GameEndOverlay : OverlayBase
    {
        public GameEndOverlay(SceneManager sceneManager, Games cgame, List<ObjectBase> passedMessage, SceneBase parentScene)
            : base(sceneManager, "Game End Overlay", parentScene)
        {
            currentGame = cgame;
            Objects = new Dictionary<string, ObjectBase> {
                { "Background", new Image("Background")
                {
                    Graphic = game.Content.Load<Texture2D>("overlayBG"),
                    DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                }},
                { "TimesUp", new Image("TimesUp")
                {
                    Graphic = game.Content.Load<Texture2D>("timesUp"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch
                }},
                { "NextRoundBtn", new MenuButton("NextRoundBtn", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Text = "Next Round",
                    Font = fonts["default_m"],
                    LeftClickAction = () => { sceneManager.currentScene = new NextGameScene(sceneManager); sceneManager.overlays.Remove("gameEnd"); }
                }},
                { "TryAgainBtn", new MenuButton("TryAgainBtn", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Text = "Try Again",
                    Font = fonts["default_m"],
                    LeftClickAction = () => { sceneManager.currentScene = new NextGameScene(sceneManager, currentGame); sceneManager.overlays.Remove("gameEnd"); }
                }},
                { "MainMenuBtn", new MenuButton("MainMenuBtn", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(5,5),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    AlignToCenter = false,
                    Text = "Go Home",
                    Font = fonts["default_m"],
                    LeftClickAction = () => { sceneManager.currentScene = new MainMenuScene(sceneManager); sceneManager.overlays.Remove("gameEnd"); }
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

        Games currentGame;
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            Objects["Background"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
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
                    TmUp.Graphic = game.Content.Load<Texture2D>("timesUp");
                    break;
                case GameEndStates.GameOver:
                    TmUp.Graphic = game.Content.Load<Texture2D>("gameOver");
                    break;
                case GameEndStates.GameWon:
                    TmUp.Graphic = game.Content.Load<Texture2D>("gameWin");
                    break;
            }
        }

        public void Game2End(List<ObjectBase> PassedMsg)
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
            // Hardcoded to show game over, no timer, no finish line crap
            SetGameEndGraphic(GameEndStates.GameOver);
        }

        public void Game1End(List<ObjectBase> CollectedObjects)
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
                spriteBatch = this.spriteBatch,
                Text = "Correct items: " + correctCrap,
                Font = fonts["default_m"]
            });
            Objects.Add("IncorrectCrap", new Label("InCorrectCrap")
            {
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Text = "Incorrect items: " + wrongCrap,
                Font = fonts["default_m"]
            });
        }

        public void Game4End(List<ObjectBase> CollectedObjects)
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
                spriteBatch = this.spriteBatch,
                Text = "People Saved: " + peopleSaved,
                Font = fonts["default_m"]
            });
            Objects.Add("IncorrectCrap", new Label("InCorrectCrap")
            {
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Text = "People Died: " + peopleDied,
                Font = fonts["default_m"]
            });
        }
    }
}
