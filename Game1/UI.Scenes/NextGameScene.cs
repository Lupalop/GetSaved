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
        public NextGameScene(Games passedGame = Games.Random, Difficulty passedDifficulty = Difficulty.Random)
            : base("Next Game Scene")
        {
            NextGame = passedGame;
            GameDifficulty = passedDifficulty;
            Initialize();
        }

        public void Initialize()
        {
            NewGameScene = DetermineNewGame();

            Objects = new Dictionary<string, GenericElement> {
                { "Dice", new Image("dice")
                {
                    Graphic = Game.Content.Load<Texture2D>("htp/dice"),
                    Location = ScreenCenter,
                    Tint = new Color(Color.White, 0),
                    OnUpdate = () => {
                        GenericElement Dice = Objects["Dice"];
                        Dice.RotationOrigin = new Vector2(Dice.Graphic.Width / 2, Dice.Graphic.Height / 2);
                        Dice.Location = new Vector2(Dice.Location.X + (Dice.Bounds.Width / 2), Dice.Location.Y + (Dice.Bounds.Height / 2));
                    }
                }},
                { "GameName", new Label("GameName")
                {
                    Text = NewGameScene.SceneName.Substring(14),
                    Font = Fonts["default_l"]
                }},
                { "GameDifficulty", new Label("GameDifficulty")
                {
                    Text = String.Format("Difficulty: {0}", GameDifficulty.ToString()),
                    Font = Fonts["default_m"]
                }},
                { "SkipBtn", new MenuButton("skipBtn", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("overlayBG"),
                    Tint = Color.Transparent,
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteType = SpriteType.None,
                    OnUpdate = () => {
                        Rectangle SrcRectSkipBtn = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        Objects["SkipBtn"].DestinationRectangle = SrcRectSkipBtn;
                        Objects["SkipBtn"].SourceRectangle = SrcRectSkipBtn;
                    },
                    LeftClickAction = () => SceneManager.SwitchToScene(NewGameScene),
                    RightClickAction = () => SceneManager.SwitchToScene(NewGameScene)
                }},
                { "HelpImage", new Image("htp")
                {
                    Graphic = HelpImage,
                    Location = ScreenCenter,
                }}
            };

            DiceSpinner.Elapsed += delegate { Objects["Dice"].Rotation += .05f; };
            SceneChanger.Elapsed += delegate { SceneManager.SwitchToScene(NewGameScene); };
        }

        private Texture2D HelpImage;
        // Dice spinner runs every 1ms;
        private Timer DiceSpinner = new Timer(1) { AutoReset = true, Enabled = true };
        // Switch to new Game scene will happen in 3 seconds (3000ms)
        private Timer SceneChanger = new Timer(3000) { AutoReset = false, Enabled = true };

        public Games NextGame { get; set; }
        public SceneBase NewGameScene { get; set; }
        public Difficulty GameDifficulty { get; set; }

        public SceneBase DetermineNewGame()
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
                // The Safety Kit
                case Games.FallingObjects:
                    HelpImage = Game.Content.Load<Texture2D>("htp/fallingobject");
                    return new GameOneScene(GameDifficulty);
                // Earthquake Escape
                case Games.EscapeEarthquake:
                    HelpImage = Game.Content.Load<Texture2D>("htp/esc");
                    return new GameTwoScene(GameDifficulty, Games.EscapeEarthquake);
                // Fire Escape
                case Games.EscapeFire:
                    HelpImage = Game.Content.Load<Texture2D>("htp/esc");
                    return new GameTwoScene(GameDifficulty, Games.EscapeFire);
                // Safety Jump
                case Games.RunningForTheirLives:
                    HelpImage = Game.Content.Load<Texture2D>("htp/dino");
                    return new GameThreeScene(GameDifficulty);
                // Aid 'Em
                case Games.HelpOthersNow:
                    HelpImage = Game.Content.Load<Texture2D>("htp/aidem");
                    return new GameFourScene(GameDifficulty);
                // If the randomizer item failed, simply throw the world selection screen...
                default:
                    HelpImage = new Texture2D(Game.GraphicsDevice, 0, 0);
                    return new WorldSelectionScene();
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
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
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
