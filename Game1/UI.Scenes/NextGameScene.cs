using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;
using System.Timers;

namespace Maquina.UI.Scenes
{
    public class NextGameScene : SceneBase
    {
        public NextGameScene(SceneManager SceneManager)
            : base(SceneManager, "Next Game Scene")
        {
            Initialize();
        }

        public NextGameScene(SceneManager SceneManager, Games passedGame)
            : base(SceneManager, "Next Game Scene")
        {
            ForcePassedGame = passedGame;
            RandomizeGame = false;
            Initialize();
        }

        public NextGameScene(SceneManager SceneManager, Games passedGame, Difficulty passedDifficulty)
            : base(SceneManager, "Next Game Scene")
        {
            ForcePassedGame = passedGame;
            ForcePassedDifficulty = passedDifficulty;
            RandomizeGame = false;
            RandomizeDifficulty = false;
            Initialize();
        }

        public void Initialize()
        {
            NextGame = DetermineNextGame();

            Objects = new Dictionary<string, GenericElement> {
                { "Dice", new Image("dice")
                {
                    Graphic = Game.Content.Load<Texture2D>("htp/dice"),
                    Location = ScreenCenter,
                    Tint = new Color(Color.White, 0),
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        GenericElement Dice = Objects["Dice"];
                        Dice.RotationOrigin = new Vector2(Dice.Graphic.Width / 2, Dice.Graphic.Height / 2);
                        Dice.Location = new Vector2(Dice.Location.X + (Dice.Bounds.Width / 2), Dice.Location.Y + (Dice.Bounds.Height / 2));
                    }
                }},
                { "GameName", new Label("GameName")
                {
                    Text = NextGame.SceneName.Substring(14),
                    SpriteBatch = this.SpriteBatch, 
                    Font = Fonts["default_l"]
                }},
                { "GameDifficulty", new Label("GameDifficulty")
                {
                    Text = String.Format("Difficulty: {0}", GameDifficulty.ToString()),
                    SpriteBatch = this.SpriteBatch, 
                    Font = Fonts["default_m"]
                }},
                { "SkipBtn", new MenuButton("skipBtn", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("overlayBG"),
                    Tint = Color.Transparent,
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.None,
                    OnUpdate = () => {
                        Rectangle SrcRectSkipBtn = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        Objects["SkipBtn"].DestinationRectangle = SrcRectSkipBtn;
                        Objects["SkipBtn"].SourceRectangle = SrcRectSkipBtn;
                    },
                    LeftClickAction = () => SceneManager.SwitchToScene(NextGame),
                    RightClickAction = () => SceneManager.SwitchToScene(NextGame)
                }},
                { "HelpImage", new Image("htp")
                {
                    Graphic = HelpImage,
                    Location = ScreenCenter,
                    SpriteBatch = this.SpriteBatch
                }}
            };

            DiceSpinner.Elapsed += delegate { Objects["Dice"].Rotation += .05f; };
            SceneChanger.Elapsed += delegate { SceneManager.SwitchToScene(NextGame); };
        }

        private Texture2D HelpImage;
        // Dice spinner runs every 1ms;
        private Timer DiceSpinner = new Timer(1) { AutoReset = true, Enabled = true };
        // Switch to new Game scene will happen in 3 seconds (3000ms)
        private Timer SceneChanger = new Timer(3000) { AutoReset = false, Enabled = true };

        public bool RandomizeGame = true;
        public bool RandomizeDifficulty = true;
        public Games ForcePassedGame { get; set; }
        public Difficulty ForcePassedDifficulty { get; set; }
        public SceneBase NextGame { get; set; }
        public Difficulty GameDifficulty { get; set; }

        public SceneBase DetermineNextGame()
        {
            // GameDifficulty would remain random
            Random rand = new Random();
            if (RandomizeDifficulty)
                GameDifficulty = (Difficulty)rand.Next(0, 3);       // Epic fail GameDifficulty intentionally ommitted, people can't handle that ;)
            else
                GameDifficulty = ForcePassedDifficulty;

            // Choose whether to randomize Game or use the passed Game
            Games NxGame;
            if (RandomizeGame)
                NxGame = (Games)rand.Next(0, 4);
            else
                NxGame = ForcePassedGame;

            switch (NxGame)
            {
                // The Safety Kit
                case Games.FallingObjects:
                    HelpImage = Game.Content.Load<Texture2D>("htp/fallingobject");
                    return new GameOneScene(SceneManager, GameDifficulty);
                // Earthquake Escape
                case Games.EscapeEarthquake:
                    HelpImage = Game.Content.Load<Texture2D>("htp/esc");
                    return new GameTwoScene(SceneManager, GameDifficulty, Games.EscapeEarthquake);
                // Fire Escape
                case Games.EscapeFire:
                    HelpImage = Game.Content.Load<Texture2D>("htp/esc");
                    return new GameTwoScene(SceneManager, GameDifficulty, Games.EscapeFire);
                // Safety Jump
                case Games.RunningForTheirLives:
                    HelpImage = Game.Content.Load<Texture2D>("htp/dino");
                    return new GameThreeScene(SceneManager, GameDifficulty);
                // Aid 'Em
                case Games.HelpOthersNow:
                    HelpImage = Game.Content.Load<Texture2D>("htp/aidem");
                    return new GameFourScene(SceneManager, GameDifficulty);
                // If the randomizer item failed, simply throw the world selection screen...
                default:
                    HelpImage = new Texture2D(Game.GraphicsDevice, 0, 0);
                    return new WorldSelectionScene(SceneManager);
            }
        }

        public override void Unload()
        {
            base.Unload();
            DiceSpinner.Close();
            SceneChanger.Close();
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin();
            base.Draw(GameTime);
            base.DrawObjects(GameTime, Objects);
            SpriteBatch.End();
        }
        
        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateObjects(GameTime, Objects);
        }
    }
}
