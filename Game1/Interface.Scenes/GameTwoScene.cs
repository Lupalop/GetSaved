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
    public class GameTwoScene : SceneBase
    {
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
        public GameTwoScene(SceneManager sceneManager, Difficulty difficulty, Games cgame)
            : base(sceneManager, "Game 2 Scene: " + GetGameName(cgame))
        {
            this.difficulty = difficulty;
            currentGame = cgame;
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
                    Text = String.Format("{0} second(s) left", timeLeft),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5),
                    AlignToCenter = false,
                    Tint = Color.Black,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_l"]
                }},
                { "DeathTimer", new Label("timer")
                {
                    //Text = String.Format("{0}", timeLeft2),
                    Tint = Color.Black,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_l"]
                }},

                { "HelpLabel", new Label("helplabel")
                {
                    Text = "",
                    AlignToCenter = true,
                    Tint = Color.Black,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_l"]
                }}
            };

            GameObjects = new Dictionary<string, ObjectBase>() {
                { "PointB", new Image("PointB")
                {
                    Graphic = game.Content.Load<Texture2D>("point"),
                    Location = PosA,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "PointC", new Image("PointC")
                {
                    Graphic = game.Content.Load<Texture2D>("htp"),
                    Location = PosB,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 15.0;
                    WalkSpeed = 1.5f;
                    break;
                case Difficulty.Medium:
                    timeLeft = 12.0;
                    WalkSpeed = 2.5f;
                    break;
                case Difficulty.Hard:
                    timeLeft = 12.0;
                    WalkSpeed = 2f;
                    break;
                case Difficulty.EpicFail:
                    timeLeft = 10.0;
                    WalkSpeed = 2f;
                    break;
            }

            InitializeTimer();
        }

        private Games currentGame;
        private MouseOverlay MsOverlay;
        private Dictionary<string, ObjectBase> GameObjects = new Dictionary<string, ObjectBase>();

        private double timeLeft;
        private double timeLeft2 = 5;
        private float WalkSpeed;

        // Points
        Vector2 PosA = new Vector2(665, 485);
        Vector2 PosB = new Vector2(100, 485);

        private int currentStage = 0;

        private Difficulty difficulty;

        Timer TimeLeftController;
        Timer DeadLeftController;
        Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            TimeLeftController = new Timer(1000);
            DeadLeftController = new Timer(1000);
            GameTimer = new Timer(timeLeft * 1000);

            TimeLeftController.Elapsed += OnTimeLeftEnd;
            TimeLeftController.AutoReset = true;
            TimeLeftController.Enabled = true;

            DeadLeftController.Elapsed += OnDeadLeftEnd;
            DeadLeftController.AutoReset = true;
            DeadLeftController.Enabled = false;

            GameTimer.Elapsed += OnGameTimerEnd;
            GameTimer.AutoReset = false;
            GameTimer.Enabled = true;
        }

        public override void Unload()
        {
            // Stop timers
            TimeLeftController.Close();
            GameTimer.Close();
            DeadLeftController.Close();

            base.Unload();
        }
        private void OnDeadLeftEnd(Object source, ElapsedEventArgs e)
        {
            if (timeLeft2 >= 1)
                timeLeft2 -= 1;
            if (timeLeft2 <= 0)
            {
                GameTimer.Enabled = false;
                TimeLeftController.Enabled = false;
                EndStateDeterminer.MessageHolder.Add(false);
                CallEndOverlay();
            }
        }
        private void OnTimeLeftEnd(Object source, ElapsedEventArgs e)
        {
            if (timeLeft >= 1)
                timeLeft -= 1;
            if (currentGame == Games.EscapeFire)
            {
                string overlayName = String.Format("fade-{0}", DateTime.Now);
                sceneManager.overlays.Add(overlayName, new FadeOverlay(sceneManager, overlayName, Color.Red) { fadeSpeed = 0.1f });
            }
            if ((MsState.LeftButton == ButtonState.Released || KeybdState.IsKeyUp(Keys.Space)) && !DeadLeftController.Enabled)
            {
                Label b = (Label)Objects["DeathTimer"];
                b.Tint = Color.Black;
                timeLeft2 = 5;
                DeadLeftController.Enabled = true;
            }
        }
        bool removeObjectCatcher = false;
        private void OnGameTimerEnd(Object source, ElapsedEventArgs e)
        {
            EndStateDeterminer.MessageHolder.Add(false);
            CallEndOverlay();
            GameTimer.Enabled = false;
        }
        private void CallEndOverlay()
        {
            removeObjectCatcher = true;

            PassedMessage.Add(EndStateDeterminer);

            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.EscapeEarthquake, PassedMessage, this));
        }
        ObjectBase EndStateDeterminer = new Controls.Label("cr");
        List<ObjectBase> PassedMessage = new List<ObjectBase>();

        public override void Draw(GameTime gameTime)
        {
            Label a = (Label)Objects["Timer"];
            a.Text = String.Format("{0} second(s) left", timeLeft);
            Label b = (Label)Objects["DeathTimer"];
            b.Text = String.Format("{0}", timeLeft2);
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            spriteBatch.End();
        }
        Vector2 PosWhich = Vector2.Zero;
        bool isMsPressed = false;
        bool isKeyPressed = false;
        bool locMove = false;
        int currX = 0;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ObjectBase GameBG = Objects["GameBG"];
            if (currentStage != 3 && currentGame == Games.EscapeEarthquake)
            {
                if (!locMove)
                {
                    currX++;
                    if (currX == 3)
                        locMove = true;
                }
                else {
                    currX--;
                    if (currX == -3)
                        locMove = false;
                }
                GameBG.DestinationRectangle = new Rectangle(currX, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            }
            else
            {
                GameBG.DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            }
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
            // If person is in object
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                ObjectBase Catchr = Objects["ObjectCatcher"];
                if (currentStage == 3 && Catchr.Graphic.Name != "human-line")
                {
                    Catchr.Graphic = game.Content.Load<Texture2D>("human-line");
                }
                if ((MsState.LeftButton == ButtonState.Pressed && !isMsPressed) || (KeybdState.IsKeyDown(Keys.Space) && !isKeyPressed))
                {
                    Label b = (Label)Objects["DeathTimer"];
                    b.Tint = Color.Transparent;
                    DeadLeftController.Enabled = false;
                    if (PosWhich == Vector2.Zero)
                    {
                        PosWhich = PosB;
                        SetHelpMessage(1);
                        currentStage = 1;
                        return;
                    }
                    if (Catchr.Bounds.Contains(PosB) && currentStage == 1)
                    {
                        ResetObjectCatcherPosition();
                        SetHelpMessage(2);
                        currentStage = 2;
                        Objects["GameBG"].Graphic = game.Content.Load<Texture2D>("game4-bg2");
                        return;
                    }
                    if (Catchr.Bounds.Contains(PosB) && currentStage == 2)
                    {
                        ResetObjectCatcherPosition();
                        SetHelpMessage(3);
                        currentStage = 3;
                        Objects["GameBG"].Graphic = game.Content.Load<Texture2D>("game4-bg3");
                        return;
                    }
                    // If last stage
                    if (Catchr.Bounds.Contains(PosB) && currentStage == 3)
                    {
                        GameTimer.Enabled = false;
                        TimeLeftController.Enabled = false;
                        EndStateDeterminer.MessageHolder.Add(true);
                        CallEndOverlay();
                    }

                    //get the difference from pos to player
                    Vector2 differenceToPlayer = PosWhich - Catchr.Location;

                    //first get direction only by normalizing the difference vector
                    //getting only the direction, with a length of one
                    differenceToPlayer.Normalize();

                    //then move in that direction
                    //based on how much time has passed
                    Catchr.Location += differenceToPlayer * (float)gameTime.ElapsedGameTime.TotalMilliseconds * WalkSpeed;
                    if (MsState.LeftButton == ButtonState.Pressed)
                        isMsPressed = true;
                    else
                        isKeyPressed = true;
                }
                if (MsState.LeftButton == ButtonState.Released)
                    isMsPressed = false;
                if (KeybdState.IsKeyUp(Keys.Space))
                    isKeyPressed = false;

                if (removeObjectCatcher)
                    Objects.Remove("ObjectCatcher");
            }
        }

        private void ResetObjectCatcherPosition()
        {
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                ObjectBase Catchr = Objects["ObjectCatcher"];
                Catchr.Location = PosA;
            }
        }

        // This method sets the help message show in the center (perhaps remove?)
        private void SetHelpMessage(int PosWhich)
        {
            Label label = (Label)Objects["HelpLabel"];
            switch (currentGame)
            {
                case Games.EscapeEarthquake:
                    if (PosWhich == 1)
                        label.Text = "Duck, cover, and Hold!";
                    if (PosWhich == 2)
                        label.Text = "Stand up and check surrounding area.";
                    if (PosWhich == 3)
                        label.Text = "Line up properly and go outside the\nbuilding or towards to safety!";
                    break;
                case Games.EscapeFire:
                    if (PosWhich == 1)
                        label.Text = "Raise alarm! Indicate that there is fire!";
                    if (PosWhich == 2)
                        label.Text = "Make sure to exit the room or stay away\n from the fire.";
                    if (PosWhich == 3)
                        label.Text = "Use the fire emergency staircases \nand exit the building immediately!";
                    break;
                default:
                    // Do nothing
                    break;
            }
        }
    }
}
