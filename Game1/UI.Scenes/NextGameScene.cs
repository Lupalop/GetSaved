using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

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
                    EgsImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("egs1"];
                    HelpImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("htp-fallingobject"];
                    GameNameLabel.Sprite.Text = "The Safety Kit";
                    return new GameOneScene(GameDifficulty);
                // Earthquake Escape
                case Games.EscapeEarthquake:
                    EgsImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("egs1"];
                    HelpImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("htp-esc"];
                    GameNameLabel.Sprite.Text = "Earthquake Escape";
                    return new GameTwoScene(GameDifficulty, Games.EscapeEarthquake);
                // Fire Escape
                case Games.EscapeFire:
                    EgsImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("egs2"];
                    HelpImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("htp-esc"];
                    GameNameLabel.Sprite.Text = "Fire Escape";
                    return new GameTwoScene(GameDifficulty, Games.EscapeFire);
                // Safety Jump
                case Games.RunningForTheirLives:
                    EgsImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("egs2"];
                    HelpImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("htp-dino"];
                    GameNameLabel.Sprite.Text = "Safety Jump";
                    return new GameThreeScene(GameDifficulty);
                // Aid 'Em
                case Games.HelpOthersNow:
                    EgsImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("egs1"];
                    HelpImage.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("htp-aidem"];
                    GameNameLabel.Sprite.Text = "Aid 'Em";
                    return new GameOneScene(GameDifficulty);
                    //return new GameFourScene(GameDifficulty);
                // If the randomizer item failed, simply throw the world selection screen...
                default:
                    EgsImage.Sprite.Graphic = new Texture2D(Game.GraphicsDevice, 1, 1);
                    HelpImage.Sprite.Graphic = new Texture2D(Game.GraphicsDevice, 1, 1);
                    return new WorldSelectionScene();
            }
        }

        public override void Draw()
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            SpriteBatch.End();
        }
        
        public override void Update()
        {
            GuiUtils.UpdateElements(Elements);
        }
    }
}
