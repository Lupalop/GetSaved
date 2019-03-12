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
using Maquina.UI.Controls;
using Maquina.Objects;
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameTwoScene : SceneBase
    {
        public GameTwoScene(SceneManager SceneManager, Difficulty Difficulty, Games cgame)
            : base(SceneManager, "Game 2 Scene: " + GetGameName(cgame))
        {
            GameDifficulty = Difficulty;
            CurrentGame = cgame;
        }

        private Dictionary<string, GenericElement> GameObjects = new Dictionary<string, GenericElement>();

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

        private GenericElement EndStateDeterminer = new Controls.Label("cr");
        private Collection<GenericElement> PassedMessage = new Collection<GenericElement>();

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
            TimeLeftController = new Timer(1000);
            DeathTimeLeftController = new Timer(1000);
            GameTimer = new Timer(TimeLeft * 1000);

            TimeLeftController.Elapsed += delegate
            {
                if (TimeLeft >= 1)
                    TimeLeft -= 1;
                if (CurrentGame == Games.EscapeFire)
                {
                    string overlayName = String.Format("fade-{0}", DateTime.Now);
                    SceneManager.Overlays.Add(overlayName, new FadeOverlay(SceneManager, overlayName, Color.Red) { FadeSpeed = 0.1f });
                }
                if ((InputManager.MouseUp(MouseButton.Left) ||
                     InputManager.MouseUp(MouseButton.Right) ||
                     InputManager.MouseUp(MouseButton.Middle) ||
                     InputManager.KeyUp(Keys.Space)) &&
                    !DeathTimeLeftController.Enabled)
                {
                    Label b = (Label)Objects["DeathTimer"];
                    b.Tint = Color.Red;
                    DeathTimeLeft = 2;
                    DeathTimeLeftController.Enabled = true;
                }
            };
            TimeLeftController.AutoReset = true;
            TimeLeftController.Enabled = true;

            DeathTimeLeftController.Elapsed += delegate
            {
                if (DeathTimeLeft >= 1)
                    DeathTimeLeft -= 1;
                if (DeathTimeLeft <= 0 && !IsGameEnd)
                {
                    GameTimer.Enabled = false;
                    TimeLeftController.Enabled = false;
                    EndStateDeterminer.MessageHolder.Add(false);
                    CallEndOverlay();
                }
            };
            DeathTimeLeftController.AutoReset = true;
            DeathTimeLeftController.Enabled = false;

            GameTimer.Elapsed += OnGameTimerEnd;
            GameTimer.AutoReset = false;
            GameTimer.Enabled = true;
        }

        private void OnGameTimerEnd(Object source, ElapsedEventArgs e)
        {
            EndStateDeterminer.MessageHolder.Add(false);
            CallEndOverlay();
            GameTimer.Enabled = false;
        }

        private void CallEndOverlay()
        {
            IsGameEnd = true;
            PassedMessage.Add(EndStateDeterminer);
            SceneManager.Overlays.Add("GameEnd", new GameEndOverlay(SceneManager, Games.EscapeEarthquake, PassedMessage, this));
        }

        private void ResetPlayerPosition()
        {
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                GenericElement Catchr = Objects["ObjectCatcher"];
                Catchr.Location = PosA;
            }
        }

        private void SetHelpMessage(int StageWhich)
        {
            Label label = (Label)Objects["HelpLabel"];
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
            Objects = new Dictionary<string, GenericElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Game.Content.Load<Texture2D>("game4-bg1"),
                    DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height),
                    ControlAlignment = ControlAlignment.Fixed,
                    OnUpdate = () => {
                        GenericElement GameBG = Objects["GameBG"];
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
                            GameBG.DestinationRectangle = new Rectangle(ShakeFactor, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        }
                        else
                        {
                            GameBG.DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                        }
                    },
                    SpriteBatch = this.SpriteBatch
                }},
                { "ProgressBar", new ProgressBar("ProgressBar", new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, 32), SceneManager)
                {
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        var a = (ProgressBar)Objects["ProgressBar"];
                        a.value = (float)TimeLeft;
                    }
                }},
                { "BackButton", new MenuButton("mb", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},
                { "ObjectCatcher", new Image("ObjectCatcher")
                {
                    Graphic = Game.Content.Load<Texture2D>("human"),
                    Location = PosA,
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                }},
                { "Timer", new Label("timer")
                {
                    Text = String.Format("{0} second(s) left", TimeLeft),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        Label a = (Label)Objects["Timer"];
                        a.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - a.Dimensions.X, 5);
                        a.Text = String.Format("{0} second(s) left", TimeLeft);
                    },
                    Font = Fonts["o-default_l"]
                }},
                { "DeathTimer", new Label("timer")
                {
                    Tint = Color.Red,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        Label b = (Label)Objects["DeathTimer"];
                        b.Text = String.Format("{0}", DeathTimeLeft);
                    },
                    Font = Fonts["o-default_xl"]
                }},
                { "HelpLabel", new Label("helplabel")
                {
                    ControlAlignment = ControlAlignment.Center,
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["o-default_l"]
                }}
            };

            GameObjects = new Dictionary<string, GenericElement>() {
                { "PointA", new Image("PointA")
                {
                    Graphic = Game.Content.Load<Texture2D>("point"),
                    Location = PosA,
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        GameObjects["PointA"].Location = PosA;
                    }
                }},
                { "PointB", new Image("PointB")
                {
                    Graphic = Game.Content.Load<Texture2D>("htp"),
                    Location = PosB,
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        GameObjects["PointB"].Location = PosB;
                    }
                }}
            };

            PointReached = Game.Content.Load<SoundEffect>("sfx/caught");
            SceneManager.PlaySong("in-pursuit");
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

            base.Unload();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePoints();
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
            // If person is in object
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                GenericElement Catchr = Objects["ObjectCatcher"];
                if (CurrentStage == 3 && Catchr.Graphic.Name != "human-line")
                    Catchr.Graphic = Game.Content.Load<Texture2D>("human-line");
                
                if (InputManager.MousePressed(MouseButton.Left) ||
                    InputManager.MousePressed(MouseButton.Right) ||
                    InputManager.MousePressed(MouseButton.Middle) ||
                    InputManager.KeyPressed(Keys.Space))
                {
                    Label b = (Label)Objects["DeathTimer"];
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
                                Objects["GameBG"].Graphic = Game.Content.Load<Texture2D>("game4-bg2");
                                return;
                            case 2:
                                SetHelpMessage(3);
                                CurrentStage = 3;
                                Objects["GameBG"].Graphic = Game.Content.Load<Texture2D>("game4-bg3");
                                return;
                            case 3:
                                SetHelpMessage(0);
                                GameTimer.Enabled = false;
                                TimeLeftController.Enabled = false;
                                EndStateDeterminer.MessageHolder.Add(true);
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
                    Objects.Remove("ObjectCatcher");
            }
        }

    }
}
