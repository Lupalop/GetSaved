using Maquina.Elements;
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
        private MenuButton BackButton;
        //
        private StackPanel MainContainer;
        //
        private MenuButton DifficultyButton;
        private StackPanel Row1Container;
        private MenuButton Game1Button;
        private MenuButton Game2AButton;
        private MenuButton Game2BButton;
        private StackPanel Row2Container;
        private MenuButton Game3Button;
        private MenuButton Game4Button;
        //
        private MenuButton RandomGameButton;

        private void InitializeComponent()
        {
            // Outer elements (no container)
            BackButton = new MenuButton("mb1");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = Global.Textures["back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new MainMenuScene());

            // Main container elements
            DifficultyButton = new MenuButton("mb2");
            DifficultyButton.Tooltip.Text = "Change the game's difficulty";
            DifficultyButton.Label.Text = string.Format("Difficulty: {0}", difficulty);
            DifficultyButton.OnLeftClick += (sender, e) => ModifyDifficulty();

            RandomGameButton = new MenuButton("mb9");
            RandomGameButton.Tooltip.Text = "Random Game";
            RandomGameButton.Background.SpriteType = SpriteType.None;
            RandomGameButton.Background.Graphic = Global.Textures["htp-dice"];
            RandomGameButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene());

            // Row1 container elements
            Game1Button = new MenuButton("mb3");
            Game1Button.Background.Graphic = Global.Textures["worldselection-one"];
            Game1Button.Background.Columns = 2;
            Game1Button.Background.Rows = 1;
            Game1Button.Tooltip.Text = "The Safety Kit";
            Game1Button.Scale = 0.7f;
            Game1Button.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.FallingObjects, difficulty));

            Game2AButton = new MenuButton("mb4");
            Game2AButton.Background.Graphic = Global.Textures["worldselection-two"];
            Game2AButton.Background.Columns = 2;
            Game2AButton.Background.Rows = 1;
            Game2AButton.Tooltip.Text = "Earthquake Escape";
            Game2AButton.Scale = 0.7f;
            Game2AButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.EscapeEarthquake, difficulty));

            Game2BButton = new MenuButton("mb5");
            Game2BButton.Background.Graphic = Global.Textures["worldselection-three"];
            Game2BButton.Background.Columns = 2;
            Game2BButton.Background.Rows = 1;
            Game2BButton.Tooltip.Text = "Fire Escape";
            Game2BButton.Scale = 0.7f;
            Game2BButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.EscapeFire, difficulty));

            Row1Container = new StackPanel("cr1")
            {
                ElementMargin = new Region(0, 5, 0, 0),
                Orientation = Orientation.Horizontal,
                Children =
                {
                    { Game1Button.Name, Game1Button },
                    { Game2AButton.Name, Game2AButton },
                    { Game2BButton.Name, Game2BButton },
                }
            };

            // Row2 container elements
            Game3Button = new MenuButton("mb6");
            Game3Button.Background.Graphic = Global.Textures["worldselection-four"];
            Game3Button.Background.Columns = 2;
            Game3Button.Background.Rows = 1;
            Game3Button.Tooltip.Text = "Safety Jump - Fire";
            Game3Button.Scale = 0.7f;
            Game3Button.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.RunningForTheirLives, difficulty));

            Game4Button = new MenuButton("mb7");
            Game4Button.Background.Graphic = Global.Textures["worldselection-five"];
            Game4Button.Background.Columns = 2;
            Game4Button.Background.Rows = 1;
            Game4Button.Tooltip.Text = "Aid 'Em - Earthquake";
            Game4Button.Scale = 0.7f;
            Game4Button.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.HelpOthersNow, difficulty));

            Row2Container = new StackPanel("cr2")
            {
                ElementMargin = new Region(0, 5, 0, 0),
                Orientation = Orientation.Horizontal,
                Children = 
                {
                    { Game3Button.Name, Game3Button },
                    { Game4Button.Name, Game4Button },
                }
            };

            // Main container
            MainContainer = new StackPanel("mainContainer")
            {
                AutoPosition = true,
                ElementMargin = new Region(0, 0, 5, 0),
                Children =
                {
                    { DifficultyButton.Name, DifficultyButton },
                    { Row1Container.Name, Row1Container },
                    { Row2Container.Name, Row2Container },
                    { RandomGameButton.Name, RandomGameButton },
                }
            };

            Elements.Add(BackButton.Name, BackButton);
            Elements.Add(MainContainer.Name, MainContainer);
        }
    }
}
