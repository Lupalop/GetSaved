using Maquina.Elements;
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
        private MenuButton NextRoundButton;
        private MenuButton TryAgainButton;
        private MenuButton BackButton;
        private StackPanel MainContainer;
        private StackPanel InfoContainer;

        private void InitializeComponent()
        {
            OverlayBG = new Image("Background");
            OverlayBG.Sprite.Graphic = Global.Textures["overlayBG"];
            OverlayBG.IgnoreGlobalScale = true;
            OverlayBG.Sprite.DestinationRectangle = WindowBounds;

            // Action Container
            NextRoundButton = new MenuButton("NextRoundBtn");
            NextRoundButton.Tooltip.Text = "Proceed to the next game";
            NextRoundButton.Label.Text = "Next Round";
            NextRoundButton.Label.Font = Global.Fonts["default_m"];
            NextRoundButton.OnLeftClick += (sender, e) =>
            {
                Global.Scenes.SwitchToScene(new NextGameScene());
                Global.Scenes.Overlays.Remove("GameEnd");
            };

            TryAgainButton = new MenuButton("TryAgainBtn");
            TryAgainButton.Tooltip.Text = "Having a hard time?\nTry this game again!";
            TryAgainButton.Label.Text = "Try Again";
            TryAgainButton.Label.Font = Global.Fonts["default_m"];
            TryAgainButton.OnLeftClick += (sender, e) =>
            {
                Global.Scenes.SwitchToScene(new NextGameScene(CurrentGame, CurrentDifficulty));
                Global.Scenes.Overlays.Remove("GameEnd");
            };

            BackButton = new MenuButton("MainMenuBtn");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = Global.Textures["back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.Label.Font = Global.Fonts["default_m"];
            BackButton.OnLeftClick += (sender, e) =>
            {
                Global.Scenes.SwitchToScene(new MainMenuScene());
                Global.Scenes.Overlays.Remove("GameEnd");
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

            Elements.Add(OverlayBG.Name, OverlayBG);
            Elements.Add(MainContainer.Name, MainContainer);

            Global.Display.ResolutionChanged += Display_ResolutionChanged; ;
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            OverlayBG.Sprite.DestinationRectangle = ((DisplayManager)sender).WindowBounds;
        }
    }
}
