using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.Elements;
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameThreeScene : SceneBase
    {
        public GameThreeScene(SceneManager SceneManager, Difficulty Difficulty)
            : base(SceneManager, "Game 3 Scene: Safety Jump")
        {
            GameDifficulty = Difficulty;
        }

        private Collection<GenericElement> GameObjects = new Collection<GenericElement>();

        private double Score;
        private int ProjectileInterval;
        private float FallingSpeed;
        private int DistanceFromBottom;
        private float JumpHeight;
        private int ScoreMultiplier;

        private SoundEffect JumpEffect;

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;

        private Difficulty GameDifficulty;
        private Vector2 PlayerPosition;
        private int MaxYPos;
        private int StartingXPos;
        private float JumpSpeed = 0;
        private bool IsJumping = false;
        private bool IsGameEnd = false;
        private Random RandNum = new Random();

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(ProjectileInterval) { AutoReset = true, Enabled = true };
            TimeLeftController = new Timer(100) { AutoReset = true, Enabled = true };
            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += delegate
            {
                GenerateFire();
            };
            TimeLeftController.Elapsed += delegate
            {
                if (!IsGameEnd)
                    Score += ScoreMultiplier;
            };
        }

        private void CallEndOverlay()
        {
            IsGameEnd = true;
            SceneManager.Overlays.Add("GameEnd", new GameEndOverlay(SceneManager, Games.RunningForTheirLives, null, this));
        }

        private void UpdateMinMaxY()
        {
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                MaxYPos = Game.GraphicsDevice.Viewport.Height - Objects["ObjectCatcher"].Bounds.Height - DistanceFromBottom - 100;
                StartingXPos = Game.GraphicsDevice.Viewport.Width - Objects["ObjectCatcher"].Bounds.Width - 10;
            }
        }

        private void GenerateFire()
        {
            if (!IsGameEnd)
            {
                // create new button object
                Image nwBtn = new Image("item")
                {
                    Graphic = Game.Content.Load<Texture2D>("dino/fire"),
                    Location = new Vector2((float)RandNum.Next(StartingXPos - 100, StartingXPos), MaxYPos),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch
                };
                GameObjects.Add(nwBtn);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Game.Content.Load<Texture2D>("game-bg/3"),
                    ControlAlignment = ControlAlignment.Fixed,
                    OnUpdate = () => {
                        Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                    },
                    SpriteBatch = this.SpriteBatch
                }},
                { "BackButton", new MenuButton("mb", SceneManager)
                {
                    Tooltip = "Back",
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LayerDepth = 0.1f,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},
                { "ObjectCatcher", new Image("ObjectCatcher")
                {
                    Graphic = Game.Content.Load<Texture2D>("human"),
                    Location = new Vector2(5, Game.GraphicsDevice.Viewport.Height - 70),
                    ControlAlignment = ControlAlignment.Fixed,
                    GraphicEffects = SpriteEffects.FlipHorizontally,
                    SpriteBatch = this.SpriteBatch
                }},
                { "ScoreCounter", new Label("timer")
                {
                    Location = new Vector2(Game.GraphicsDevice.Viewport.Width - 305, 5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LayerDepth = 0.1f,
                    OnUpdate = () => {
                        Label a = (Label)Objects["ScoreCounter"];
                        a.Text = String.Format("Score: {0}", Score);
                        a.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - a.Dimensions.X, 5);
                    },
                    Font = Fonts["o-default_l"]
                }}
            };

            JumpEffect = Game.Content.Load<SoundEffect>("sfx/caught");
            SceneManager.AudioManager.PlaySong("shenanigans");
            DistanceFromBottom = -30;
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    ProjectileInterval = 1500;
                    FallingSpeed = 3;
                    JumpHeight = -15;
                    ScoreMultiplier = 1;
                    break;
                case Difficulty.Medium:
                    ProjectileInterval = 1000;
                    FallingSpeed = 3;
                    JumpHeight = -15;
                    ScoreMultiplier = 2;
                    break;
                case Difficulty.Hard:
                    ProjectileInterval = 800;
                    FallingSpeed = 5;
                    JumpHeight = -10;
                    ScoreMultiplier = 3;
                    break;
                case Difficulty.EpicFail:
                    ProjectileInterval = 700;
                    FallingSpeed = 10;
                    JumpHeight = -20;
                    ScoreMultiplier = 3;
                    break;
            }

            InitializeTimer();
            UpdateMinMaxY();
            GenerateFire();
            PlayerPosition = new Vector2(100, MaxYPos - 100);
        }

        public override void Unload()
        {
            // Close all timers
            ProjectileGenerator.Close();
            TimeLeftController.Close();

            base.Unload();
        }

        public override void Draw(GameTime GameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(GameTime);
            base.DrawObjects(GameTime, Objects);
            base.DrawObjects(GameTime, GameObjects);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateObjects(GameTime, Objects);
            base.UpdateObjects(GameTime, GameObjects);
            UpdateMinMaxY();
            if (IsJumping)
            {
                PlayerPosition.Y += JumpSpeed;
                JumpSpeed += 0.5f;
                if (PlayerPosition.Y >= MaxYPos)
                {
                    PlayerPosition.Y = MaxYPos;
                    IsJumping = false;
                }
            }
            else
            {
                if (InputManager.KeyboardState.IsKeyDown(Keys.Space) ||
                    InputManager.MousePressed(MouseButton.Left) ||
                    InputManager.MousePressed(MouseButton.Right) ||
                    InputManager.MousePressed(MouseButton.Middle))
                {
                    IsJumping = true;
                    JumpEffect.Play();
                    JumpSpeed = JumpHeight;
                }
            }

            if (Objects.ContainsKey("ObjectCatcher"))
            {
                if (IsGameEnd)
                    Objects.Remove("ObjectCatcher");
                else
                    Objects["ObjectCatcher"].Location = PlayerPosition;
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Moves Game object
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X - FallingSpeed, GameObjects[i].Location.Y);

                // Check if Game object collides/intersects with catcher
                if (Objects.ContainsKey("ObjectCatcher") && Objects["ObjectCatcher"].Bounds.Contains(GameObjects[i].Bounds.Center))
                {
                    CallEndOverlay();
                    GameObjects.Remove(GameObjects[i]);
                    return;
                }

                // Remove objects once it exceeds the object catcher, this also removes all objects when time's up
                if ((Objects.ContainsKey("ObjectCatcher") && (GameObjects[i].Location.X < Objects["ObjectCatcher"].Location.X - 50)) || IsGameEnd)
                    GameObjects.Remove(GameObjects[i]);
            }
        }
    }
}
