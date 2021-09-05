using Maquina.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class WorldSelectionScene
    {
        private Button BackButton;
        //
        private StackPanel MainContainer;
        //
        private Button DifficultyButton;
        private StackPanel Row1Container;
        private Button Game1Button;
        private Button Game2AButton;
        private Button Game2BButton;
        private StackPanel Row2Container;
        private Button Game3Button;
        private Button Game4Button;
        //
        private Button RandomGameButton;

        private void InitializeComponent()
        {
            // Outer elements (no container)
            BackButton = new Button("mb1");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new MainMenuScene());

            // Main container elements
            DifficultyButton = new Button("mb2");
            DifficultyButton.Tooltip.Text = "Change the game's difficulty";
            DifficultyButton.Label.Text = string.Format("Difficulty: {0}", difficulty);
            DifficultyButton.OnLeftClick += (sender, e) => ModifyDifficulty();

            RandomGameButton = new Button("mb9");
            RandomGameButton.Tooltip.Text = "Random Game";
            RandomGameButton.Background.SpriteType = SpriteType.None;
            RandomGameButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("htp-dice"];
            RandomGameButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new NextGameScene());

            // Row1 container elements
            Game1Button = new Button("mb3");
            Game1Button.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("worldselection-one"];
            Game1Button.Background.Columns = 2;
            Game1Button.Background.Rows = 1;
            Game1Button.Tooltip.Text = "The Safety Kit";
            Game1Button.Scale = 0.7f;
            Game1Button.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new NextGameScene(Games.FallingObjects, difficulty));

            Game2AButton = new Button("mb4");
            Game2AButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("worldselection-two"];
            Game2AButton.Background.Columns = 2;
            Game2AButton.Background.Rows = 1;
            Game2AButton.Tooltip.Text = "Earthquake Escape";
            Game2AButton.Scale = 0.7f;
            Game2AButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new NextGameScene(Games.EscapeEarthquake, difficulty));

            Game2BButton = new Button("mb5");
            Game2BButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("worldselection-three"];
            Game2BButton.Background.Columns = 2;
            Game2BButton.Background.Rows = 1;
            Game2BButton.Tooltip.Text = "Fire Escape";
            Game2BButton.Scale = 0.7f;
            Game2BButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new NextGameScene(Games.EscapeFire, difficulty));

            Row1Container = new StackPanel("cr1")
            {
                ControlMargin = new Margin(0, 5, 0, 0),
                Orientation = Orientation.Horizontal,
                Children =
                {
                    Game1Button,
                    Game2AButton,
                    Game2BButton,
                }
            };

            // Row2 container elements
            Game3Button = new Button("mb6");
            Game3Button.BackgroundSprite = (TextureSprite)ContentFactory.TryGetResource("worldselection-four");
            Game3Button.Background.Columns = 2;
            Game3Button.Background.Rows = 1;
            Game3Button.Tooltip.Text = "Safety Jump - Fire";
            Game3Button.Scale = 0.7f;
            Game3Button.LeftMouseDown += (sender, e) => Application.Scenes.SwitchToScene(new NextGameScene(Games.RunningForTheirLives, difficulty));

            Game4Button = new Button("mb7");
            Game4Button.BackgroundSprite = (TextureSprite)ContentFactory.TryGetResource("worldselection-five");
            Game4Button.Background.Columns = 2;
            Game4Button.Background.Rows = 1;
            Game4Button.Tooltip.Text = "Aid 'Em - Earthquake";
            Game4Button.Scale = 0.7f;
            Game4Button.LeftMouseDown += (sender, e) => Application.Scenes.SwitchToScene(new NextGameScene(Games.HelpOthersNow, difficulty));

            Row2Container = new StackPanel("cr2")
            {
                ControlMargin = new Margin(0, 5, 0, 0),
                Orientation = Orientation.Horizontal,
                Children = 
                {
                    Game3Button,
                    Game4Button,
                }
            };

            // Main container
            MainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                ControlMargin = new Margin(0, 0, 5, 0),
                Children =
                {
                    DifficultyButton,
                    Row1Container,
                    Row2Container,
                    RandomGameButton,
                }
            };

            Entities.Add(BackButton);
            Entities.Add(MainContainer);
        }
    }
}
