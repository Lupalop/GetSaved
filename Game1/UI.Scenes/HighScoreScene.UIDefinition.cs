using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class HighScoreScene
    {
        private MenuButton BackButton;
        private Label HeaderLabel;
        private Throbber Throbber1;
        private StackPanel ScoreContainer;
        private StackPanel MainContainer;

        private void InitializeComponent()
        {
            BackButton = new MenuButton("mb1")
            {
                TooltipText = "Back",
                MenuBackground = Global.Textures["back-btn"],
                Location = new Point(5, 5),
            };
            BackButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new MainMenuScene());

            HeaderLabel = new Label("lb1")
            {
                Text = "High Scores",
                Font = Global.Fonts["o-default_l"]
            };

            Throbber1 = new Throbber("tb1");

            ScoreContainer = new StackPanel("cr")
            {
                ElementMargin = new Region(10, 0, 15, 0),
            };

            MainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                Children =
                {
                    { HeaderLabel.Name, HeaderLabel},
                    { Throbber1.Name, Throbber1 },
                }
            };

            Elements.Add(MainContainer.Name, MainContainer);
            Elements.Add(BackButton.Name, BackButton);
        }
    }
}
