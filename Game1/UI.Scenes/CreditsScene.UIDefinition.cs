using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class CreditsScene
    {
        private StackPanel ScrollContainer;
        private ObservableDictionary<string, BaseElement> ScrollingElements
        {
            get { return ScrollContainer.Children; }
        }
        private MenuButton BackButton;

        private void InitializeComponent()
        {
            BackButton = new MenuButton("mb1");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = Global.Textures["back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new MainMenuScene());

            ScrollContainer = new StackPanel("container1")
            {
                AutoPosition = true,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Elements.Add(BackButton.Name, BackButton);
            Elements.Add(ScrollContainer.Name, ScrollContainer);
        }
    }
}
