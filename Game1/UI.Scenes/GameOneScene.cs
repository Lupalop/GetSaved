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
    public partial class GameOneScene : Scene
    {
        public GameOneScene(Difficulty Difficulty)
            : base("Game 1 Scene: The Safety Kit")
        {
            GameDifficulty = Difficulty;
        }

        private Collection<string> AvailableItems = new Collection<string> {
			"Medicine", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "!Car",
			"!Donut", "!Shoes", "!Jewelry", "!Ball", "!Wall Clock", "!Chair", "!Bomb"
			};

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
                // TODO: Restore once we get progress bar reimplemented in platform
                /*var a = (ProgressBar)Elements["ProgressBar"];
                a.maximum = (float)value;*/
            }
        }

        private double TimeLeft;
        private int GenerationInterval;
        private int FallingSpeed;
        private bool IsGameEnd = false;

        private SoundEffect ObjectCaught;

        private Random RandNum = new Random();
        private Difficulty gameDifficulty;
        private Difficulty GameDifficulty
        {
            get { return gameDifficulty; }
            set
            {
                gameDifficulty = value;
                switch (value)
                {
                    case Difficulty.Easy:
                        InitialTimeLeft = 25.0;
                        GenerationInterval = 500;
                        FallingSpeed = 3;
                        break;
                    case Difficulty.Medium:
                        InitialTimeLeft = 20.0;
                        GenerationInterval = 300;
                        FallingSpeed = 3;
                        break;
                    case Difficulty.Hard:
                        InitialTimeLeft = 15.0;
                        GenerationInterval = 200;
                        FallingSpeed = 5;
                        break;
                    case Difficulty.EpicFail:
                        InitialTimeLeft = 10.0;
                        GenerationInterval = 50;
                        FallingSpeed = 10;
                        break;
                    case Difficulty.Demo:
                        InitialTimeLeft = 10.0;
                        GenerationInterval = 400;
                        FallingSpeed = 3;
                        break;
                }
            }
        }

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;
        private Timer GameTimer;

        private void InitializeTimer()
        {
            // Initialize timers
            ProjectileGenerator = new Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = GenerationInterval
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
            ProjectileGenerator.Elapsed += CreateFallingItem;
            TimeLeftController.Elapsed += delegate
            {
                if (TimeLeft > 0)
                    TimeLeft--;
            };
            GameTimer.Elapsed += delegate
            {
                IsGameEnd = true;
                Global.Scenes.Overlays.Add("GameEnd",
                    new GameEndOverlay(Games.FallingObjects, CollectedElements, this, GameDifficulty));
            };
        }

        private void CreateFallingItem(object sender, EventArgs eventArgs)
        {
            if (GameDifficulty == Difficulty.Demo) {
                StackPanel container = new StackPanel("container" + RandNum.Next(0, 9999))
                {
                    Orientation = Orientation.Horizontal,
                    Location = new Point(5, -64),
                };
                int ColumnCount = WindowBounds.Width / 64;

                for (int i = 0; i < ColumnCount; i++)
                {
                    FallingItem fallingItem = new FallingItem("falling-item-" + i);
                    string givenName = AvailableItems[RandNum.Next(0, AvailableItems.Count)];
                    if (givenName.Contains('!'))
                    {
                        givenName = givenName.Remove(0, 1);
                    }
                    fallingItem.Graphic = Global.Textures["item-" + givenName];
                    fallingItem.Size = new Point(64);
                    container.Children.Add(i.ToString(), fallingItem);
                }
                GameCanvas.Children.Add(container.Name, container);
                return;
            }

            if (!IsGameEnd)
            {
                FallingItem fallingItem = new FallingItem("falling-item" + DateTime.Now.ToBinary())
                {
                    // Random X, constant TValue initial value
                    Location = new Point(
                        RandNum.Next(5, Game.GraphicsDevice.Viewport.Width - 5), 0),
                };

                int itemID = RandNum.Next(0, AvailableItems.Count);
                string givenName = AvailableItems[itemID];
                if (givenName.Contains('!'))
                {
                    fallingItem.IsEmergencyItem = false;
                    givenName = givenName.Remove(0, 1);
                }

                fallingItem.ItemID = itemID;
                fallingItem.Graphic = Global.Textures[string.Format("item-{0}", givenName)];
                GameCanvas.Children.Add(fallingItem.Name, fallingItem);
            }
        }

        public override void LoadContent()
        {
            InitializeComponent();

            ObjectCaught = Global.SFX["caught"];

            if (GameDifficulty != Difficulty.Demo)
            {
                Global.Audio.PlaySong("hide-seek");
            }

            InitializeTimer();

            // Remove UI and timers
            if (GameDifficulty == Difficulty.Demo)
            {
                TimeLeftController.Close();
                GameTimer.Close();
                Elements.Remove(UICanvas.Name);
            }

            base.LoadContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close all timers
                ProjectileGenerator.Close();
                TimeLeftController.Close();
                GameTimer.Close();

                GuiUtils.DisposeElements(CollectedElements);
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
            for (int i = 0; i < GameCanvas.Children.Count; i++)
            {
                // Positions the falling object
                GameCanvas.Children.Values.ElementAt(i).Location = new Point(
                    GameCanvas.Children.Values.ElementAt(i).Location.X,
                    GameCanvas.Children.Values.ElementAt(i).Location.Y + FallingSpeed);
                // Check if game object intersects with emergency kit
                if (ObjectCatcher.ActualBounds.Intersects(GameCanvas.Children.Values.ElementAt(i).ActualBounds) && GameDifficulty != Difficulty.Demo)
                {
                    ObjectCaught.Play();
                    CollectedElements.Add(GameCanvas.Children.Values.ElementAt(i));
                    GameCanvas.Children.Remove(GameCanvas.Children.Keys.ElementAt(i));
                    return;
                }

                // Remove the object once it reaches the bottom-most part of the window
                // This also removes all the Elements when the time is up
                if ((GameCanvas.Children.Values.ElementAt(i).Location.Y > WindowBounds.Bottom) ||
                    IsGameEnd)
                {
                    GameCanvas.Children.Remove(GameCanvas.Children.Keys.ElementAt(i));
                }
            }

            GuiUtils.UpdateElements(Elements);
        }
    }
}
