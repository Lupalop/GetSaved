using Maquina.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class UserProfileScene
    {
        private StackPanel mainContainer;
        private Button mb1;
        private Label lb1;
        private Label lb2;
        private Label lb3;
        private TextBox tb1;
        private Button mb2;
        private Label lb4;

        private void InitializeComponent()
        {
            mb1 = new Button("mb1");
            mb1.Tooltip.Text = "Back";
            mb1.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("back-btn"];
            mb1.Location = new Point(5, 5);
            mb1.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new MainMenuScene());

            lb1 = new Label("lb1");
            lb1.Sprite.Text = string.Format("Are you {0}?", UserApplication.UserName);
            lb1.Sprite.Font = Application.Fonts["o-default_l"];

            lb2 = new Label("lb2");
            lb2.Sprite.Text = string.Format("You currently have {0} points!", UserApplication.Score);
            lb2.Sprite.Font = Application.Fonts["default_m"];

            lb3 = new Label("lb3");
            lb3.Sprite.Text = "If no, type your name at the box\n below and confirm.";
            lb3.Sprite.Font = Application.Fonts["default_m"];

            tb1 = new TextBox("tb1");
            tb1.OnInput += (sender, e) => lb4.Sprite.Tint = Color.Transparent;

            mb2 = new Button("mb2");
            mb2.Tooltip.Text = "Clicking here will clear your points\n and change the active user.";
            mb2.Label.Text = "Confirm and change user";
            mb2.OnLeftClick += (sender, e) =>
            {
                // Show the validation warning when textbox is left blank.
                if (tb1.Label.Text.Trim() == "")
                {
                    lb4.Sprite.Tint = Color.White;
                    return;
                }
                UserApplication.UserName = tb1.Label.Text;
                UserApplication.Score = 0;

                Application.Scenes.SwitchToScene(new MainMenuScene());
            };

            lb4 = new Label("lb4");
            lb4.Sprite.Text = "Leaving the name field blank is bad.";
            lb4.Sprite.Font = Application.Fonts["default"];
            lb4.Sprite.Tint = Color.Transparent;

            mainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                Children =
                {
                    { lb1.Name, lb1 },
                    { lb2.Name, lb2 },
                    { lb3.Name, lb3 },
                    { tb1.Name, tb1 },
                    { mb2.Name, mb2 },
                    { lb4.Name, lb4 },
                }
            };

            Entities.Add(mb1.Name, mb1);
            Entities.Add(mainContainer.Name, mainContainer);
        }
    }
}
