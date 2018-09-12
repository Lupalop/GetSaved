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
    public class GameEndOverlay : SceneBase
    {
        public GameEndOverlay(SceneManager sceneManager, Games cgame, List<ObjectBase> passedMessage)
            : base(sceneManager, "Game End Overlay")
        {
            currentGame = cgame;
            Objects = new Dictionary<string, ObjectBase> {
                { "Background", new Image("Background")
                {
                    Graphic = game.Content.Load<Texture2D>("overlayBG"),
                    CustomRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
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
                    Text = "Next Round",
                    Font = fonts["default_m"],
                    ClickAction = () => { sceneManager.currentScene = new WorldSelectionScene(sceneManager); sceneManager.overlays.Remove("gameEnd"); }
                }}
            };

            switch (currentGame)
            {
                case Games.FallingObjects:
                    Game1End(passedMessage);
                    break;
                case Games.EscapeEarthquake:
                    break;
                case Games.EscapeFire:
                    break;
                case Games.RunningForTheirLives:
                    break;
                case Games.HelpOthersNow:
                    break;
            }
        }

        Games currentGame;
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
        }
        public override void Update(GameTime gameTime)
        {
            Objects["Background"].CustomRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
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
    }
}
