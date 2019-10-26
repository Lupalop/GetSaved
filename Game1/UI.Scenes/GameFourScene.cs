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
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameFourScene : Scene
    {
        public GameFourScene(Difficulty Difficulty) : base("Game 4 Scene: Aid 'em")
        {
            GameDifficulty = Difficulty;
        }

        private Dictionary<string, BaseElement> GameElements = new Dictionary<string, BaseElement>();
        private Collection<BaseElement> CollectedElements = new Collection<BaseElement>();

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
            ProjectileGenerator = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = ProjectileInterval
            };
            TimeLeftController = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 1000
            };
            GameTimer = new Timer()
            {
                AutoReset = false,
                Enabled = true,
                Interval = TimeLeft * 1000
            };

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

                Global.Scenes.Overlays.Add("GameEnd", new GameEndOverlay(Games.HelpOthersNow, CollectedElements, this, GameDifficulty));
                GameTimer.Enabled = false;
            };
        }

        private void ProjectileGenerator_Elapsed(object sender, EventArgs e)
        {
            AttemptRemoveHelpman();
            GameElements.Add("helpman", new Helpman("helpman")
            {
                Graphic = Global.Textures["helpman"],
                OnUpdate = (element) =>
                {
                    Helpman helpman = (Helpman)element;
                    helpman.Location = new Vector2(ScreenCenter.X - helpman.Size.X / 2, ScreenCenter.Y - helpman.Size.Y / 2);
                },
                HitsBeforeBreak = HitsBeforeSaved
            });
        }

        private void AttemptRemoveHelpman()
        {
            if (GameElements.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameElements["helpman"];
                if (helpman.HitsBeforeBreak > 0)
                {
                    CreateFade(Color.Red);
                    CreateFlash("dead", 1.4f, 1000);
                    helpman.IsAlive = false;
                }
                else
                {
                    CreateFade(Color.Green);
                    CreateFlash("saved", 1.4f, 1000);
                }
                CollectedElements.Add(helpman);
                GameElements.Remove("helpman");
            }
        }

        private void CreateFlash(string resource, float scale = 1f, int delay = 0)
        {
            // Try to remove previous flashes
            SceneManager.TryRemoveOverlays("flash");

            string overlayName = String.Format("flash-{0}-{1}{2}", DateTime.Now, new Random().Next(0, 1000), resource);
            try
            {
                Global.Scenes.Overlays.Add(overlayName,
                    new FlashOverlay(overlayName,
                        Global.Textures[resource], scale,
                        delay) { FadeSpeed = 0.1f });
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
        private void CreateFade(Color color)
        {
            string overlayName = String.Format("fade-{0}-{1}", DateTime.Now, new Random().Next(0, 1000));
            try
            {
                Global.Scenes.Overlays.Add(overlayName,
                    new FadeOverlay(overlayName, color) { FadeSpeed = 0.01f });
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void AddSubtractBrickHit(ControllerKeys cKey)
        {
            if (GameElements.ContainsKey("helpman"))
            {
                Helpman helpman = (Helpman)GameElements["helpman"];
                if (CurrentController == cKey)
                {
                    helpman.HitsBeforeBreak--;
                    CreateFlash("check", 1f, 500);
                }
                else
                {
                    CreateFlash("cross", 1f, 500);
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
                CurrentController = (ControllerKeys)crap.Next(0, 4);
                ChangeControllerKeyNow = false;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Elements = new Dictionary<string, BaseElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Global.Textures["game-bg-2"],
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        element.DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
                    }
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
                { "Hand1", new Image("hand1")
                {
                    Graphic = Global.Textures["hand"],
                    ControlAlignment = Alignment.Fixed,
                    SpriteType = SpriteType.Static,
                    Columns = 2,
                    Rows = 1
                }},
                { "Hand2", new Image("hand2")
                {
                    Graphic = Global.Textures["hand"],
                    ControlAlignment = Alignment.Fixed,
                    SpriteType = SpriteType.Static,
                    CurrentFrame = 1,
                    Columns = 2,
                    Rows = 1
                }},
                { "Controller-Bandage", new MenuButton("controller-x")
                {
                    Tooltip = "Bandage (X)",
                    Graphic = Global.Textures["bandage"],
                    SpriteType = SpriteType.None,
                    ControlAlignment = Alignment.Fixed,
                    Location = new Vector2(80,460),
                    LayerDepth = 0.1f,
                    Scale = 1.3f,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Bandage)
                }},
                { "Controller-Stitch", new MenuButton("controller-a")
                {
                    Tooltip = "Stitch (A)",
                    Graphic = Global.Textures["stitch"],
                    SpriteType = SpriteType.None,
                    ControlAlignment = Alignment.Fixed,
                    Location = new Vector2(150, 530),
                    LayerDepth = 0.1f,
                    Scale = 1.3f,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Stitch)
                }},
                { "Controller-Medicine", new MenuButton("controller-s")
                {
                    Tooltip = "Medicine (S)",
                    Graphic = Global.Textures["medicine"],
                    SpriteType = SpriteType.None,
                    ControlAlignment = Alignment.Fixed,
                    Location = new Vector2(730,460),
                    LayerDepth = 0.1f,
                    Scale = 1.3f,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.Medicine)
                }},
                { "Controller-CPR", new MenuButton("controller-o")
                {
                    Tooltip = "CPR (O)",
                    Graphic = Global.Textures["cpr"],
                    SpriteType = SpriteType.None,
                    ControlAlignment = Alignment.Fixed,
                    Location = new Vector2(650, 550),
                    LayerDepth = 0.1f,
                    Scale = 1.3f,
                    LeftClickAction = () => AddSubtractBrickHit(ControllerKeys.CPR)
                }},
                { "PressLabel", new Label("press-label")
                {
                    Text = String.Format("Use {0}!", CurrentController.ToString()),
                    ControlAlignment = Alignment.Fixed,
                    Font = Global.Fonts["default_l"]
                }}
            };

            Global.AudioManager.PlaySong("flying-high");
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
            {
                case Difficulty.Easy:
                    InitialTimeLeft = 60.0;
                    ProjectileInterval = 10000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.Medium:
                    InitialTimeLeft = 30.0;
                    ProjectileInterval = 8000;
                    HitsBeforeSaved = 5;
                    break;
                case Difficulty.Hard:
                    InitialTimeLeft = 20.0;
                    ProjectileInterval = 6000;
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
            DisposeElements(GameElements);
            DisposeElements(CollectedElements);

            base.Unload();
        }

        public override void Draw(GameTime GameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(GameTime);
            Label a = (Label)Elements["Timer"];
            a.Text = String.Format("{0} second(s) left", TimeLeft);
            GuiUtils.DrawElements(GameTime, Elements);
            GuiUtils.DrawElements(GameTime, GameElements);
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
            Label Timer = (Label)Elements["Timer"];
            Timer.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Timer.Font.MeasureString(Timer.Text).X, 5);
            Elements["Hand1"].Location = new Vector2(Game.GraphicsDevice.Viewport.Width - (Elements["Hand1"].Bounds.Width / 2) - 100, Game.GraphicsDevice.Viewport.Height - Elements["Hand1"].Bounds.Height + 100);
            Elements["Hand2"].Location = new Vector2(-100, Game.GraphicsDevice.Viewport.Height - Elements["Hand2"].Bounds.Height + 100);
            // Align controllers
            Elements["Controller-Bandage"].Location = new Vector2(80,
                Game.GraphicsDevice.Viewport.Height - Elements["Controller-Bandage"].Bounds.Height - 100);
            Elements["Controller-Stitch"].Location = new Vector2(250,
                Game.GraphicsDevice.Viewport.Height - Elements["Controller-Bandage"].Bounds.Height - 30);
            Elements["Controller-Medicine"].Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Elements["Controller-Bandage"].Bounds.Width - 80,
                Game.GraphicsDevice.Viewport.Height - Elements["Controller-Bandage"].Bounds.Height - 100);
            Elements["Controller-CPR"].Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Elements["Controller-Bandage"].Bounds.Width - 250,
                Game.GraphicsDevice.Viewport.Height - Elements["Controller-Bandage"].Bounds.Height - 30);
            // Current Controller
            DetermineCurrentController();
            Label pressLabel = (Label)Elements["PressLabel"];
            pressLabel.Text = String.Format("Use {0}!", CurrentController.ToString());
            pressLabel.Location = new Vector2(
                ScreenCenter.X -(Global.Fonts["default_l"].MeasureString(pressLabel.Text).X / 2), 80);
            // base
            GuiUtils.UpdateElements(GameTime, Elements);
            GuiUtils.UpdateElements(GameTime, GameElements);
        }
    }
}
