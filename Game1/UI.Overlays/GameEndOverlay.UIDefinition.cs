using Maquina.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class GameEndOverlay
    {
        private Image OverlayBG;
        private Image TimesUp;
        private Button NextRoundButton;
        private Button TryAgainButton;
        private Button BackButton;
        private StackPanel MainContainer;
        private StackPanel InfoContainer;

        private void InitializeComponent()
        {
            OverlayBG = new Image("Background");
            OverlayBG.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("overlayBG"];
            OverlayBG.IgnoreApplicationScale = true;
            OverlayBG.Sprite.DestinationRectangle = WindowBounds;

            // Action Container
            NextRoundButton = new Button("NextRoundBtn");
            NextRoundButton.Tooltip.Text = "Proceed to the next game";
            NextRoundButton.Label.Text = "Next Round";
            NextRoundButton.Label.Font = Application.Fonts["default_m"];
            NextRoundButton.OnLeftClick += (sender, e) =>
            {
                Application.Scenes.SwitchToScene(new NextGameScene());
                Application.Scenes.Overlays.Remove("GameEnd");
            };

            TryAgainButton = new Button("TryAgainBtn");
            TryAgainButton.Tooltip.Text = "Having a hard time?\nTry this game again!";
            TryAgainButton.Label.Text = "Try Again";
            TryAgainButton.Label.Font = Application.Fonts["default_m"];
            TryAgainButton.OnLeftClick += (sender, e) =>
            {
                Application.Scenes.SwitchToScene(new NextGameScene(CurrentGame, CurrentDifficulty));
                Application.Scenes.Overlays.Remove("GameEnd");
            };

            BackButton = new Button("MainMenuBtn");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.Label.Font = Application.Fonts["default_m"];
            BackButton.OnLeftClick += (sender, e) =>
            {
                Application.Scenes.SwitchToScene(new MainMenuScene());
                Application.Scenes.Overlays.Remove("GameEnd");
            };

            // Info Container
            InfoContainer = new StackPanel("InfoContainer");

            // Main Container
            TimesUp = new Image("TimesUp");
            TimesUp.Sprite.Graphic = Game.Content.Load<Texture2D>("timesUp");

            MainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                Children =
                {
                    { TimesUp.Name, TimesUp },
                    { InfoContainer.Name, InfoContainer },
                    { NextRoundButton.Name, NextRoundButton },
                    { TryAgainButton.Name, TryAgainButton },
                    { BackButton.Name, BackButton },
                }
            };

            Entities.Add(OverlayBG.Name, OverlayBG);
            Entities.Add(MainContainer.Name, MainContainer);

            Application.Display.ResolutionChanged += Display_ResolutionChanged; ;
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            OverlayBG.Sprite.DestinationRectangle = ((DisplayManager)sender).WindowBounds;
        }
    }
}
