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
            OverlayBG = new Image("Background")
            {
                Graphic = Global.Textures["overlayBG"],
                IgnoreGlobalScale = true,
            };
            OverlayBG.Background.DestinationRectangle = WindowBounds;

            // Action Container
            NextRoundButton = new MenuButton("NextRoundBtn")
            {
                TooltipText = "Proceed to the next game",
                MenuLabel = "Next Round",
                MenuFont = Global.Fonts["default_m"],
            };
            NextRoundButton.OnLeftClick += (sender, e) =>
            {
                Global.Scenes.SwitchToScene(new NextGameScene());
                Global.Scenes.Overlays.Remove("GameEnd");
            };

            TryAgainButton = new MenuButton("TryAgainBtn")
            {
                TooltipText = "Having a hard time?\nTry this game again!",
                MenuLabel = "Try Again",
                MenuFont = Global.Fonts["default_m"],
            };
            TryAgainButton.OnLeftClick += (sender, e) =>
            {
                Global.Scenes.SwitchToScene(new NextGameScene(CurrentGame, CurrentDifficulty));
                Global.Scenes.Overlays.Remove("GameEnd");
            };

            BackButton = new MenuButton("MainMenuBtn")
            {
                TooltipText = "Back",
                MenuBackground = Global.Textures["back-btn"],
                Location = new Point(5, 5),
                MenuFont = Global.Fonts["default_m"],
            };
            BackButton.OnLeftClick += (sender, e) =>
            {
                Global.Scenes.SwitchToScene(new MainMenuScene());
                Global.Scenes.Overlays.Remove("GameEnd");
            };

            // Info Container
            InfoContainer = new StackPanel("InfoContainer");

            // Main Container
            TimesUp = new Image("TimesUp")
            {
                Graphic = Game.Content.Load<Texture2D>("timesUp"),
            };

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
            OverlayBG.Background.DestinationRectangle = ((DisplayManager)sender).WindowBounds;
        }
    }
}
