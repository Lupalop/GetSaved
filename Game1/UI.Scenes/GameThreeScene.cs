using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;
using Microsoft.Xna.Framework.Audio;

namespace Maquina.UI.Scenes
{
    public partial class GameThreeScene : Scene
    {
        public GameThreeScene(Difficulty Difficulty)
            : base("Game 3 Scene: Safety Jump")
        {
            GameDifficulty = Difficulty;
        }

        public double Score { private set; get; }
        private int ProjectileInterval;
        private float ObjectMovementSpeed;
        private float JumpHeight;
        private int ScoreMultiplier;

        private SoundEffect JumpEffect;

        private Timer ProjectileGenerator;
        private Timer ScoreTimer;

        private Difficulty gameDifficulty;
        public Difficulty GameDifficulty
        {
            get { return gameDifficulty; }
            set
            {
                gameDifficulty = value;
                switch (GameDifficulty)
                {
                    case Difficulty.Easy:
                        ProjectileInterval = 1500;
                        ObjectMovementSpeed = 3;
                        JumpHeight = -15;
                        ScoreMultiplier = 1;
                        break;
                    case Difficulty.Medium:
                        ProjectileInterval = 1000;
                        ObjectMovementSpeed = 3;
                        JumpHeight = -15;
                        ScoreMultiplier = 2;
                        break;
                    case Difficulty.Hard:
                        ProjectileInterval = 800;
                        ObjectMovementSpeed = 5;
                        JumpHeight = -10;
                        ScoreMultiplier = 3;
                        break;
                    case Difficulty.EpicFail:
                        ProjectileInterval = 700;
                        ObjectMovementSpeed = 10;
                        JumpHeight = -10;
                        ScoreMultiplier = 3;
                        break;
                }
            }
        }
        private Point PlayerPosition;
        private int PlayerInitialY;
        private int FireInitialY;
        private int StartingXPos;
        private float JumpSpeed = 0;
        private bool IsJumping = false;
        private bool IsGameEnd = false;
        private Random RandNum = new Random();

        private void InitializeTimer()
        {
            ProjectileGenerator = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = ProjectileInterval
            };
            ScoreTimer = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 100
            };
            ProjectileGenerator.Elapsed += delegate
            {
                CreateObstacle();
            };
            ScoreTimer.Elapsed += delegate
            {
                if (!IsGameEnd)
                {
                    Score += ScoreMultiplier;
                    ObjectMovementSpeed += 0.01f;
                }
            };
        }

        private void CallEndOverlay()
        {
            IsGameEnd = true;
            Global.Scenes.Overlays.Add("GameEnd", new GameEndOverlay(Games.RunningForTheirLives, null, this, GameDifficulty));
        }

        private void CreateObstacle()
        {
            if (!IsGameEnd)
            {
                Image obstacle = new Image("item" + DateTime.Now.ToBinary());
                obstacle.Sprite.Graphic = Global.Textures["fire"];
                obstacle.Sprite.Columns = 3;
                obstacle.Sprite.Rows = 1;
                obstacle.Sprite.SpriteType = SpriteType.Animated;
                obstacle.Location = new Point(RandNum.Next(StartingXPos - 100, StartingXPos), FireInitialY);
                GameCanvas.Children.Add(obstacle.Name, obstacle);
            }
        }

        private void UpdateInitialPosition()
        {
            PlayerInitialY = WindowBounds.Height - PlayerElement.Bounds.Height - 130;
            FireInitialY = WindowBounds.Height - Global.Textures["fire"].Height - 130;
            StartingXPos = WindowBounds.Width - PlayerElement.Bounds.Width - 10;
        }

        public override void LoadContent()
        {
            InitializeComponent();

            JumpEffect = Global.SFX["caught"];
            Global.Audio.PlaySong("shenanigans");
            UpdateInitialPosition();
            InitializeTimer();
            PlayerPosition.X = 150;

            base.LoadContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close all timers
                ProjectileGenerator.Close();
                ScoreTimer.Close();
                GuiUtils.DisposeElements(Elements);
            }
            base.Dispose(disposing);
        }

        public override void Draw()
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            SpriteBatch.End();
        }

        public override void Update()
        {
            GuiUtils.UpdateElements(Elements);

            if (IsGameEnd)
            {
                if (GameCanvas.Children.Count > 0)
                {
                    GameCanvas.Children.Clear();
                }
                return;
            }
            
            if (IsJumping)
            {
                PlayerPosition.Y += (int)JumpSpeed;
                JumpSpeed += 0.5f;
                if (PlayerPosition.Y >= PlayerInitialY)
                {
                    PlayerPosition.Y = PlayerInitialY;
                    IsJumping = false;
                }
            }
            else
            {
                if (Global.Input.KeyPressed(Keys.Space) ||
                    Global.Input.MousePressed(MouseButton.Left) ||
                    Global.Input.MousePressed(MouseButton.Right) ||
                    Global.Input.MousePressed(MouseButton.Middle))
                {
                    IsJumping = true;
                    JumpEffect.Play();
                    JumpSpeed = JumpHeight;
                }
                PlayerPosition.Y = PlayerInitialY;
            }

            PlayerElement.Location = PlayerPosition;

            for (int i = 0; i < GameCanvas.Children.Count; i++)
            {
                string elementKey = GameCanvas.Children.Keys.ElementAt(i);
                BaseElement gameElement = GameCanvas.Children[elementKey];
                // Moves Game object
                gameElement.Location =
                    new Point(gameElement.Location.X - (int)ObjectMovementSpeed, gameElement.Location.Y);

                // Check if Game object collides/intersects with catcher
                if (PlayerElement.Bounds.Contains(gameElement.Bounds.Center))
                {
                    CallEndOverlay();
                    GameCanvas.Children.Remove(elementKey);
                    return;
                }

                // Remove Elements once it exceeds the object catcher
                if (gameElement.Location.X < -100)
                {
                    GameCanvas.Children.Remove(elementKey);
                }
            }
        }
    }
}
