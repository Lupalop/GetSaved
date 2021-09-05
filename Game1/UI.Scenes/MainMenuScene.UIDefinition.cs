using Maquina.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class MainMenuScene
    {
        Image GameLogo;
        Label GameTagline;
        Label VersionLabel;
        Button PlayButton;
        Button CreditsButton;
        Button MuteAudioButton;
        Button HighScoresButton;
        Button UserProfileButton;
        StackPanel MainContainer;
        StackPanel ActionContainer;

        private void InitializeComponent()
        {
            GameLogo = new Image("logo");
            GameLogo.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("gameLogo"];

            GameTagline = new Label("tagline");
            GameTagline.Sprite.Text = "Disaster Preparedness for Everyone!";
            GameTagline.Sprite.Font = Application.Fonts["default_m"];

            PlayButton = new Button("mb1");
            PlayButton.Tooltip.Text = "Play Game!";
            PlayButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("playBtn"];
            PlayButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new WorldSelectionScene());

            VersionLabel = new Label("lb1");
            VersionLabel.Sprite.Text = "Prototype Version";
            VersionLabel.Sprite.Font = Application.Fonts["o-default"];

            //
            CreditsButton = new Button("mb2");
            CreditsButton.Tooltip.Text = "Credits";
            CreditsButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("circle-btn"];
            CreditsButton.Icon.Graphic = (TextureSprite)ContentFactory.TryGetResource("credits-btn"];
            CreditsButton.Label.Font = Application.Fonts["default"];
            CreditsButton.OnRightClick += (sender, e) => {
                /*
                Application.WindowManager.Windows.Add("testwin",
                    new EmptyWindow()
                    {
                        Location = ScreenCenter.ToVector2() - new Vector2(200),
                        Dimensions = new Vector2(400, 400)
                    });
                */
            };
            CreditsButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new CreditsScene());

            MuteAudioButton = new Button("mb3");
            MuteAudioButton.Tooltip.Text = "Mute Audio";
            MuteAudioButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("circle-btn"];
            MuteAudioButton.Icon.Graphic = (TextureSprite)ContentFactory.TryGetResource("sound-btn"];
            MuteAudioButton.Label.Font = Application.Fonts["default"];
            MuteAudioButton.OnLeftClick += (sender, e) => Application.Audio.ToggleMute();

            HighScoresButton = new Button("mb4");
            HighScoresButton.Tooltip.Text = "View High Scores";
            HighScoresButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("circle-btn"];
            HighScoresButton.Icon.Graphic = (TextureSprite)ContentFactory.TryGetResource("highscore-btn"];
            HighScoresButton.Label.Font = Application.Fonts["default"];
            HighScoresButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new HighScoreScene());

            UserProfileButton = new Button("mb5");
            UserProfileButton.Tooltip.Text = "Change User";
            UserProfileButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("circle-btn"];
            UserProfileButton.Icon.Graphic = (TextureSprite)ContentFactory.TryGetResource("user-btn"];
            UserProfileButton.Label.Font = Application.Fonts["default"];
            UserProfileButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new UserProfileScene());

            ActionContainer = new StackPanel("actionContainer")
            {
                ControlMargin = new Margin(0, 5, 0, 0),
                Orientation = Orientation.Horizontal,
                Children =
                {
                    { CreditsButton.Name, CreditsButton },
                    { MuteAudioButton.Name, MuteAudioButton },
                    { HighScoresButton.Name, HighScoresButton },
                    { UserProfileButton.Name, UserProfileButton },
                }
            };

            //
            MainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                ControlMargin = new Margin(0, 0, 12, 0),
                Children =
                {
                    { GameLogo.Name, GameLogo },
                    { GameTagline.Name, GameTagline },
                    { PlayButton.Name, PlayButton },
                    { VersionLabel.Name, VersionLabel },
                    { ActionContainer.Name, ActionContainer },
                }
            };

            Entities.Add(MainContainer.Name, MainContainer);
        }
    }
}
