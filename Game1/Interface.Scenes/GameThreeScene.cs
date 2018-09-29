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
    public class GameThreeScene : SceneBase
    {
        public GameThreeScene(SceneManager sceneManager, Difficulty Difficulty)
            : base(sceneManager, "Game 3 Scene: Safety Jump")
        {
            GameDifficulty = Difficulty;
        }

        private MouseOverlay MsOverlay;
        private List<ObjectBase> GameObjects = new List<ObjectBase>();

        private double Score;
        private int ProjectileInterval;
        private float FallingSpeed;
        private int DistanceFromBottom;
        private float JumpHeight;
        private int ScoreMultiplier;

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
            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.RunningForTheirLives, null, this));
        }

        private void UpdateMinMaxY()
        {
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                MaxYPos = game.GraphicsDevice.Viewport.Height - Objects["ObjectCatcher"].Bounds.Height - DistanceFromBottom - 100;
                StartingXPos = game.GraphicsDevice.Viewport.Width - Objects["ObjectCatcher"].Bounds.Width - 10;
            }
        }

        private void GenerateFire()
        {
            if (!IsGameEnd)
            {
                // create new button object
                Image nwBtn = new Image("crap")
                {
                    Graphic = game.Content.Load<Texture2D>("dino/fire"),
                    Location = new Vector2((float)RandNum.Next(StartingXPos - 100, StartingXPos), MaxYPos),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                };
                GameObjects.Add(nwBtn);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("dino/gameBG3"),
                    AlignToCenter = false,
                    OnUpdate = () => {
                        Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
                    },
                    spriteBatch = this.spriteBatch
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
                    Graphic = game.Content.Load<Texture2D>("human"),
                    Location = new Vector2(5, game.GraphicsDevice.Viewport.Height - 70),
                    AlignToCenter = false,
                    GraphicEffects = SpriteEffects.FlipHorizontally,
                    spriteBatch = this.spriteBatch
                }},
                { "ScoreCounter", new Label("timer")
                {
                    Text = String.Format("Score: {0}", Score),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => {
                        Label a = (Label)Objects["ScoreCounter"];
                        a.Text = String.Format("Score: {0}", Score);
                        a.Location = new Vector2(game.GraphicsDevice.Viewport.Width - a.Font.MeasureString(a.Text).X, 5);
                    },
                    Font = fonts["o-default_l"]
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
                if (KeybdState.IsKeyDown(Keys.Space) || MsState.LeftButton == ButtonState.Pressed ||
                    MsState.RightButton == ButtonState.Pressed || MsState.MiddleButton == ButtonState.Pressed)
                {
                    IsJumping = true;
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
                // Moves game object
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X - FallingSpeed, GameObjects[i].Location.Y);

                // Check if game object collides/intersects with catcher
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
