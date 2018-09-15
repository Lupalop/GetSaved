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
        public GameTwoScene(SceneManager sceneManager, Difficulty difficulty)
            : base(sceneManager, "Game 2 Scene: Earthquake Escape")
        {
            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("gameBG4"),
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
                    Graphic = game.Content.Load<Texture2D>("falling-object/briefcase"),
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
                }}
            };

            GameObjects = new Dictionary<string, ObjectBase>() {
                { "PointB", new Image("PointB")
                {
                    Graphic = game.Content.Load<Texture2D>("point"),
                    Location = PosB,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "PointC", new Image("PointC")
                {
                    Graphic = game.Content.Load<Texture2D>("point"),
                    Location = PosC,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "PointD", new Image("PointD")
                {
                    Graphic = game.Content.Load<Texture2D>("point"),
                    Location = PosD,
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }}

            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 15.0;
                    FallingSpeed = 3;
                    break;
                case Difficulty.Medium:
                    timeLeft = 10.0;
                    FallingSpeed = 3;
                    break;
                case Difficulty.Hard:
                    timeLeft = 5.0;
                    FallingSpeed = 5;
                    break;
                case Difficulty.EpicFail:
                    timeLeft = 15.0;
                    FallingSpeed = 10;
                    break;
            }
            this.difficulty = difficulty;
            InitializeTimer();
        }

        private MouseOverlay MsOverlay;
        private Dictionary<string, ObjectBase> GameObjects = new Dictionary<string, ObjectBase>();

        private double timeLeft;
        private float FallingSpeed;

        // Points
        Vector2 PosA = new Vector2(630, 165);
        Vector2 PosB = new Vector2(665, 485);
        Vector2 PosC = new Vector2(100, 530);
        Vector2 PosD = new Vector2(80, 90);

        private Difficulty difficulty;

        Timer TimeLeftController;
        Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            TimeLeftController = new Timer(1000);
            GameTimer = new Timer(timeLeft * 1000);

            TimeLeftController.Elapsed += OnTimeLeftEnd;
            TimeLeftController.AutoReset = true;
            TimeLeftController.Enabled = true;

            GameTimer.Elapsed += OnGameTimerEnd;
            GameTimer.AutoReset = false;
            GameTimer.Enabled = true;
        }

        public override void Unload()
        {
            // Stop timers
            TimeLeftController.Close();
            GameTimer.Close();

            base.Unload();
        }

        private void OnTimeLeftEnd(Object source, ElapsedEventArgs e)
        {
            if (timeLeft >= 1)
                timeLeft -= 1;
        }
        bool removeObjectCatcher = false;
        private void OnGameTimerEnd(Object source, ElapsedEventArgs e)
        {
            removeObjectCatcher = true;
            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.EscapeEarthquake, null, this));
            GameTimer.Enabled = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Label a = (Label)Objects["Timer"];
            a.Text = String.Format("{0} second(s) left", timeLeft);
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            spriteBatch.End();
        }
        Vector2 PosWhich = Vector2.Zero;
        bool isMsPressed = false;
        bool isKeyPressed = false;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                ObjectBase Catchr = Objects["ObjectCatcher"];
                if ((MsState.LeftButton == ButtonState.Pressed && !isMsPressed) || (KeybdState.IsKeyDown(Keys.Space) && !isKeyPressed))
                {
                    if (PosWhich == Vector2.Zero)
                        PosWhich = PosB;
                    if (Catchr.Bounds.Contains(PosB))
                        PosWhich = PosC;
                    if (Catchr.Bounds.Contains(PosC))
                        PosWhich = PosD;

                    //get the difference from pos to player
                    Vector2 differenceToPlayer = PosWhich - Catchr.Location;

                    //first get direction only by normalizing the difference vector
                    //getting only the direction, with a length of one
                    differenceToPlayer.Normalize();

                    //then move in that direction
                    //based on how much time has passed
                    Catchr.Location += differenceToPlayer * (float)gameTime.ElapsedGameTime.TotalMilliseconds * .2f;
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
    }
}
