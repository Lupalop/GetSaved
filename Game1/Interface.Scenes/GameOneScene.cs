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
    public class GameOneScene : SceneBase
    {
        public GameOneScene(SceneManager sceneManager)
            : base(sceneManager, "Game 1 Scene")
        {
            Objects = new ObjectBase[] {
                new MenuButton("mb", sceneManager)
                {
                    Text = "Back",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                },
                new Image("ObjectCatcher")
                {
                    Graphic = game.Content.Load<Texture2D>("Briefcase"),
                    Location = new Vector2(5, 500),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                },
                new MenuButton("timer", sceneManager)
                {
                    Text = "Time Left: " + timeLeft,
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(300, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"]
                }
            }.ToList();
            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            InitializeTimer();

        }

        private List<string> FallingObjects = new List<string> { 
			"Medkit", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "!Car",
			"!Donut", "!Heels", "!Makeup", "!Ball", "!Wall Clock", "!Chair"
			};
        private MouseOverlay MsOverlay;
        private List<ObjectBase> Objects;

        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();

        Timer SomeTimer = new Timer(300);
        Timer GameTimer = new Timer(10000);
        Timer InGameTimer = new Timer(1000);
        private void InitializeTimer()
        {
            // Add the event handler to the timer object
            SomeTimer.Elapsed += OnTimerEnd;
            SomeTimer.AutoReset = true;
            SomeTimer.Enabled = true;

            InGameTimer.Elapsed += UpdateInGameTimer;
            InGameTimer.AutoReset = true;
            InGameTimer.Enabled = true;

            GameTimer.Elapsed += OnGameTimerEnd;
            GameTimer.Enabled = true;
        }

        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            GenerateFallingCrap();
        }

        private void UpdateInGameTimer(Object source, ElapsedEventArgs e)
        {
            if (timeLeft >= 1)
                timeLeft -= 1;
        }

        private bool stopCreatingCrap = false;
        private double timeLeft = 10.0;
        private void OnGameTimerEnd(Object source, ElapsedEventArgs e)
        {
            stopCreatingCrap = true;
            Objects.Add(new MenuButton("TimesUp", sceneManager)
            {
                Graphic = game.Content.Load<Texture2D>("timesUp"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Text = "",
                Font = fonts["default_m"],
                ClickAction = () => game.Exit()
            });
            int correctCrap = 0;
            int wrongCrap = 0;
            // Count correct crap
            foreach (var crap in CollectedObjects)
            {
                if (crap.MessageHolder[0].ToString().Contains('!'))
                    wrongCrap++;
                else
                    correctCrap++;
            }
            Objects.Add(new MenuButton("CorrectCrap", sceneManager) {
                Graphic = game.Content.Load<Texture2D>("holder"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Text = "Correct items: " + correctCrap,
                Font = fonts["default_m"]
            });
            Objects.Add(new MenuButton("CorrectCrap", sceneManager)
            {
                Graphic = game.Content.Load<Texture2D>("holder"),
                Location = ScreenCenter,
                spriteBatch = this.spriteBatch,
                Text = "Incorrect items: " + wrongCrap,
                Font = fonts["default_m"]
            });
            GameTimer.Enabled = false;
        }

        Random randNum = new Random();

        private void GenerateFallingCrap()
        {
            if (!stopCreatingCrap)
            {
                // create new button object
                MenuButton nwBtn = new MenuButton("crap", sceneManager) {
                    Graphic = game.Content.Load<Texture2D>("holder"),
                    Location = new Vector2((float)randNum.Next(5, (int)500 - 5), 30),
                    Font = fonts["default"],
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                };
                string tex = FallingObjects[randNum.Next(0, FallingObjects.Count)];
                nwBtn.MessageHolder.Add(tex);
                if (tex.Contains('!'))
                    tex = tex.Remove(0, 1);
                nwBtn.Text = tex;
                Objects.Add(nwBtn);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.LightGreen);
            base.Draw(gameTime);
            base.DrawObjectsFromBase(gameTime, Objects.ToArray());
            MenuButton a = (MenuButton)Objects[2];
            a.Text = "Time Left: " + timeLeft;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.AlignObjectsToCenterUsingBase(gameTime, Objects.ToArray());
            Objects[1].Location = new Vector2(MsOverlay.mouseBox.X - (Objects[1].Graphic.Width / 2), 500);
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].Name == "crap")
                {
                    // Move crap by 3
                    Objects[i].Location = new Vector2(Objects[i].Location.X, Objects[i].Location.Y + 3);

                    // Check for collisions/intersections
                    if (Objects[1].Bounds.Intersects(Objects[i].Bounds))
                    {
                        CollectedObjects.Add(Objects[i]);
                        Objects.Remove(Objects[i]);
                    }

                    // cleanup crapped shit
                    // (normal human speak: remove objects once it exceeds the object catcher)
                    // also remove all crap once time is up
                    if ((Objects[i].Location.Y > Objects[1].Location.Y + 10) || stopCreatingCrap)
                        Objects.Remove(Objects[i]);
                }
            }
        }
    }
}
