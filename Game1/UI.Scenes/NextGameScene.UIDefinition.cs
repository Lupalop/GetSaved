using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class NextGameScene
    {
        private MenuButton SkipTriggerArea;
        private StackPanel MainContainer;
        private StackPanel InfoContainer;
        private Image EgsImage;
        private Image HelpImage;
        private Label GameNameLabel;
        private Label GameDifficultyLabel;
        private Label ContinueTipLabel;

        private void InitializeComponent()
        {
            SkipTriggerArea = new MenuButton("skipBtn")
            {
                MenuBackground = Global.Textures["overlayBG"],
                Tint = Color.Transparent,
                MenuBackgroundSpriteType = SpriteType.None,
                Bounds = WindowBounds,
                IgnoreGlobalScale = true,
            };
            SkipTriggerArea.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(NewGameScene);
            SkipTriggerArea.OnRightClick += (sender, e) => Global.Scenes.SwitchToScene(NewGameScene);

            EgsImage = new Image("egs")
            {
                Scale = 0.8f
            };

            GameNameLabel = new Label("GameName")
            {
                Font = Global.Fonts["default_l"]
            };

            GameDifficultyLabel = new Label("GameDifficulty")
            {
                Text = string.Format("Difficulty: {0}", GameDifficulty.ToString()),
                Font = Global.Fonts["default_m"]
            };

            HelpImage = new Image("htp");

            ContinueTipLabel = new Label("label")
            {
                Text = "Click or tap anywhere to continue.",
                Font = Global.Fonts["o-default_m"],
            };

            InfoContainer = new StackPanel("infoContainer")
            {
                Orientation = Orientation.Vertical,
                Children =
                {
                    { GameNameLabel.Name, GameNameLabel },
                    { GameDifficultyLabel.Name, GameDifficultyLabel },
                    { HelpImage.Name, HelpImage },
                    { ContinueTipLabel.Name, ContinueTipLabel }
                }
            };

            MainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                Orientation = Orientation.Horizontal,
                Children =
                {
                    { EgsImage.Name, EgsImage },
                    { InfoContainer.Name, InfoContainer },
                }
            };

            Elements.Add(SkipTriggerArea.Name, SkipTriggerArea);
            Elements.Add(MainContainer.Name, MainContainer);

            Global.Display.ResolutionChanged += Display_ResolutionChanged;
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            SkipTriggerArea.Bounds = ((DisplayManager)sender).WindowBounds;
        }
    }
}
