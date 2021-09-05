﻿using Maquina.Entities;
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
        private Button BackButton;
        private Label HeaderLabel;
        private Throbber Throbber1;
        private StackPanel ScoreContainer;
        private StackPanel MainContainer;

        private void InitializeComponent()
        {
            BackButton = new Button("mb1");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new MainMenuScene());

            HeaderLabel = new Label("lb1");
            HeaderLabel.Sprite.Text = "High Scores";
            HeaderLabel.Sprite.Font = Application.Fonts["o-default_l"];

            Throbber1 = new Throbber("tb1");

            ScoreContainer = new StackPanel("cr")
            {
                ControlMargin = new Margin(10, 0, 15, 0),
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

            Entities.Add(MainContainer.Name, MainContainer);
            Entities.Add(BackButton.Name, BackButton);
        }
    }
}
