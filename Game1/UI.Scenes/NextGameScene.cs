using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

namespace Maquina.UI.Scenes
{
    public partial class NextGameScene : Scene
    {
        public NextGameScene(Games passedGame = Games.Random, Difficulty passedDifficulty = Difficulty.Random)
            : base("Next Game Scene")
        {
            NextGame = passedGame;
            GameDifficulty = passedDifficulty;
        }

        public override void LoadContent()
        {
            InitializeComponent();
            NewGameScene = DetermineNewGame();
            base.LoadContent();
        }

        public Games NextGame { get; set; }
        public Scene NewGameScene { get; set; }
        public Difficulty GameDifficulty { get; set; }

        public Scene DetermineNewGame()
        {
            Random rand = new Random();
            if (GameDifficulty == Difficulty.Random)
            {
                // Epic fail difficulty intentionally ommitted, people can't handle that ;)
                GameDifficulty = (Difficulty)rand.Next(0, 3);
            }
            if (NextGame == Games.Random)
            {
                NextGame = (Games)rand.Next(0, 5);
            }
            switch (NextGame)
            {
                // FIXME: Other game scenes temporarily disabled
                // The Safety Kit
                case Games.FallingObjects:
                    EgsImage.Graphic = Global.Textures["egs1"];
                    HelpImage.Graphic = Global.Textures["htp-fallingobject"];
                    GameNameLabel.Text = "The Safety Kit";
                    return new GameOneScene(GameDifficulty);
                // Earthquake Escape
                case Games.EscapeEarthquake:
                    EgsImage.Graphic = Global.Textures["egs1"];
                    HelpImage.Graphic = Global.Textures["htp-esc"];
                    GameNameLabel.Text = "Earthquake Escape";
                    return new GameOneScene(GameDifficulty);
                    //return new GameTwoScene(GameDifficulty, Games.EscapeEarthquake);
                // Fire Escape
                case Games.EscapeFire:
                    EgsImage.Graphic = Global.Textures["egs2"];
                    HelpImage.Graphic = Global.Textures["htp-esc"];
                    GameNameLabel.Text = "Fire Escape";
                    return new GameOneScene(GameDifficulty);
                    //return new GameTwoScene(GameDifficulty, Games.EscapeFire);
                // Safety Jump
                case Games.RunningForTheirLives:
                    EgsImage.Graphic = Global.Textures["egs2"];
                    HelpImage.Graphic = Global.Textures["htp-dino"];
                    GameNameLabel.Text = "Safety Jump";
                    return new GameOneScene(GameDifficulty);
                    //return new GameThreeScene(GameDifficulty);
                // Aid 'Em
                case Games.HelpOthersNow:
                    EgsImage.Graphic = Global.Textures["egs1"];
                    HelpImage.Graphic = Global.Textures["htp-aidem"];
                    GameNameLabel.Text = "Aid 'Em";
                    return new GameOneScene(GameDifficulty);
                    //return new GameFourScene(GameDifficulty);
                // If the randomizer item failed, simply throw the world selection screen...
                default:
                    EgsImage.Graphic = new Texture2D(Game.GraphicsDevice, 1, 1);
                    HelpImage.Graphic = new Texture2D(Game.GraphicsDevice, 1, 1);
                    return new WorldSelectionScene();
            }
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(GameTime, Elements);
            SpriteBatch.End();
        }
        
        public override void Update(GameTime GameTime)
        {
            GuiUtils.UpdateElements(GameTime, Elements);
        }
    }
}
