using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface;
using Arkabound.Interface.Controls;
using Arkabound.Objects;

namespace Arkabound.Interface.Scenes
{
    public class GameThreeScene : SceneBase
    {
        public GameThreeScene(SceneManager sceneManager, Difficulty difficulty)
            : base(sceneManager, "Game 3 Scene: Dino-like Game")
        {
            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("dino/gameBG3"),
                    DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                }},
                { "BackButton", new MenuButton("mb", sceneManager)
                {
                    Text = "Back",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                }},
                { "ObjectCatcher", new Image("ObjectCatcher")
                {
                    Graphic = game.Content.Load<Texture2D>("dino/character"),
                    Location = new Vector2(5, game.GraphicsDevice.Viewport.Height - 70),
                    AlignToCenter = false,
                    SpriteType = SpriteTypes.Animated,
                    Rows = 2,
                    Columns = 8,
                    spriteBatch = this.spriteBatch,
                }},
                { "Timer", new Label("timer")
                {
                    Text = String.Format("{0} second(s) left", timeLeft),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5), //new Vector2(300, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_l"]
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 15.0;
                    projectileInterval = 1500;
                    FallingSpeed = 3;
                    JumpHeight = -15;
                    break;
                case Difficulty.Medium:
                    timeLeft = 10.0;
                    projectileInterval = 1000;
                    FallingSpeed = 3;
                    JumpHeight = -15;
                    break;
                case Difficulty.Hard:
                    timeLeft = 5.0;
                    projectileInterval = 800;
                    FallingSpeed = 5;
                    JumpHeight = -10;
                    break;
                case Difficulty.EpicFail:
                    timeLeft = 60000 * 5;
                    projectileInterval = 700;
                    FallingSpeed = 10;
                    JumpHeight = -20;
                    break;
            }
            DistanceFromBottom = -30;
            this.difficulty = difficulty;
            InitializeTimer();
            UpdateMinMaxY();
            GenerateFallingCrap();
            PlayerPosition = new Vector2(100, MaxYPos);
        }

        private MouseOverlay MsOverlay;
        private List<ObjectBase> GameObjects = new List<ObjectBase>();
        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();

        private double timeLeft;
        private int projectileInterval;
        private float FallingSpeed;
        private int DistanceFromBottom;
        private float JumpHeight;

        private Difficulty difficulty;

        Timer ProjectileGenerator;
        Timer TimeLeftController;
        Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(projectileInterval) { AutoReset = true, Enabled = true };
            TimeLeftController = new Timer(1000) { AutoReset = true, Enabled = true };
            GameTimer = new Timer(timeLeft * 1000) { AutoReset = false, Enabled = true };
            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += delegate
            {
                GenerateFallingCrap();
            };
            TimeLeftController.Elapsed += delegate
            {
                if (timeLeft >= 1)
                    timeLeft -= 1;
            };
            GameTimer.Elapsed += GameTimer_Elapsed;
        }

        void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            stopCreatingCrap = true;
            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.RunningForTheirLives, null, this));
        }

        public override void Unload()
        {
            // Close all timers
            ProjectileGenerator.Close();
            TimeLeftController.Close();
            GameTimer.Close();

            base.Unload();
        }

        public int GameOverReason = 0;
        private bool stopCreatingCrap = false;
        Random randNum = new Random();
        private void GenerateFallingCrap()
        {
            if (!stopCreatingCrap)
            {
                // create new button object
                Image nwBtn = new Image("crap")
                {
                    Graphic = game.Content.Load<Texture2D>("dino/fire"),
                    //Location = new Vector2((float)randNum.Next(5, (int)game.GraphicsDevice.Viewport.Width - 5), 30),
                    Location = new Vector2((float)randNum.Next(StartingXPos - 100, StartingXPos), MaxYPos),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                };
                GameObjects.Add(nwBtn);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);

            Label a = (Label)Objects["Timer"];
            a.Text = String.Format("{0} second(s) left", timeLeft);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            spriteBatch.End();
        }

        void UpdateMinMaxY()
        {
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                MinYPos = game.GraphicsDevice.Viewport.Height - Objects["ObjectCatcher"].Bounds.Height - DistanceFromBottom - 300;
                MaxYPos = game.GraphicsDevice.Viewport.Height - Objects["ObjectCatcher"].Bounds.Height - DistanceFromBottom - 100;
                StartingXPos = game.GraphicsDevice.Viewport.Width - Objects["ObjectCatcher"].Bounds.Width - 10;
            }
        }
        Vector2 PlayerPosition;
        int MinYPos;
        int MaxYPos;
        int StartingXPos;
        float JumpSpeed = 0;
        bool IsJumping = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Timer
            Label Timer = (Label)Objects["Timer"];
            Timer.Location = new Vector2(game.GraphicsDevice.Viewport.Width - Timer.Font.MeasureString(Timer.Text).X, 5);
            // Backrgound Resize
            Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            // base updates
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
            UpdateMinMaxY();
            if (IsJumping)
            {
                PlayerPosition.Y += JumpSpeed;//Making it go up
                JumpSpeed += 0.5f;//Some math (explained later)
                if (PlayerPosition.Y >= MaxYPos)
                //If it's farther than ground
                {
                    PlayerPosition.Y = MaxYPos;//Then set it on
                    IsJumping = false;
                }
            }
            else
            {
                if (KeybdState.IsKeyDown(Keys.Space) || MsState.LeftButton == ButtonState.Pressed ||
                    MsState.RightButton == ButtonState.Pressed || MsState.MiddleButton == ButtonState.Pressed)
                {
                    IsJumping = true;
                    JumpSpeed = JumpHeight;//Give it upward thrust
                }
            }
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                if (stopCreatingCrap)
                    Objects.Remove("ObjectCatcher");
                else
                    Objects["ObjectCatcher"].Location = PlayerPosition;
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Move crap by 3
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X - FallingSpeed, GameObjects[i].Location.Y);

                // Check if game object collides/intersects with catcher
                if (Objects.ContainsKey("ObjectCatcher") && Objects["ObjectCatcher"].Bounds.Contains(GameObjects[i].Bounds.Center))
                {
                    GameOverReason = -1;
                    GameTimer_Elapsed(null, null);
                    GameObjects.Remove(GameObjects[i]);
                    return;
                }

                // cleanup crapped shit
                // (normal human speak: remove objects once it exceeds the object catcher)
                // also remove all crap once time is up
                if ((Objects.ContainsKey("ObjectCatcher") && (GameObjects[i].Location.X < Objects["ObjectCatcher"].Location.X - 50)) || stopCreatingCrap)
                    GameObjects.Remove(GameObjects[i]);
            }
        }
    }
}
