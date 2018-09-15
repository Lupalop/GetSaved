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
    public class GameFourScene : SceneBase
    {
        public GameFourScene(SceneManager sceneManager, Difficulty difficulty)
            : base(sceneManager, "Game 4 Scene: Heal 'em")
        {
            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("gameBG2"),
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
                { "Timer", new Label("timer")
                {
                    Text = String.Format("{0} second(s) left", timeLeft),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_l"]
                }},
                { "Hand1", new Image("hand1")
                {
                    Graphic = game.Content.Load<Texture2D>("aid-em/Hand"),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Columns = 2,
                    Rows = 1
                }},
                { "Hand2", new Image("hand2")
                {
                    Graphic = game.Content.Load<Texture2D>("aid-em/Hand"),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    CurrentFrame = 1,
                    Columns = 2,
                    Rows = 1
                }},
                { "Controller-X", new MenuButton("controller-x", sceneManager)
                {
                    Text = "X",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(80,460),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.X)
                }},
                { "Controller-A", new MenuButton("controller-a", sceneManager)
                {
                    Text = "A",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(150, 530),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.A)
                }},
                { "Controller-S", new MenuButton("controller-s", sceneManager)
                {
                    Text = "S",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(730,460),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.S)
                }},
                { "Controller-O", new MenuButton("controller-o", sceneManager)
                {
                    Text = "O",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(650, 550),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.O)
                }},
                { "PressLabel", new Label("press-label")
                {
                    Text = String.Format("Press/Tap: {0}!", CurrentController.ToString()),
                    spriteBatch = this.spriteBatch,
                    AlignToCenter = false,
                    Font = fonts["default_m"]
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 24.0;
                    projectileInterval = 6000;
                    hitsBeforeSaved = 5;
                    break;
                case Difficulty.Medium:
                    timeLeft = 15.0;
                    projectileInterval = 5000;
                    hitsBeforeSaved = 5;
                    break;
                case Difficulty.Hard:
                    timeLeft = 12.0;
                    projectileInterval = 4000;
                    hitsBeforeSaved = 5;
                    break;
                case Difficulty.EpicFail:
                    timeLeft = 50.0;
                    projectileInterval = 5000;
                    hitsBeforeSaved = 10;
                    break;
            }
            this.difficulty = difficulty;
            InitializeTimer();
        }

        private MouseOverlay MsOverlay;
        private Dictionary<string, ObjectBase> GameObjects = new Dictionary<string, ObjectBase>();
        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();

        private double timeLeft;
        private int projectileInterval;
        private int hitsBeforeSaved;

        private ControllerKeys CurrentController;

        private Difficulty difficulty;

        Timer ProjectileGenerator;
        Timer TimeLeftController;
        Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(projectileInterval) { AutoReset = true, Enabled = true };
            TimeLeftController = new Timer(1000)                { AutoReset = true, Enabled = true };
            GameTimer = new Timer(timeLeft * 1000)              { AutoReset = false, Enabled = true };

            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += ProjectileGenerator_Elapsed;
            ProjectileGenerator_Elapsed(null, null);
            TimeLeftController.Elapsed += delegate
            {
                if (timeLeft >= 1)
                    timeLeft -= 1;
            };
            GameTimer.Elapsed += delegate
            {
                ProjectileGenerator.Close();
                AttemptRemoveHelpman();

                sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.HelpOthersNow, CollectedObjects, this));
                GameTimer.Enabled = false;
            };
        }

        void ProjectileGenerator_Elapsed(object sender, ElapsedEventArgs e)
        {
            AttemptRemoveHelpman();
            GameObjects.Add("helpman", new Helpman("helpman")
            {
                Graphic = game.Content.Load<Texture2D>("aid-em/helpman"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                HitsBeforeBreak = hitsBeforeSaved
            });
        }

        void AttemptRemoveHelpman()
        {
            if (GameObjects.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameObjects["helpman"];
                if (helpman.HitsBeforeBreak > 0)
                    helpman.MessageHolder.Add("!");
                else
                    helpman.MessageHolder.Add("~");
                CollectedObjects.Add(helpman);
                GameObjects.Remove("helpman");
            }
        }
        bool ChangeControllerKeyNow = true;
        enum ControllerKeys { X, A, S, O }
        void AddSubtractBrickHit(ControllerKeys cKey)
        {
            if (GameObjects.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameObjects["helpman"];
                if (CurrentController == cKey)
                    helpman.HitsBeforeBreak--;
                else
                    helpman.HitsBeforeBreak++;
                ChangeControllerKeyNow = true;
                if (helpman.HitsBeforeBreak <= 0)
                    AttemptRemoveHelpman();
            }
        }

        public override void Unload()
        {
            // Close all timers
            ProjectileGenerator.Close();
            TimeLeftController.Close();
            GameTimer.Close();

            base.Unload();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            Label a = (Label)Objects["Timer"];
            a.Text = String.Format("{0} second(s) left", timeLeft);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            spriteBatch.End();
        }

        public void DetermineCurrentController()
        {
            Random crap = new Random();
            if (ChangeControllerKeyNow)
            {
                CurrentController = (ControllerKeys)crap.Next(0, 3);
                ChangeControllerKeyNow = false;
            }
        }
        public Keys PreviousKey;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Allow keyboard hits
            if (KeybdState.IsKeyDown(Keys.X) && PreviousKey != Keys.X)
            {
                AddSubtractBrickHit(ControllerKeys.X);
                PreviousKey = Keys.X;
            }
            if (KeybdState.IsKeyDown(Keys.A) && PreviousKey != Keys.A)
            {
                AddSubtractBrickHit(ControllerKeys.A);
                PreviousKey = Keys.A;
            }
            if (KeybdState.IsKeyDown(Keys.S) && PreviousKey != Keys.S)
            {
                AddSubtractBrickHit(ControllerKeys.S);
                PreviousKey = Keys.S;
            }
            if (KeybdState.IsKeyDown(Keys.O) && PreviousKey != Keys.O)
            {
                AddSubtractBrickHit(ControllerKeys.O);
                PreviousKey = Keys.O;
            }
            Keys[] pressedKeys = KeybdState.GetPressedKeys();
            if (pressedKeys.Length != 0) PreviousKey = pressedKeys[0];

            // Update object location on viewport change
            Label Timer = (Label)Objects["Timer"];
            Timer.Location = new Vector2(game.GraphicsDevice.Viewport.Width - Timer.Font.MeasureString(Timer.Text).X, 5);
            Objects["Hand1"].Location = new Vector2(game.GraphicsDevice.Viewport.Width - (Objects["Hand1"].Bounds.Width / 2) - 100, game.GraphicsDevice.Viewport.Height - Objects["Hand1"].Bounds.Height + 100);
            Objects["Hand2"].Location = new Vector2(-100, game.GraphicsDevice.Viewport.Height - Objects["Hand2"].Bounds.Height + 100);
            // Align controllers
            Objects["Controller-X"].Location = new Vector2(60, 
                game.GraphicsDevice.Viewport.Height - Objects["Controller-X"].Bounds.Height - 100);
            Objects["Controller-A"].Location = new Vector2(100,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-X"].Bounds.Height - 30);
            Objects["Controller-S"].Location = new Vector2(game.GraphicsDevice.Viewport.Width - Objects["Controller-X"].Bounds.Width - 60,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-X"].Bounds.Height - 100);
            Objects["Controller-O"].Location = new Vector2(game.GraphicsDevice.Viewport.Width - Objects["Controller-X"].Bounds.Width - 100,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-X"].Bounds.Height - 30);
            // Resize game background if necessary
            Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            // Current Controller
            DetermineCurrentController();
            Label pressLabel = (Label)Objects["PressLabel"];
            pressLabel.Text = String.Format("Press/Tap: {0}!", CurrentController.ToString());
            pressLabel.Location = new Vector2(ScreenCenter.X - (fonts["default_m"].MeasureString(pressLabel.Text).X / 2), 80);
            // base
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
        }
    }
}
