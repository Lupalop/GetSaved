using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.Entities;
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public partial class GameTwoScene : Scene
    {
        public GameTwoScene(Difficulty Difficulty, Games cgame)
            : base("Game 2 Scene: " + GetGameName(cgame))
        {
            GameDifficulty = Difficulty;
            CurrentGame = cgame;
        }

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
                // TODO: Restore once we get progress bar reimplemented in platform
                /*var a = (ProgressBar)Elements["ProgressBar"];
                a.maximum = (float)value;*/
            }
        }

        public double TimeLeft { private set; get; }
        private double DeathTimeLeft = 3;
        private float WalkSpeed;
        private bool IsGameEnd = false;

        private Games CurrentGame;
        private int CurrentStage = 0;
        private Difficulty gameDifficulty;
        private Difficulty GameDifficulty
        {
            get { return gameDifficulty; }
            set
            {
                gameDifficulty = value;
                switch (GameDifficulty)
                {
                    case Difficulty.Easy:
                        InitialTimeLeft = 15.0;
                        WalkSpeed = 1.5f;
                        break;
                    case Difficulty.Medium:
                        InitialTimeLeft = 12.0;
                        WalkSpeed = 2.5f;
                        break;
                    case Difficulty.Hard:
                        InitialTimeLeft = 12.0;
                        WalkSpeed = 2f;
                        break;
                    case Difficulty.EpicFail:
                        InitialTimeLeft = 10.0;
                        WalkSpeed = 2f;
                        break;
                }
            }
        }

        private SoundEffect PointReached;

        public bool IsLevelPassed = false;
        public bool IsTimedOut = false;

        private bool ShakeToLeft = false;
        private int ShakeFactor = 0;

        static string GetGameName(Games cgame)
        {
            switch (cgame)
            {
                case Games.EscapeEarthquake:
                    return "Earthquake Escape";
                case Games.EscapeFire:
                    return "Fire Escape";
                default:
                    break;
            }
            return "";
        }


        private Timer TimeLeftController;
        private Timer DeathTimeLeftController;
        private Timer GameTimer;

        private void CallEndOverlay()
        {
            IsGameEnd = true;
            Application.Scenes.Overlays.Add("GameEnd", new GameEndOverlay(Games.EscapeEarthquake, null, this, GameDifficulty));
        }

        private void ResetPlayerPosition()
        {
            if (PlayerElement != null)
            {
                PlayerElement.Location = PointA.Location;
            }
        }

        private void SetHelpMessage(int StageWhich)
        {
            if (StageWhich == 0)
                HelpLabel.Sprite.Text = String.Empty;
            switch (CurrentGame)
            {
                case Games.EscapeEarthquake:
                    if (StageWhich == 1)
                        HelpLabel.Sprite.Text = "Duck, cover, and Hold!";
                    if (StageWhich == 2)
                        HelpLabel.Sprite.Text = "Stand up and check surrounding area.";
                    if (StageWhich == 3)
                        HelpLabel.Sprite.Text = "Line up properly and go outside the\nbuilding or towards to safety!";
                    break;
                case Games.EscapeFire:
                    if (StageWhich == 1)
                        HelpLabel.Sprite.Text = "Raise alarm! Indicate that there is fire!";
                    if (StageWhich == 2)
                        HelpLabel.Sprite.Text = "Make sure to exit the room or stay away\n from the fire.";
                    if (StageWhich == 3)
                        HelpLabel.Sprite.Text = "Use the fire emergency staircases \nand exit the building immediately!";
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        public override void LoadContent()
        {
            InitializeComponent();

            // Init level
            UpdatePoints();
            SetHelpMessage(1);
            CurrentStage = 1;

            // Initialize timer
            TimeLeftController = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 1000
            };
            DeathTimeLeftController = new Timer()
            {
                AutoReset = true,
                Enabled = false,
                Interval = 1000
            };
            GameTimer = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = TimeLeft * 1000
            };

            TimeLeftController.Elapsed += delegate
            {
                if (TimeLeft >= 1)
                    TimeLeft -= 1;
                if (CurrentGame == Games.EscapeFire)
                {
                    string overlayName = String.Format("fade-{0}", DateTime.Now);
                    Application.Scenes.Overlays.Add(overlayName, new FadeOverlay(overlayName, Color.Red) { FadeSpeed = 0.1f });
                }
                if ((Application.Input.MouseUp(MouseButton.Left) ||
                     Application.Input.MouseUp(MouseButton.Right) ||
                     Application.Input.MouseUp(MouseButton.Middle) ||
                     Application.Input.KeyUp(Keys.Space)) &&
                    !DeathTimeLeftController.Enabled)
                {
                    DeathTimerLabel.Sprite.Tint = Color.Red;
                    DeathTimeLeft = 2;
                    DeathTimeLeftController.Enabled = true;
                }
            };

            DeathTimeLeftController.Elapsed += delegate
            {
                if (DeathTimeLeft >= 1)
                    DeathTimeLeft -= 1;
                if (DeathTimeLeft <= 0 && !IsGameEnd)
                {
                    GameTimer.Enabled = false;
                    TimeLeftController.Enabled = false;
                    IsTimedOut = true;
                    CallEndOverlay();
                }
            };

            GameTimer.Elapsed += delegate
            {
                CallEndOverlay();
                GameTimer.Enabled = false;
            };

            PointReached = Application.SFX["caught"];
            Application.Audio.PlaySong("in-pursuit");

            base.LoadContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Stop timers
                TimeLeftController.Close();
                GameTimer.Close();
                DeathTimeLeftController.Close();
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
            // If person is in object
            if (PlayerElement != null)
            {
                if (CurrentStage == 3 && PlayerElement.Sprite.Graphic.Name != "character-line")
                {
                    PlayerElement.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("character-line"];
                }
                
                if (Application.Input.MousePressed(MouseButton.Left) ||
                    Application.Input.MousePressed(MouseButton.Right) ||
                    Application.Input.MousePressed(MouseButton.Middle) ||
                    Application.Input.KeyPressed(Keys.Space))
                {
                    DeathTimerLabel.Sprite.Tint = Color.Transparent;
                    DeathTimeLeftController.Enabled = false;
                    if (PlayerElement.Bounds.Intersects(PointB.Bounds))
                    {
                        ResetPlayerPosition();
                        PointReached.Play();
                        switch (CurrentStage)
                        {
                            case 1:
                                SetHelpMessage(2);
                                CurrentStage = 2;
                                GameBG.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("game-bg-4_2"];
                                break;
                            case 2:
                                SetHelpMessage(3);
                                CurrentStage = 3;
                                GameBG.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("game-bg-4_3"];
                                break;
                            case 3:
                                SetHelpMessage(0);
                                GameTimer.Enabled = false;
                                TimeLeftController.Enabled = false;
                                IsLevelPassed = true;
                                CallEndOverlay();
                                break;
                            default:
                                break;
                        }
                        Display_ResolutionChanged(Application.Display, EventArgs.Empty);
                    }

                    // Get difference from pos to player
                    Vector2 differenceToPlayer = (PointB.Location - PlayerElement.Location).ToVector2();

                    // Get direction only by normalizing the difference vector
                    // Getting only the direction, with a length of one
                    differenceToPlayer.Normalize();

                    differenceToPlayer = differenceToPlayer * (float)Application.GameTime.ElapsedGameTime.TotalMilliseconds * WalkSpeed;

                    // Move in that direction based on elapsed time
                    PlayerElement.Location += differenceToPlayer.ToPoint();
                }

                if (IsGameEnd)
                {
                    GameCanvas.Children.Remove(PlayerElement.Name);
                }
            }
        }

    }
}
