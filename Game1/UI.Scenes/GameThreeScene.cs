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
        public GameThreeScene(Difficulty Difficulty)
            : base("Game 3 Scene: Safety Jump")
        {
            GameDifficulty = Difficulty;
        }

        private Collection<GenericElement> GameObjects = new Collection<GenericElement>();

        public double Score { private set; get; }
        private int ProjectileInterval;
        private float ObjectMovementSpeed;
        private int DistanceFromBottom;
        private float JumpHeight;
        private int ScoreMultiplier;

        private SoundEffect JumpEffect;

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;

        private Difficulty GameDifficulty;
        private Vector2 PlayerPosition;
        private int PlayerInitialY;
        private int FireInitialY;
        private int StartingXPos;
        private float JumpSpeed = 0;
        private bool IsJumping = false;
        private bool IsGameEnd = false;
        private Random RandNum = new Random();

        private Texture2D FireGraphic;
        private Texture2D CharacterGraphic;

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
                {
                    Score += ScoreMultiplier;
                    ObjectMovementSpeed += 0.01f;
                }
            };
        }

        private void CallEndOverlay()
        {
            IsGameEnd = true;
            SceneManager.Overlays.Add("GameEnd", new GameEndOverlay(Games.RunningForTheirLives, null, this, GameDifficulty));
        }

        private void UpdateMinMaxY()
        {
            if (Objects.ContainsKey("Player"))
            {
                PlayerInitialY = Game.GraphicsDevice.Viewport.Height - CharacterGraphic.Height - DistanceFromBottom - 100;
                FireInitialY = Game.GraphicsDevice.Viewport.Height - FireGraphic.Height - DistanceFromBottom - 100;
                StartingXPos = Game.GraphicsDevice.Viewport.Width - CharacterGraphic.Width - 10;
            }
        }

        private void GenerateFire()
        {
            if (!IsGameEnd)
            {
                // create new button object
                Image nwBtn = new Image("item")
                {
                    Graphic = FireGraphic,
                    Columns = 3,
                    Rows = 1,
                    SpriteType = SpriteType.Animated,
                    Location = new Vector2((float)RandNum.Next(StartingXPos - 100, StartingXPos), FireInitialY),
                    ControlAlignment = ControlAlignment.Fixed,
                };
                GameObjects.Add(nwBtn);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            FireGraphic = Global.Textures["fire"];
            CharacterGraphic = Global.Textures["character"];
            JumpEffect = Global.SFX["caught"];
            Global.AudioManager.PlaySong("shenanigans");

            Objects = new Dictionary<string, GenericElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Global.Textures["game-bg-3"],
                    ControlAlignment = ControlAlignment.Fixed,
                    OnUpdate = (element) => {
                        element.DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                    },
                }},
                { "BackButton", new MenuButton("mb")
                {
                    Tooltip = "Back",
                    Graphic = Global.Textures["back-btn"],
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    LayerDepth = 0.1f,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene())
                }},
                { "Player", new Image("Player")
                {
                    Graphic = CharacterGraphic,
                    Columns = 3,
                    Rows = 1,
                    SpriteType = SpriteType.Animated,
                    ControlAlignment = ControlAlignment.Fixed,
                }},
                { "ScoreCounter", new Label("timer")
                {
                    Location = new Vector2(Game.GraphicsDevice.Viewport.Width - 305, 5),
                    ControlAlignment = ControlAlignment.Fixed,
                    LayerDepth = 0.1f,
                    OnUpdate = (element) => {
                        Label a = (Label)element;
                        a.Text = String.Format("Score: {0}", Score);
                        a.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - a.Dimensions.X, 5);
                    },
                    Font = Fonts["o-default_l"]
                }}
            };

            DistanceFromBottom = -30;
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

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

            InitializeTimer();
            PlayerPosition.X = 150;
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
                if (PlayerPosition.Y >= PlayerInitialY)
                {
                    PlayerPosition.Y = PlayerInitialY;
                    IsJumping = false;
                }
            }
            else
            {
                if (InputManager.KeyPressed(Keys.Space) ||
                    InputManager.MousePressed(MouseButton.Left) ||
                    InputManager.MousePressed(MouseButton.Right) ||
                    InputManager.MousePressed(MouseButton.Middle))
                {
                    IsJumping = true;
                    JumpEffect.Play();
                    JumpSpeed = JumpHeight;
                }
                PlayerPosition.Y = PlayerInitialY;
            }

            if (Objects.ContainsKey("Player"))
            {
                if (IsGameEnd)
                {
                    Objects.Remove("Player");
                }
                else
                {
                    Objects["Player"].Location = PlayerPosition;
                }
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Moves Game object
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X - ObjectMovementSpeed, GameObjects[i].Location.Y);

                // Check if Game object collides/intersects with catcher
                if (Objects.ContainsKey("Player") && Objects["Player"].Bounds.Contains(GameObjects[i].Bounds.Center))
                {
                    CallEndOverlay();
                    GameObjects.Remove(GameObjects[i]);
                    return;
                }

                // Remove objects once it exceeds the object catcher, this also removes all objects when time's up
                if (GameObjects[i].Location.X < -100 || IsGameEnd)
                {
                    GameObjects.Remove(GameObjects[i]);
                }
            }
        }
    }
}
