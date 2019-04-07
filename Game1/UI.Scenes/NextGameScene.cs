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
                { "SkipBtn", new MenuButton("skipBtn")
                {
                    Graphic = Game.Content.Load<Texture2D>("overlayBG"),
                    Tint = Color.Transparent,
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteType = SpriteType.None,
                    OnUpdate = (element) => {
                        Rectangle SrcRectSkipBtn = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        element.DestinationRectangle = SrcRectSkipBtn;
                        element.SourceRectangle = SrcRectSkipBtn;
                    },
                    LeftClickAction = () => SceneManager.SwitchToScene(NewGameScene),
                    RightClickAction = () => SceneManager.SwitchToScene(NewGameScene)
                }},
                { "main-container", new ElementContainer("cr")
                {
                    ControlAlignment = ControlAlignment.Center,
                    ContainerAlignment = ContainerAlignment.Horizontal,
                    Children = {
                        { "Egs", new Image("egs")
                        {
                            Graphic = EgsImage,
                            Scale = 0.8f
                        }},
                        { "container2", new ElementContainer("cr")
                        {
                            ControlAlignment = Elements.ControlAlignment.Center,
                            ContainerAlignment = Elements.ContainerAlignment.Vertical,
                            Children = {
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
                                { "HelpImage", new Image("htp")
                                {
                                    Graphic = HelpImage,
                                    Location = ScreenCenter,
                                }},
                                { "ContinueMsg", new Label("label")
                                {
                                    Text = "Click or tap anywhere to continue.",
                                    Font = Fonts["o-default_m"],
                                }},
                            }
                        }},
                    }
                }},
            };
        }

        private Texture2D HelpImage;
        private Texture2D EgsImage;

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
                    EgsImage = Game.Content.Load<Texture2D>("egs1");
                    HelpImage = Game.Content.Load<Texture2D>("htp/fallingobject");
                    return new GameOneScene(GameDifficulty);
                // Earthquake Escape
                case Games.EscapeEarthquake:
                    EgsImage = Game.Content.Load<Texture2D>("egs1");
                    HelpImage = Game.Content.Load<Texture2D>("htp/esc");
                    return new GameTwoScene(GameDifficulty, Games.EscapeEarthquake);
                // Fire Escape
                case Games.EscapeFire:
                    EgsImage = Game.Content.Load<Texture2D>("egs2");
                    HelpImage = Game.Content.Load<Texture2D>("htp/esc");
                    return new GameTwoScene(GameDifficulty, Games.EscapeFire);
                // Safety Jump
                case Games.RunningForTheirLives:
                    EgsImage = Game.Content.Load<Texture2D>("egs2");
                    HelpImage = Game.Content.Load<Texture2D>("htp/dino");
                    return new GameThreeScene(GameDifficulty);
                // Aid 'Em
                case Games.HelpOthersNow:
                    EgsImage = Game.Content.Load<Texture2D>("egs1");
                    HelpImage = Game.Content.Load<Texture2D>("htp/aidem");
                    return new GameFourScene(GameDifficulty);
                // If the randomizer item failed, simply throw the world selection screen...
                default:
                    EgsImage = new Texture2D(Game.GraphicsDevice, 0, 0);
                    HelpImage = new Texture2D(Game.GraphicsDevice, 0, 0);
                    return new WorldSelectionScene();
            }
        }

        public override void Unload()
        {
            base.Unload();
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
