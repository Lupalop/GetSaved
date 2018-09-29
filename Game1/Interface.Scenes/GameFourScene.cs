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
    public class GameFourScene : SceneBase
    {
        public GameFourScene(SceneManager sceneManager, Difficulty Difficulty)
            : base(sceneManager, "Game 4 Scene: Aid 'em")
        {
            GameDifficulty = Difficulty;
        }

        private MouseOverlay MsOverlay;
        private Dictionary<string, ObjectBase> GameObjects = new Dictionary<string, ObjectBase>();
        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();

        private double TimeLeft;
        private int ProjectileInterval;
        private int HitsBeforeSaved;

        private ControllerKeys CurrentController;
        private Difficulty GameDifficulty;

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;
        private Timer GameTimer;

        private Keys PreviousKey;
        private bool ChangeControllerKeyNow = true;
        private enum ControllerKeys { Bandage, Stitch, Medicine, CPR }

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(ProjectileInterval) { AutoReset = true, Enabled = true };
            TimeLeftController = new Timer(1000) { AutoReset = true, Enabled = true };
            GameTimer = new Timer(TimeLeft * 1000) { AutoReset = false, Enabled = true };

            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += ProjectileGenerator_Elapsed;
            ProjectileGenerator_Elapsed(null, null);
            TimeLeftController.Elapsed += delegate
            {
                if (TimeLeft >= 1)
                    TimeLeft -= 1;
            };
            GameTimer.Elapsed += delegate
            {
                ProjectileGenerator.Close();
                AttemptRemoveHelpman();

                sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.HelpOthersNow, CollectedObjects, this));
                GameTimer.Enabled = false;
            };
        }

        private void ProjectileGenerator_Elapsed(object sender, ElapsedEventArgs e)
        {
            AttemptRemoveHelpman();
            GameObjects.Add("helpman", new Helpman("helpman")
            {
                Graphic = game.Content.Load<Texture2D>("aid-em/helpman"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                HitsBeforeBreak = HitsBeforeSaved
            });
        }

        private void AttemptRemoveHelpman()
        {
            if (GameObjects.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameObjects["helpman"];
                if (helpman.HitsBeforeBreak > 0)
                {
                    string overlayName = String.Format("fade-{0}", DateTime.Now);
                    sceneManager.overlays.Add(overlayName, new FadeOverlay(sceneManager, overlayName, Color.DarkRed) { FadeSpeed = 0.1f });
                    helpman.MessageHolder.Add("!");
                }
                else
                {
                    string overlayName = String.Format("fade-{0}", DateTime.Now);
                    sceneManager.overlays.Add(overlayName, new FadeOverlay(sceneManager, overlayName, Color.LightGreen) { FadeSpeed = 0.1f });
                    helpman.MessageHolder.Add("~");
                }
                CollectedObjects.Add(helpman);
                GameObjects.Remove("helpman");
            }
        }

        private void AddSubtractBrickHit(ControllerKeys cKey)
        {
            if (GameObjects.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameObjects["helpman"];
                if (CurrentController == cKey)
                {
                    helpman.HitsBeforeBreak--;
                }
                else
                {
                    string overlayName = String.Format("fade-{0}-{1}", DateTime.Now, new Random().Next(0, 1000));
                    try
                    {
                        sceneManager.overlays.Add(overlayName, new FadeOverlay(sceneManager, overlayName, Color.Red) { FadeSpeed = 0.1f });
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                }
                ChangeControllerKeyNow = true;
                if (helpman.HitsBeforeBreak <= 0)
                    AttemptRemoveHelpman();
            }
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

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("gameBG2"),
                    AlignToCenter = false,
                    OnUpdate = () => Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
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
                { "Timer", new Label("timer")
                {
                    Text = String.Format("{0} second(s) left", TimeLeft),
                    Location = new Vector2(game.GraphicsDevice.Viewport.Width - 305, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["o-default_l"]
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
                { "Controller-Bandage", new MenuButton("controller-x", sceneManager)
                {
                    Text = "Bandage (X)",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(80,460),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Bandage)
                }},
                { "Controller-Stitch", new MenuButton("controller-a", sceneManager)
                {
                    Text = "Stitch (A)",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(150, 530),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Stitch)
                }},
                { "Controller-Medicine", new MenuButton("controller-s", sceneManager)
                {
                    Text = "Medicine (S)",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(730,460),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Medicine)
                }},
                { "Controller-CPR", new MenuButton("controller-o", sceneManager)
                {
                    Text = "CPR (O)",
                    Graphic = game.Content.Load<Texture2D>("controllerBtn"),
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = new Vector2(650, 550),
                    Font = fonts["default_l"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.CPR)
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
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    TimeLeft = 24.0;
                    ProjectileInterval = 6000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.Medium:
                    TimeLeft = 15.0;
                    ProjectileInterval = 5000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.Hard:
                    TimeLeft = 12.0;
                    ProjectileInterval = 4000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.EpicFail:
                    TimeLeft = 50.0;
                    ProjectileInterval = 5000;
                    HitsBeforeSaved = 10;
                    break;
            }

            InitializeTimer();
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
            a.Text = String.Format("{0} second(s) left", TimeLeft);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Allow keyboard hits
            if (KeybdState.IsKeyDown(Keys.X) && (PreviousKey != Keys.X || CurrentController == ControllerKeys.Bandage))
            {
                AddSubtractBrickHit(ControllerKeys.Bandage);
                PreviousKey = Keys.X;
            }
            if (KeybdState.IsKeyDown(Keys.A) && (PreviousKey != Keys.A || CurrentController == ControllerKeys.Stitch))
            {
                AddSubtractBrickHit(ControllerKeys.Stitch);
                PreviousKey = Keys.A;
            }
            if (KeybdState.IsKeyDown(Keys.S) && (PreviousKey != Keys.S || CurrentController == ControllerKeys.Medicine))
            {
                AddSubtractBrickHit(ControllerKeys.Medicine);
                PreviousKey = Keys.S;
            }
            if (KeybdState.IsKeyDown(Keys.O) && (PreviousKey != Keys.O || CurrentController == ControllerKeys.CPR))
            {
                AddSubtractBrickHit(ControllerKeys.CPR);
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
            Objects["Controller-Bandage"].Location = new Vector2(60,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 100);
            Objects["Controller-Stitch"].Location = new Vector2(100,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 30);
            Objects["Controller-Medicine"].Location = new Vector2(game.GraphicsDevice.Viewport.Width - Objects["Controller-Bandage"].Bounds.Width - 60,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 100);
            Objects["Controller-CPR"].Location = new Vector2(game.GraphicsDevice.Viewport.Width - Objects["Controller-Bandage"].Bounds.Width - 100,
                game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 30);
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
