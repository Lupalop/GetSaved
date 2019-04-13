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
using System.Threading;

namespace Maquina.UI.Scenes
{
    public class HighScoreScene : SceneBase
    {
        public HighScoreScene() : base("High Scores") {}

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "mb1", new MenuButton("mb")
                {
                    Tooltip = "Back",
                    Graphic = Global.Textures["back-btn"],
                    Location = new Vector2(5, 5),
                    ControlAlignment = ControlAlignment.Fixed,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene())
                }},
                { "header", new Label("lb")
                {
                    Text = "High Scores",
                    Font = Fonts["o-default_l"]
                }},
                { "throbber", new Throbber("tb")
                },
            };

            Thread loader = new Thread(() =>
            {
                ElementContainer elementContainer = new ElementContainer("cr")
                {
                    ElementSpacing = 5
                };

                for (int i = 1; i <= 10; i++)
                {
                    if (Global.PreferencesManager.GetCharPref(String.Format("game.highscore.user-{0}", i)).Trim() == "")
                    {
                        continue;
                    }

                    elementContainer.Children.Add(String.Format("score-{0}", i), new Label("lb")
                    {
                        ControlAlignment = ControlAlignment.Left,
                        Text = String.Format("{0}. {1} earned {2} points!",
                            i,
                            Global.PreferencesManager.GetCharPref(
                                String.Format("game.highscore.user-{0}", i)),
                            Global.PreferencesManager.GetIntPref(
                                String.Format("game.highscore.score-{0}", i))),
                        Font = Fonts["default_m"]
                    });
                }
                Objects.Remove("throbber");
                Objects.Add("container", elementContainer);
            });

            loader.Start();
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
