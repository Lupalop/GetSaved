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
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameFourScene : SceneBase
    {
        public GameFourScene(SceneManager SceneManager, Difficulty Difficulty)
            : base(SceneManager, "Game 4 Scene: Aid 'em")
        {
            GameDifficulty = Difficulty;
        }

        private Dictionary<string, GenericElement> GameObjects = new Dictionary<string, GenericElement>();
        private Collection<GenericElement> CollectedObjects = new Collection<GenericElement>();

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
        private int ProjectileInterval;
        private int HitsBeforeSaved;

        private ControllerKeys CurrentController;
        private Difficulty GameDifficulty;

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;
        private Timer GameTimer;

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
                if (TimeLeft > 0)
                    TimeLeft--;
            };
            GameTimer.Elapsed += delegate
            {
                ProjectileGenerator.Close();
                AttemptRemoveHelpman();

                SceneManager.Overlays.Add("GameEnd", new GameEndOverlay(SceneManager, Games.HelpOthersNow, CollectedObjects, this));
                GameTimer.Enabled = false;
            };
        }

        private void ProjectileGenerator_Elapsed(object sender, ElapsedEventArgs e)
        {
            AttemptRemoveHelpman();
            GameObjects.Add("helpman", new Helpman("helpman")
            {
                Graphic = Game.Content.Load<Texture2D>("aid-em/helpman"),
                OnUpdate = () =>
                {
                    Helpman helpman = (Helpman)GameObjects["helpman"];
                    helpman.Location = new Vector2(ScreenCenter.X - helpman.Dimensions.X / 2, ScreenCenter.Y - helpman.Dimensions.Y / 2);
                },
                SpriteBatch = this.SpriteBatch,
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
                    CreateFade(Color.Red);
                    CreateFlash("dead", 1.4f);
                    helpman.MessageHolder.Add("!");
                }
                else
                {
                    CreateFade(Color.Green);
                    CreateFlash("saved", 1.4f);
                    helpman.MessageHolder.Add("~");
                }
                CollectedObjects.Add(helpman);
                GameObjects.Remove("helpman");
            }
        }

        private void CreateFlash(string resource, float scale = 1f)
        {
            string overlayName = String.Format("flash-{0}-{1}", DateTime.Now, new Random().Next(0, 1000));
            try
            {
                SceneManager.Overlays.Add(overlayName,
                    new FlashOverlay(SceneManager,
                        overlayName, Game.Content.Load<Texture2D>(resource), scale) { FadeSpeed = 0.01f });
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private void CreateFade(Color color)
        {
            string overlayName = String.Format("fade-{0}-{1}", DateTime.Now, new Random().Next(0, 1000));
            try
            {
                SceneManager.Overlays.Add(overlayName,
                    new FadeOverlay(SceneManager, overlayName, color) { FadeSpeed = 0.01f });
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void AddSubtractBrickHit(ControllerKeys cKey)
        {
            if (GameObjects.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameObjects["helpman"];
                if (CurrentController == cKey)
                {
                    helpman.HitsBeforeBreak--;
                    CreateFlash("check");
                }
                else
                {
                    CreateFlash("cross");
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

            Objects = new Dictionary<string, GenericElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Game.Content.Load<Texture2D>("gameBG2"),
                    ControlAlignment = ControlAlignment.Fixed,
                    OnUpdate = () => Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height),
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
                { "Hand1", new Image("hand1")
                {
                    Graphic = Game.Content.Load<Texture2D>("aid-em/Hand"),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    Columns = 2,
                    Rows = 1
                }},
                { "Hand2", new Image("hand2")
                {
                    Graphic = Game.Content.Load<Texture2D>("aid-em/Hand"),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    CurrentFrame = 1,
                    Columns = 2,
                    Rows = 1
                }},
                { "Controller-Bandage", new MenuButton("controller-x", SceneManager)
                {
                    Text = "Bandage (X)",
                    Graphic = Game.Content.Load<Texture2D>("controllerBtn"),
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    ControlAlignment = ControlAlignment.Fixed,
                    Location = new Vector2(80,460),
                    Font = Fonts["default_m"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Bandage)
                }},
                { "Controller-Stitch", new MenuButton("controller-a", SceneManager)
                {
                    Text = "Stitch (A)",
                    Graphic = Game.Content.Load<Texture2D>("controllerBtn"),
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    ControlAlignment = ControlAlignment.Fixed,
                    Location = new Vector2(150, 530),
                    Font = Fonts["default_m"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Stitch)
                }},
                { "Controller-Medicine", new MenuButton("controller-s", SceneManager)
                {
                    Text = "Medicine (S)",
                    Graphic = Game.Content.Load<Texture2D>("controllerBtn"),
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    ControlAlignment = ControlAlignment.Fixed,
                    Location = new Vector2(730,460),
                    Font = Fonts["default_m"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Medicine)
                }},
                { "Controller-CPR", new MenuButton("controller-o", SceneManager)
                {
                    Text = "CPR (O)",
                    Graphic = Game.Content.Load<Texture2D>("controllerBtn"),
                    SpriteBatch = this.SpriteBatch,
                    SpriteType = SpriteType.Static,
                    ControlAlignment = ControlAlignment.Fixed,
                    Location = new Vector2(650, 550),
                    Font = Fonts["default_m"],
                    Columns = 3,
                    Rows = 1,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.CPR)
                }},
                { "PressLabel", new Label("press-label")
                {
                    Text = String.Format("Press/Tap: {0}!", CurrentController.ToString()),
                    SpriteBatch = this.SpriteBatch,
                    ControlAlignment = ControlAlignment.Fixed,
                    Font = Fonts["default_m"]
                }}
            };

            SceneManager.AudioManager.PlaySong("flying-high");
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    InitialTimeLeft = 24.0;
                    ProjectileInterval = 6000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.Medium:
                    InitialTimeLeft = 15.0;
                    ProjectileInterval = 5000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.Hard:
                    InitialTimeLeft = 12.0;
                    ProjectileInterval = 4000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.EpicFail:
                    InitialTimeLeft = 50.0;
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

        public override void Draw(GameTime GameTime)
        {
            SpriteBatch.Begin();
            base.Draw(GameTime);
            Label a = (Label)Objects["Timer"];
            a.Text = String.Format("{0} second(s) left", TimeLeft);
            base.DrawObjects(GameTime, Objects);
            base.DrawObjects(GameTime, GameObjects);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            // Allow keyboard hits
            if (InputManager.KeyPressed(Keys.X))
            {
                AddSubtractBrickHit(ControllerKeys.Bandage);
            }
            if (InputManager.KeyPressed(Keys.A))
            {
                AddSubtractBrickHit(ControllerKeys.Stitch);
            }
            if (InputManager.KeyPressed(Keys.S))
            {
                AddSubtractBrickHit(ControllerKeys.Medicine);
            }
            if (InputManager.KeyPressed(Keys.O))
            {
                AddSubtractBrickHit(ControllerKeys.CPR);
            }
            // Update object location on viewport change
            Label Timer = (Label)Objects["Timer"];
            Timer.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Timer.Font.MeasureString(Timer.Text).X, 5);
            Objects["Hand1"].Location = new Vector2(Game.GraphicsDevice.Viewport.Width - (Objects["Hand1"].Bounds.Width / 2) - 100, Game.GraphicsDevice.Viewport.Height - Objects["Hand1"].Bounds.Height + 100);
            Objects["Hand2"].Location = new Vector2(-100, Game.GraphicsDevice.Viewport.Height - Objects["Hand2"].Bounds.Height + 100);
            // Align controllers
            Objects["Controller-Bandage"].Location = new Vector2(60,
                Game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 100);
            Objects["Controller-Stitch"].Location = new Vector2(100,
                Game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 30);
            Objects["Controller-Medicine"].Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Objects["Controller-Bandage"].Bounds.Width - 60,
                Game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 100);
            Objects["Controller-CPR"].Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Objects["Controller-Bandage"].Bounds.Width - 100,
                Game.GraphicsDevice.Viewport.Height - Objects["Controller-Bandage"].Bounds.Height - 30);
            // Current Controller
            DetermineCurrentController();
            Label pressLabel = (Label)Objects["PressLabel"];
            pressLabel.Text = String.Format("Press/Tap: {0}!", CurrentController.ToString());
            pressLabel.Location = new Vector2(ScreenCenter.X - (Fonts["default_m"].MeasureString(pressLabel.Text).X / 2), 80);
            // base
            base.UpdateObjects(GameTime, Objects);
            base.UpdateObjects(GameTime, GameObjects);
        }
    }
}
