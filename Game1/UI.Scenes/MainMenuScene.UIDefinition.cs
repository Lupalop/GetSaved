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

            PlayButton = new MenuButton("mb1")
            {
                TooltipText = "Play Game!",
                MenuBackground = Global.Textures["playBtn"],
            };
            PlayButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new WorldSelectionScene());

            VersionLabel = new Label("lb1");
            VersionLabel.Sprite.Text = "Prototype Version";
            VersionLabel.Sprite.Font = Global.Fonts["o-default"];

            //
            CreditsButton = new MenuButton("mb2")
            {
                TooltipText = "Credits",
                MenuBackground = Global.Textures["circle-btn"],
                MenuIcon = Global.Textures["credits-btn"],
                MenuFont = Global.Fonts["default"],
            };
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

            MuteAudioButton = new MenuButton("mb3")
            {
                TooltipText = "Mute Audio",
                MenuBackground = Global.Textures["circle-btn"],
                MenuIcon = Global.Textures["sound-btn"],
                MenuFont = Global.Fonts["default"],
            };
            MuteAudioButton.OnLeftClick += (sender, e) => Global.Audio.ToggleMute();

            HighScoresButton = new MenuButton("mb4")
            {
                TooltipText = "View High Scores",
                MenuBackground = Global.Textures["circle-btn"],
                MenuIcon = Global.Textures["highscore-btn"],
                MenuFont = Global.Fonts["default"],
            };
            HighScoresButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new HighScoreScene());

            UserProfileButton = new MenuButton("mb5")
            {
                TooltipText = "Change User",
                MenuBackground = Global.Textures["circle-btn"],
                MenuIcon = Global.Textures["user-btn"],
                MenuFont = Global.Fonts["default"],
            };
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
