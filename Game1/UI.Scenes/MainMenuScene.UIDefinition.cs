using Maquina.Elements;
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
        MenuButton PlayButton;
        MenuButton CreditsButton;
        MenuButton MuteAudioButton;
        MenuButton HighScoresButton;
        MenuButton UserProfileButton;
        StackPanel MainContainer;
        StackPanel ActionContainer;

        private void InitializeComponent()
        {
            GameLogo = new Image("logo");
            GameLogo.Sprite.Graphic = Global.Textures["gameLogo"];

            GameTagline = new Label("tagline");
            GameTagline.Sprite.Text = "Disaster Preparedness for Everyone!";
            GameTagline.Sprite.Font = Global.Fonts["default_m"];

            PlayButton = new MenuButton("mb1");
            PlayButton.Tooltip.Text = "Play Game!";
            PlayButton.Background.Graphic = Global.Textures["playBtn"];
            PlayButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new WorldSelectionScene());

            VersionLabel = new Label("lb1");
            VersionLabel.Sprite.Text = "Prototype Version";
            VersionLabel.Sprite.Font = Global.Fonts["o-default"];

            //
            CreditsButton = new MenuButton("mb2");
            CreditsButton.Tooltip.Text = "Credits";
            CreditsButton.Background.Graphic = Global.Textures["circle-btn"];
            CreditsButton.Icon.Graphic = Global.Textures["credits-btn"];
            CreditsButton.Label.Font = Global.Fonts["default"];
            CreditsButton.OnRightClick += (sender, e) => {
                /*
                Global.WindowManager.Windows.Add("testwin",
                    new EmptyWindow()
                    {
                        Location = ScreenCenter.ToVector2() - new Vector2(200),
                        Dimensions = new Vector2(400, 400)
                    });
                */
            };
            CreditsButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new CreditsScene());

            MuteAudioButton = new MenuButton("mb3");
            MuteAudioButton.Tooltip.Text = "Mute Audio";
            MuteAudioButton.Background.Graphic = Global.Textures["circle-btn"];
            MuteAudioButton.Icon.Graphic = Global.Textures["sound-btn"];
            MuteAudioButton.Label.Font = Global.Fonts["default"];
            MuteAudioButton.OnLeftClick += (sender, e) => Global.Audio.ToggleMute();

            HighScoresButton = new MenuButton("mb4");
            MuteAudioButton.Tooltip.Text = "View High Scores";
            MuteAudioButton.Background.Graphic = Global.Textures["circle-btn"];
            MuteAudioButton.Icon.Graphic = Global.Textures["highscore-btn"];
            MuteAudioButton.Label.Font = Global.Fonts["default"];
            HighScoresButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new HighScoreScene());

            UserProfileButton = new MenuButton("mb5");
            UserProfileButton.Tooltip.Text = "Change User";
            UserProfileButton.Background.Graphic = Global.Textures["circle-btn"];
            UserProfileButton.Icon.Graphic = Global.Textures["user-btn"];
            UserProfileButton.Label.Font = Global.Fonts["default"];
            UserProfileButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new UserProfileScene());

            ActionContainer = new StackPanel("actionContainer")
            {
                ElementMargin = new Region(0, 5, 0, 0),
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
                ElementMargin = new Region(0, 0, 12, 0),
                Children =
                {
                    { GameLogo.Name, GameLogo },
                    { GameTagline.Name, GameTagline },
                    { PlayButton.Name, PlayButton },
                    { VersionLabel.Name, VersionLabel },
                    { ActionContainer.Name, ActionContainer },
                }
            };

            Elements.Add(MainContainer.Name, MainContainer);
        }
    }
}
