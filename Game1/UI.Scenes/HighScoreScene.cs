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
    public partial class HighScoreScene : Scene
    {
        public HighScoreScene() : base("High Scores") {}

        public override void LoadContent()
        {
            InitializeComponent();

            Thread loader = new Thread(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (Global.Preferences.GetStringPreference(
                        string.Format("game.highscore.user-{0}", i)).Trim() == "")
                    {
                        continue;
                    }

                    Label scoreLabel = new Label("lb");
                    scoreLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    scoreLabel.Sprite.Text = string.Format("{0}. {1} earned {2} points!",
                            i,
                            Global.Preferences.GetStringPreference(
                                string.Format("game.highscore.user-{0}", i)),
                            Global.Preferences.GetIntPreference(
                                string.Format("game.highscore.score-{0}", i)));
                    scoreLabel.Sprite.Font = Global.Fonts["default_m"];
                    ScoreContainer.Children.Add(string.Format("score-{0}", i), scoreLabel);
                }
                MainContainer.Children.Remove(Throbber1.Name);
                MainContainer.Children.Add("container", ScoreContainer);
            });
            loader.Start();

            base.LoadContent();
        }

        public override void Draw()
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            SpriteBatch.End();
        }

        public override void Update()
        {
            GuiUtils.UpdateElements(Elements);
        }
    }
}
