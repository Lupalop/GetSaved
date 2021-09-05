using Maquina.Entities;
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
        private Button SkipTriggerArea;
        private StackPanel MainContainer;
        private StackPanel InfoContainer;
        private Image EgsImage;
        private Image HelpImage;
        private Label GameNameLabel;
        private Label GameDifficultyLabel;
        private Label ContinueTipLabel;

        private void InitializeComponent()
        {
            SkipTriggerArea = new Button("skipBtn");
            SkipTriggerArea.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("overlayBG"];
            SkipTriggerArea.Tint = Color.Transparent;
            SkipTriggerArea.Background.SpriteType = SpriteType.None;
            SkipTriggerArea.Bounds = WindowBounds;
            SkipTriggerArea.IgnoreApplicationScale = true;
            SkipTriggerArea.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(NewGameScene);
            SkipTriggerArea.OnRightClick += (sender, e) => Application.Scenes.SwitchToScene(NewGameScene);

            EgsImage = new Image("egs")
            {
                Scale = 0.8f
            };

            GameNameLabel = new Label("GameName");
            GameNameLabel.Sprite.Font = Application.Fonts["default_l"];

            GameDifficultyLabel = new Label("GameDifficulty");
            GameDifficultyLabel.Sprite.Text = string.Format("Difficulty: {0}", GameDifficulty.ToString());
            GameDifficultyLabel.Sprite.Font = Application.Fonts["default_m"];

            HelpImage = new Image("htp");

            ContinueTipLabel = new Label("label");
            ContinueTipLabel.Sprite.Text = "Click or tap anywhere to continue.";
            ContinueTipLabel.Sprite.Font = Application.Fonts["o-default_m"];

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

            Entities.Add(SkipTriggerArea.Name, SkipTriggerArea);
            Entities.Add(MainContainer.Name, MainContainer);

            Application.Display.ResolutionChanged += Display_ResolutionChanged;
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            SkipTriggerArea.Bounds = ((DisplayManager)sender).WindowBounds;
        }
    }
}
