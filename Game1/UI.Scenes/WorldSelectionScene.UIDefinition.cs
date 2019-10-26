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
            BackButton = new MenuButton("mb1")
            {
                TooltipText = "Back",
                MenuBackground = Global.Textures["back-btn"],
                Location = new Point(5, 5),
            };
            BackButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new MainMenuScene());

            // Main container elements
            DifficultyButton = new MenuButton("mb2")
            {
                TooltipText = "Change the game's difficulty",
                MenuLabel = string.Format("Difficulty: {0}", difficulty),
            };
            DifficultyButton.OnLeftClick += (sender, e) => ModifyDifficulty();

            RandomGameButton = new MenuButton("mb9")
            {
                TooltipText = "Random Game",
                MenuBackgroundSpriteType = SpriteType.None,
                MenuBackground = Global.Textures["htp-dice"],
            };
            RandomGameButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene());

            // Row1 container elements
            Game1Button = new MenuButton("mb3")
            {
                MenuBackground = Global.Textures["worldselection-one"],
                MenuBackgroundColumns = 2,
                MenuBackgroundRows = 1,
                TooltipText = "The Safety Kit",
                Scale = 0.7f,
            };
            Game1Button.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.FallingObjects, difficulty));

            Game2AButton = new MenuButton("mb4")
            {
                MenuBackground = Global.Textures["worldselection-two"],
                MenuBackgroundColumns = 2,
                MenuBackgroundRows = 1,
                TooltipText = "Earthquake Escape",
                Scale = 0.7f,
            };
            Game2AButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.EscapeEarthquake, difficulty));

            Game2BButton = new MenuButton("mb5")
            {
                MenuBackground = Global.Textures["worldselection-three"],
                MenuBackgroundColumns = 2,
                MenuBackgroundRows = 1,
                TooltipText = "Fire Escape",
                Scale = 0.7f,
            };
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
            Game3Button = new MenuButton("mb6")
            {
                MenuBackground = Global.Textures["worldselection-four"],
                MenuBackgroundColumns = 2,
                MenuBackgroundRows = 1,
                TooltipText = "Safety Jump - Fire",
                Scale = 0.7f,
            };
            Game3Button.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new NextGameScene(Games.RunningForTheirLives, difficulty));

            Game4Button = new MenuButton("mb7")
            {
                MenuBackground = Global.Textures["worldselection-five"],
                MenuBackgroundColumns = 2,
                MenuBackgroundRows = 1,
                TooltipText = "Aid 'Em - Earthquake",
                Scale = 0.7f,
            };
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
