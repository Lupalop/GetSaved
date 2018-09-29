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
    public class GameTwoScene : SceneBase
    {
        public GameTwoScene(SceneManager sceneManager, Difficulty Difficulty, Games cgame)
            : base(sceneManager, "Game 2 Scene: " + GetGameName(cgame))
        {
            GameDifficulty = Difficulty;
            CurrentGame = cgame;
        }

        private MouseOverlay MsOverlay;
        private Dictionary<string, ObjectBase> GameObjects = new Dictionary<string, ObjectBase>();

        private double TimeLeft;
        private double DeathTimeLeft = 3;
        private float WalkSpeed;
        private bool IsGameEnd = false;

        // Points
        private Vector2 PosA = new Vector2(665, 485);
        private Vector2 PosB = new Vector2(100, 485);

        private Games CurrentGame;
        private int CurrentStage = 0;
        private Difficulty GameDifficulty;

        private Timer TimeLeftController;
        private Timer DeathTimeLeftController;
        private Timer GameTimer;

        private ObjectBase EndStateDeterminer = new Controls.Label("cr");
        private List<ObjectBase> PassedMessage = new List<ObjectBase>();

        private Vector2 PosWhich = Vector2.Zero;
        private bool IsMsPressed = false;
        private bool IsKeyPressed = false;
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

            TimeLeftController.Elapsed += OnTimeLeftEnd;
            TimeLeftController.AutoReset = true;
            TimeLeftController.Enabled = true;

            DeathTimeLeftController.Elapsed += OnDeadLeftEnd;
            DeathTimeLeftController.AutoReset = true;
            DeathTimeLeftController.Enabled = false;

            GameTimer.Elapsed += OnGameTimerEnd;
            GameTimer.AutoReset = false;
            GameTimer.Enabled = true;
        }

        private void OnDeadLeftEnd(Object source, ElapsedEventArgs e)
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
        }

        private void OnTimeLeftEnd(Object source, ElapsedEventArgs e)
        {
            if (TimeLeft >= 1)
                TimeLeft -= 1;
            if (CurrentGame == Games.EscapeFire)
            {
                string overlayName = String.Format("fade-{0}", DateTime.Now);
                sceneManager.overlays.Add(overlayName, new FadeOverlay(sceneManager, overlayName, Color.Red) { FadeSpeed = 0.1f });
            }
            if ((MsState.LeftButton == ButtonState.Released || KeybdState.IsKeyUp(Keys.Space)) && !DeathTimeLeftController.Enabled)
            {
                Label b = (Label)Objects["DeathTimer"];
                b.Tint = Color.Red;
                DeathTimeLeft = 2;
                DeathTimeLeftController.Enabled = true;
            }
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
            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.EscapeEarthquake, PassedMessage, this));
        }

        private void ResetPlayerPosition()
        {
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                ObjectBase Catchr = Objects["ObjectCatcher"];
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

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("game4-bg1"),
                    DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    AlignToCenter = false,
                    OnUpdate = () => {
                        ObjectBase GameBG = Objects["GameBG"];
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
                            GameBG.DestinationRectangle = new Rectangle(ShakeFactor, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
                        }
                        else
                        {
                            GameBG.DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
                        }
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
                    Location = PosA,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "Timer", new Label("timer")
                {
                    Text = String.Format("{0} second(s) left", TimeLeft),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => {
                        Label a = (Label)Objects["Timer"];
                        a.Location = new Vector2(game.GraphicsDevice.Viewport.Width - a.Font.MeasureString(a.Text).X, 5);
                        a.Text = String.Format("{0} second(s) left", TimeLeft);
                    },
                    Font = fonts["o-default_l"]
                }},
                { "DeathTimer", new Label("timer")
                {
                    Tint = Color.Red,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => {
                        Label b = (Label)Objects["DeathTimer"];
                        b.Text = String.Format("{0}", DeathTimeLeft);
                    },
                    Font = fonts["o-default_xl"]
                }},
                { "HelpLabel", new Label("helplabel")
                {
                    AlignToCenter = true,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["o-default_l"]
                }}
            };

            GameObjects = new Dictionary<string, ObjectBase>() {
                { "PointA", new Image("PointA")
                {
                    Graphic = game.Content.Load<Texture2D>("point"),
                    Location = PosA,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "PointB", new Image("PointB")
                {
                    Graphic = game.Content.Load<Texture2D>("htp"),
                    Location = PosB,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    TimeLeft = 15.0;
                    WalkSpeed = 1.5f;
                    break;
                case Difficulty.Medium:
                    TimeLeft = 12.0;
                    WalkSpeed = 2.5f;
                    break;
                case Difficulty.Hard:
                    TimeLeft = 12.0;
                    WalkSpeed = 2f;
                    break;
                case Difficulty.EpicFail:
                    TimeLeft = 10.0;
                    WalkSpeed = 2f;
                    break;
            }

            // Init level
            PosWhich = PosB;
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
            // If person is in object
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                ObjectBase Catchr = Objects["ObjectCatcher"];
                if (CurrentStage == 3 && Catchr.Graphic.Name != "human-line")
                    Catchr.Graphic = game.Content.Load<Texture2D>("human-line");
                
                if ((MsState.LeftButton == ButtonState.Pressed && !IsMsPressed) || (KeybdState.IsKeyDown(Keys.Space) && !IsKeyPressed))
                {
                    Label b = (Label)Objects["DeathTimer"];
                    b.Tint = Color.Transparent;
                    DeathTimeLeftController.Enabled = false;

                    if (Catchr.Bounds.Contains(PosB))
                    {
                        ResetPlayerPosition();
                        switch (CurrentStage)
                        {
                            case 1:
                                SetHelpMessage(2);
                                CurrentStage = 2;
                                Objects["GameBG"].Graphic = game.Content.Load<Texture2D>("game4-bg2");
                                return;
                            case 2:
                                SetHelpMessage(3);
                                CurrentStage = 3;
                                Objects["GameBG"].Graphic = game.Content.Load<Texture2D>("game4-bg3");
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
                    if (MsState.LeftButton == ButtonState.Pressed)
                        IsMsPressed = true;
                    else
                        IsKeyPressed = true;
                }
                if (MsState.LeftButton == ButtonState.Released)
                    IsMsPressed = false;
                if (KeybdState.IsKeyUp(Keys.Space))
                    IsKeyPressed = false;

                if (IsGameEnd)
                    Objects.Remove("ObjectCatcher");
            }
        }

    }
}
