using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.Elements;
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameTwoScene : Scene
    {
        public GameTwoScene(Difficulty Difficulty, Games cgame)
            : base("Game 2 Scene: " + GetGameName(cgame))
        {
            GameDifficulty = Difficulty;
            CurrentGame = cgame;
        }

        private Dictionary<string, BaseElement> GameElements = new Dictionary<string, BaseElement>();

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
                var a = (ProgressBar)Elements["ProgressBar"];
                a.maximum = (float)value;
            }
        }

        public double TimeLeft { private set; get; }
        private double DeathTimeLeft = 3;
        private float WalkSpeed;
        private bool IsGameEnd = false;

        // Points
        private Vector2 PosA = new Vector2(800 / 2 + 250, 600 / 2 + 185);
        private Vector2 PosB = new Vector2(800 / 2 - 300, 600 / 2 + 185);

        private Games CurrentGame;
        private int CurrentStage = 0;
        private Difficulty GameDifficulty;

        private SoundEffect PointReached;

        private Timer TimeLeftController;
        private Timer DeathTimeLeftController;
        private Timer GameTimer;

        public bool IsLevelPassed = false;
        public bool IsTimedOut = false;

        private Vector2 PosWhich = Vector2.Zero;
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

        private void InitializeTimer()
        {
            // Initiailize timers
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
                    Global.Scenes.Overlays.Add(overlayName, new FadeOverlay(overlayName, Color.Red) { FadeSpeed = 0.1f });
                }
                if ((InputManager.MouseUp(MouseButton.Left) ||
                     InputManager.MouseUp(MouseButton.Right) ||
                     InputManager.MouseUp(MouseButton.Middle) ||
                     InputManager.KeyUp(Keys.Space)) &&
                    !DeathTimeLeftController.Enabled)
                {
                    Label b = (Label)Elements["DeathTimer"];
                    b.Tint = Color.Red;
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
        }

        private void CallEndOverlay()
        {
            IsGameEnd = true;
            Global.Scenes.Overlays.Add("GameEnd", new GameEndOverlay(Games.EscapeEarthquake, null, this, GameDifficulty));
        }

        private void ResetPlayerPosition()
        {
            if (Elements.ContainsKey("Player"))
            {
                BaseElement Catchr = Elements["Player"];
                Catchr.Location = PosA;
            }
        }

        private void SetHelpMessage(int StageWhich)
        {
            Label label = (Label)Elements["HelpLabel"];
            if (StageWhich == 0)
                label.Text = String.Empty;
            switch (CurrentGame)
            {
                case Games.EscapeEarthquake:
                    if (StageWhich == 1)
                        label.Text = "Duck, cover, and Hold!";
                    if (StageWhich == 2)
                        label.Text = "Stand up and check surrounding area.";
                    if (StageWhich == 3)
                        label.Text = "Line up properly and go outside the\nbuilding or towards to safety!";
                    break;
                case Games.EscapeFire:
                    if (StageWhich == 1)
                        label.Text = "Raise alarm! Indicate that there is fire!";
                    if (StageWhich == 2)
                        label.Text = "Make sure to exit the room or stay away\n from the fire.";
                    if (StageWhich == 3)
                        label.Text = "Use the fire emergency staircases \nand exit the building immediately!";
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        private void UpdatePoints()
        {
            // Update positions
            PosA = new Vector2(ScreenCenter.X + 250, ScreenCenter.Y + 185);
            PosB = new Vector2(ScreenCenter.X - 300, ScreenCenter.Y + 185);
            PosWhich = PosB;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Elements = new Dictionary<string, BaseElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Global.Textures["game-bg-4_1"],
                    DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height),
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        if (CurrentStage != 3 && CurrentGame == Games.EscapeEarthquake)
                        {
                            if (!ShakeToLeft)
                            {
                                ShakeFactor++;
                                if (ShakeFactor == 3)
                                    ShakeToLeft = true;
                            }
                            else
                            {
                                ShakeFactor--;
                                if (ShakeFactor == -3)
                                    ShakeToLeft = false;
                            }
                            element.DestinationRectangle = new Rectangle(ShakeFactor, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        }
                        else
                        {
                            element.DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        }
                    },
                }},
                { "ProgressBar", new ProgressBar("ProgressBar", new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, 32))
                {
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        var a = (ProgressBar)element;
                        a.value = (float)TimeLeft;
                    }
                }},
                { "BackButton", new MenuButton("mb")
                {
                    Tooltip = "Back",
                    Graphic = Global.Textures["back-btn"],
                    Location = new Vector2(5,5),
                    ControlAlignment = Alignment.Fixed,
                    LayerDepth = 0.1f,
                    LeftClickAction = () => Global.Scenes.SwitchToScene(new MainMenuScene())
                }},
                { "Player", new Image("Player")
                {
                    Graphic = Global.Textures["character"],
                    Columns = 3,
                    Rows = 1,
                    SpriteType = SpriteType.Animated,
                    Location = PosA,
                    GraphicEffects = SpriteEffects.FlipHorizontally,
                    ControlAlignment = Alignment.Fixed,
                }},
                { "Timer", new Label("timer")
                {
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        Label a = (Label)element;
                        a.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - a.Dimensions.X, 5);
                        a.Text = TimeLeft.ToString();
                    },
                    LayerDepth = 0.1f,
                    Font = Global.Fonts["o-default_l"]
                }},
                { "DeathTimer", new Label("timer")
                {
                    Tint = Color.Red,
                    OnUpdate = (element) => {
                        Label b = (Label)element;
                        b.Text = DeathTimeLeft.ToString();
                    },
                    Font = Global.Fonts["o-default_xl"]
                }},
                { "HelpLabel", new Label("helplabel")
                {
                    ControlAlignment = Alignment.Center,
                    Font = Global.Fonts["o-default_l"]
                }}
            };

            GameElements = new Dictionary<string, BaseElement>() {
                { "PointA", new Image("PointA")
                {
                    Graphic = Global.Textures["starting-point"],
                    Location = PosA,
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        element.Location = PosA;
                    }
                }},
                { "PointB", new Image("PointB")
                {
                    Graphic = Global.Textures["exit-label"],
                    Location = PosB,
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        element.Location = PosB;
                    }
                }}
            };

            PointReached = Global.SFX["caught"];
            Global.AudioManager.PlaySong("in-pursuit");
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

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

            // Init level
            UpdatePoints();
            SetHelpMessage(1);
            CurrentStage = 1;

            InitializeTimer();
        }

        public override void Unload()
        {
            // Stop timers
            TimeLeftController.Close();
            GameTimer.Close();
            DeathTimeLeftController.Close();
            DisposeElements(GameElements);

            base.Unload();
        }

        public override void Draw()
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            GuiUtils.DrawElements(Elements);
            GuiUtils.DrawElements(GameElements);
            SpriteBatch.End();
        }

        public override void Update()
        {
            UpdatePoints();
            base.Update(gameTime);
            GuiUtils.UpdateElements(Elements);
            GuiUtils.UpdateElements(GameElements);
            // If person is in object
            if (Elements.ContainsKey("Player"))
            {
                BaseElement Catchr = Elements["Player"];
                if (CurrentStage == 3 && Catchr.Graphic.Name != "character-line")
                {
                    Catchr.Graphic = Global.Textures["character-line"];
                }
                
                if (InputManager.MousePressed(MouseButton.Left) ||
                    InputManager.MousePressed(MouseButton.Right) ||
                    InputManager.MousePressed(MouseButton.Middle) ||
                    InputManager.KeyPressed(Keys.Space))
                {
                    Label b = (Label)Elements["DeathTimer"];
                    b.Tint = Color.Transparent;
                    DeathTimeLeftController.Enabled = false;
                    // Keep in sync with human height
                    var ExpandedBounds = new Rectangle((int)PosB.X, (int)PosB.Y, 79, 79);
                    if (Catchr.Bounds.Intersects(ExpandedBounds))
                    {
                        ResetPlayerPosition();
                        PointReached.Play();
                        switch (CurrentStage)
                        {
                            case 1:
                                SetHelpMessage(2);
                                CurrentStage = 2;
                                Elements["GameBG"].Graphic = Global.Textures["game-bg-4_2"];
                                return;
                            case 2:
                                SetHelpMessage(3);
                                CurrentStage = 3;
                                Elements["GameBG"].Graphic = Global.Textures["game-bg-4_3"];
                                return;
                            case 3:
                                SetHelpMessage(0);
                                GameTimer.Enabled = false;
                                TimeLeftController.Enabled = false;
                                IsLevelPassed = true;
                                CallEndOverlay();
                                return;
                            default:
                                return;
                        }
                    }

                    // Get difference from pos to player
                    Vector2 differenceToPlayer = PosWhich - Catchr.Location;

                    // Get direction only by normalizing the difference vector
                    // Getting only the direction, with a length of one
                    differenceToPlayer.Normalize();

                    // Move in that direction based on elapsed time
                    Catchr.Location += differenceToPlayer * (float)gameTime.ElapsedGameTime.TotalMilliseconds * WalkSpeed;
                }

                if (IsGameEnd)
                    Elements.Remove("Player");
            }
        }

    }
}
