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
    public class HighScoreScene : SceneBase
    {
        public HighScoreScene(SceneManager SceneManager)
            : base(SceneManager, "High Scores")
        {
            this.PreferencesManager = new PreferencesManager();
        }

        private PreferencesManager PreferencesManager;

        public override void LoadContent()
        {
            base.LoadContent();

            ElementContainer elementContainer = new ElementContainer("cr")
            {
                ElementSpacing = 5
            };

            for (int i = 1; i <= 10; i++)
            {
                if (PreferencesManager.GetCharPref(String.Format("game.highscore.user-{0}", i)).Trim() == "")
                {
                    continue;
                }

                elementContainer.Children.Add(String.Format("score-{0}", i), new Label("lb")
                {
                    Text = String.Format("{0}. {1} earned {2} points!",
                        i,
                        PreferencesManager.GetCharPref(
                            String.Format("game.highscore.user-{0}", i)),
                        PreferencesManager.GetIntPref(
                            String.Format("game.highscore.score-{0}", i))),
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["default_m"]
                });
            }

            Objects = new Dictionary<string, GenericElement> {
                { "mb1", new MenuButton("mb", SceneManager)
                {
                    Tooltip = "Back",
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5, 5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},
                { "header", new Label("lb")
                {
                    Text = "High Scores",
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["o-default_l"]
                }},
                { "container", elementContainer }
            };
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
