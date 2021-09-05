using Maquina.Entities;
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
        private ObservableDictionary<string, Entity> ScrollingElements
        {
            get { return ScrollContainer.Children; }
        }
        private Button BackButton;

        private void InitializeComponent()
        {
            BackButton = new Button("mb1");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new MainMenuScene());

            ScrollContainer = new StackPanel("container1")
            {
                AutoPosition = true,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Entities.Add(BackButton.Name, BackButton);
            Entities.Add(ScrollContainer.Name, ScrollContainer);
        }
    }
}
