using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface.Controls;
using Arkabound.Objects;
using System.Timers;

namespace Arkabound.Interface.Scenes
{
    public class NextGameScene : SceneBase
    {
        public NextGameScene(SceneManager sceneManager)
            : base(sceneManager, "Next Game Scene")
        {
            Initialize();
        }

        public NextGameScene(SceneManager sceneManager, Games passedGame)
            : base(sceneManager, "Next Game Scene")
        {
            ForcePassedGame = passedGame;
            RandomizeGame = false;
            Initialize();
        }

        public NextGameScene(SceneManager sceneManager, Games passedGame, Difficulty passedDifficulty)
            : base(sceneManager, "Next Game Scene")
        {
            ForcePassedGame = passedGame;
            ForcePassedDifficulty = passedDifficulty;
            RandomizeGame = false;
            RandomizeDifficulty = false;
            Initialize();
        }
        Texture2D HelpImage;
        public void Initialize()
        {
            NextGame = DetermineNextGame();

            Objects = new Dictionary<string, ObjectBase> {
                { "Dice", new Image("dice")
                {
                    Graphic = game.Content.Load<Texture2D>("dice"),
                    Location = ScreenCenter,
                    Tint = new Color(Color.White, 0),
                    spriteBatch = this.spriteBatch
                }},
                { "GameName", new Label("gameName")
                {
                    Text = NextGame.sceneName.Substring(14),
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_l"]
                }},
                { "Difficulty", new Label("difficulty")
                {
                    Text = String.Format("Difficulty: {0}", GameDifficulty.ToString()),
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"]
                }},
                { "SkipBtn", new MenuButton("skipBtn", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("overlayBG"),
                    DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    Tint = Color.Transparent,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => { sceneManager.currentScene = NextGame; },
                    RightClickAction = () => { sceneManager.currentScene = NextGame; }
                }},
                { "HelpImage", new Image("htp")
                {
                    Graphic = HelpImage,
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch
                }}
            };

            DiceSpinner.Elapsed += delegate { Objects["Dice"].Rotation += .05f; };
            SceneChanger.Elapsed += delegate { sceneManager.currentScene = NextGame; };
        }

        private Timer DiceSpinner = new Timer(1) { AutoReset = true, Enabled = true };
        private Timer SceneChanger = new Timer(3000) { AutoReset = false, Enabled = true };

        public bool RandomizeGame = true;
        public bool RandomizeDifficulty = true;
        public Games ForcePassedGame { get; set; }
        public Difficulty ForcePassedDifficulty { get; set; }
        public SceneBase NextGame { get; set; }
        public Difficulty GameDifficulty { get; set; }

        public SceneBase DetermineNextGame()
        {
            // Difficulty would remain random
            Random rand = new Random();
            if (RandomizeDifficulty)
                GameDifficulty = (Difficulty)rand.Next(0, 3);       // Epic fail difficulty intentionally ommitted, people can't handle that ;)
            else
                GameDifficulty = ForcePassedDifficulty;

            // Choose whether to randomize game or use the passed game
            Games NxGame;
            if (RandomizeGame)
                NxGame = (Games)rand.Next(0, 4);
            else
                NxGame = ForcePassedGame;

            switch (NxGame)
            {
                // Falling Objects
                case Games.FallingObjects:
                    HelpImage = game.Content.Load<Texture2D>("htp-fallingobject");
                    return new GameOneScene(sceneManager, GameDifficulty);
                // Earthquake Escape
                case Games.EscapeEarthquake:
                    HelpImage = game.Content.Load<Texture2D>("htp-esc");
                    return new GameTwoScene(sceneManager, GameDifficulty, Games.EscapeEarthquake);
                // Fire Escape
                case Games.EscapeFire:
                    HelpImage = game.Content.Load<Texture2D>("htp-esc");
                    return new GameTwoScene(sceneManager, GameDifficulty, Games.EscapeFire);
                // Running for their lives
                case Games.RunningForTheirLives:
                    HelpImage = game.Content.Load<Texture2D>("htp-dino");
                    return new GameThreeScene(sceneManager, GameDifficulty);
                // Help others now
                case Games.HelpOthersNow:
                    HelpImage = game.Content.Load<Texture2D>("htp-aidem");
                    return new GameFourScene(sceneManager, GameDifficulty);
                // If the randomizer crap failed, simply throw the world selection screen...
                default:
                    HelpImage = new Texture2D(game.GraphicsDevice, 0, 0);
                    return new WorldSelectionScene(sceneManager);
            }
        }

        public override void Unload()
        {
            base.Unload();
            DiceSpinner.Close();
            SceneChanger.Close();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
            Rectangle SrcRectSkipBtn = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            Objects["SkipBtn"].DestinationRectangle = SrcRectSkipBtn;
            Objects["SkipBtn"].SourceRectangle = SrcRectSkipBtn;
            ObjectBase Dice = Objects["Dice"];
            Dice.RotationOrigin = new Vector2(Dice.Graphic.Width / 2, Dice.Graphic.Height / 2);
            Dice.Location = new Vector2(Dice.Location.X + (Dice.Bounds.Width / 2), Dice.Location.Y + (Dice.Bounds.Height / 2));
        }
    }
}
