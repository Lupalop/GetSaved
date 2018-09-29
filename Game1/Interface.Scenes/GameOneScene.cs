using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Interface;
using Maquina.Interface.Controls;
using Maquina.Objects;

namespace Maquina.Interface.Scenes
{
    public class GameOneScene : SceneBase
    {
        public GameOneScene(SceneManager sceneManager, Difficulty Difficulty)
            : base(sceneManager, "Game 1 Scene: The Safety Kit")
        {
            GameDifficulty = Difficulty;
        }

        private List<string> FallingObjects = new List<string> {
			"Medkit", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "!Car",
			"!Donut", "!Shoes", "!Jewelry", "!Ball", "!Wall Clock", "!Chair", "!Bomb"
			};
        private MouseOverlay MsOverlay;
        private List<ObjectBase> GameObjects = new List<ObjectBase>();
        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();
        private Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();

        private double _InitialTimeLeft;
        private double InitialTimeLeft
        {
            get
            {
                return _InitialTimeLeft;
            }
            set
            {
                _InitialTimeLeft = value;
                TimeLeft = value;
                var a = (ProgressBar)Objects["ProgressBar"];
                a.maximum = (float)value;
            }
        }

        private double TimeLeft;
        private int GenerationInterval;
        private float FallingSpeed;
        private int DistanceFromBottom;
        private bool IsGameEnd = false;

        private Random RandNum = new Random();
        private Difficulty GameDifficulty;

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;
        private Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(GenerationInterval) { AutoReset = true, Enabled = true };
            TimeLeftController = new Timer(1000)                { AutoReset = true, Enabled = true };
            GameTimer = new Timer(TimeLeft * 1000)              { AutoReset = false, Enabled = true };
            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += delegate
            { 
                GenerateFallingItems();
            };
            TimeLeftController.Elapsed += delegate
            {
                if (TimeLeft > 0)
                    TimeLeft--;
            };
            GameTimer.Elapsed += delegate
            {
                IsGameEnd = true;
                sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.FallingObjects, CollectedObjects, this));
            };
        }

        private void GenerateFallingItems()
        {
            if (!IsGameEnd)
            {
                // create new button object
                Image nwBtn = new Image("crap")
                {
                    Graphic = game.Content.Load<Texture2D>("point"),
                    Location = new Vector2((float)RandNum.Next(5, (int)game.GraphicsDevice.Viewport.Width - 5), 30),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                };
                string tex = FallingObjects[RandNum.Next(0, FallingObjects.Count)];
                nwBtn.MessageHolder.Add(tex);
                if (tex.Contains('!') || tex.Contains('~')) tex = tex.Remove(0, 1);
                nwBtn.Graphic = Images[tex.ToLower()];
                GameObjects.Add(nwBtn);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            foreach (var item in FallingObjects)
            {
                string it = item;
                if (it.Contains('!') || it.Contains('~'))
                    it = it.Remove(0, 1);
                Images.Add(it.ToLower(), game.Content.Load<Texture2D>("falling-object/" + it));
            }

            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("gameBG1"),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height)
                }},
                { "ProgressBar", new ProgressBar("ProgressBar", new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, 32), sceneManager)
                {
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => {
                        var a = (ProgressBar)Objects["ProgressBar"];
                        a.value = (float)TimeLeft;
                    }
                }},
                { "BackButton", new MenuButton("mb", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
                { "ObjectCatcher", new Image("ObjectCatcher")
                {
                    Graphic = game.Content.Load<Texture2D>("falling-object/briefcase"),
                    Location = new Vector2(5, game.GraphicsDevice.Viewport.Height - 70),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "Timer", new Label("o-timer")
                {
                    Text = String.Format("{0} second(s) left", TimeLeft),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["o-default_l"],
                    OnUpdate = () => {
                        Label Timer = (Label)Objects["Timer"];
                        Timer.Location = new Vector2(game.GraphicsDevice.Viewport.Width - Timer.Font.MeasureString(Timer.Text).X, 5);
                        Timer.Text = String.Format("{0} second(s) left", MathHelper.Clamp((int)TimeLeft, 0, 100));
                    }
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            DistanceFromBottom = -30;
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    InitialTimeLeft = 25.0;
                    GenerationInterval = 500;
                    FallingSpeed = 3;
                    break;
                case Difficulty.Medium:
                    InitialTimeLeft = 20.0;
                    GenerationInterval = 300;
                    FallingSpeed = 3;
                    break;
                case Difficulty.Hard:
                    InitialTimeLeft = 15.0;
                    GenerationInterval = 200;
                    FallingSpeed = 5;
                    break;
                case Difficulty.EpicFail:
                    InitialTimeLeft = 10.0;
                    GenerationInterval = 50;
                    FallingSpeed = 10;
                    break;
            }

            InitializeTimer();
        }

        public override void Unload()
        {
            // Close all timers
            ProjectileGenerator.Close();
            TimeLeftController.Close();
            GameTimer.Close();

            base.Unload();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                if (IsGameEnd)
                    Objects.Remove("ObjectCatcher");
                else
                    Objects["ObjectCatcher"].Location = new Vector2(MsOverlay.Bounds.X - (Objects["ObjectCatcher"].Graphic.Width / 2), game.GraphicsDevice.Viewport.Height - Objects["ObjectCatcher"].Bounds.Height + DistanceFromBottom);
            }
            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Moves game object
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X, GameObjects[i].Location.Y + FallingSpeed);

                // Check if game object collides/intersects with catcher
                if (Objects.ContainsKey("ObjectCatcher") && Objects["ObjectCatcher"].Bounds.Intersects(GameObjects[i].Bounds))
                {
                    CollectedObjects.Add(GameObjects[i]);
                    GameObjects.Remove(GameObjects[i]);
                    return;
                }

                // Remove objects once it exceeds the object catcher, this also removes all objects when time's up
                if ((Objects.ContainsKey("ObjectCatcher") && (GameObjects[i].Location.Y > Objects["ObjectCatcher"].Location.Y + 50)) || IsGameEnd)
                    GameObjects.Remove(GameObjects[i]);
            }
        }
    }
}
