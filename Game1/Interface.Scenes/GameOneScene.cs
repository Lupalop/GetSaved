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
        public GameOneScene(SceneManager sceneManager, Difficulty difficulty)
            : base(sceneManager, "Game 1 Scene: Falling Objects")
        {
            Objects = new Dictionary<string, ObjectBase> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = game.Content.Load<Texture2D>("GAMEBG"),
                    CustomRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
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
                    Font = fonts["default"],
                    ClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
                { "ObjectCatcher", new Image("ObjectCatcher")
                {
                    Graphic = game.Content.Load<Texture2D>("falling-object/briefcase"),
                    Location = new Vector2(5, 500),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                }},
                { "Timer", new MenuButton("timer", sceneManager)
                {
                    Text = "Time Left: " + timeLeft,
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(300, 5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"]
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 15.0;
                    projectileInterval = 500;
                    FallingSpeed = 3;
                    break;
                case Difficulty.Medium:
                    timeLeft = 10.0;
                    projectileInterval = 300;
                    FallingSpeed = 3;
                    break;
                case Difficulty.Hard:
                    timeLeft = 5.0;
                    projectileInterval = 200;
                    FallingSpeed = 5;
                    break;
                case Difficulty.EpicFail:
                    timeLeft = 15.0;
                    projectileInterval = 50;
                    FallingSpeed = 10;
                    break;
            }

            InitializeTimer();
        }

        private List<string> FallingObjects = new List<string> {
			"Medkit", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "!Car",
			"!Donut", "!Shoes", "!Jewelry", "!Ball", "!Wall Clock", "!Chair", "!Bomb"
			};
        private MouseOverlay MsOverlay;
        private List<ObjectBase> GameObjects = new List<ObjectBase>();
        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();

        private double timeLeft;
        private int projectileInterval;
        private float FallingSpeed;

        Timer ProjectileGenerator;
        Timer TimeLeftController;
        Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(projectileInterval);
            TimeLeftController = new Timer(1000);
            GameTimer = new Timer(timeLeft * 1000);
            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += OnProjectileGeneratorEnd;
            ProjectileGenerator.AutoReset = true;
            ProjectileGenerator.Enabled = true;

            TimeLeftController.Elapsed += OnTimeLeftEnd;
            TimeLeftController.AutoReset = true;
            TimeLeftController.Enabled = true;

            GameTimer.Elapsed += OnGameTimerEnd;
            GameTimer.AutoReset = false;
            GameTimer.Enabled = true;
        }

        private void OnProjectileGeneratorEnd(Object source, ElapsedEventArgs e)
        {
            GenerateFallingCrap();
        }

        private void OnTimeLeftEnd(Object source, ElapsedEventArgs e)
        {
            if (timeLeft >= 1)
                timeLeft -= 1;
        }

        private bool stopCreatingCrap = false;

        private void OnGameTimerEnd(Object source, ElapsedEventArgs e)
        {
            stopCreatingCrap = true;
            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.FallingObjects, CollectedObjects));
            GameTimer.Enabled = false;
        }

        Random randNum = new Random();

        private void GenerateFallingCrap()
        {
            if (!stopCreatingCrap)
            {
                // create new button object
                Image nwBtn = new Image("crap") {
                    Graphic = game.Content.Load<Texture2D>("holder"),
                    Location = new Vector2((float)randNum.Next(5, (int)game.GraphicsDevice.Viewport.Width - 5), 30),
                    //Font = fonts["default"],
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch
                };
                string tex = FallingObjects[randNum.Next(0, FallingObjects.Count)];
                nwBtn.MessageHolder.Add(tex);
                if (tex.Contains('!') || tex.Contains('~')) tex = tex.Remove(0, 1);
                nwBtn.Graphic = game.Content.Load<Texture2D>("falling-object/" + tex.ToLower());
                GameObjects.Add(nwBtn);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.LightSalmon);
            MenuButton a = (MenuButton)Objects["Timer"];
            a.Text = "Time Left: " + timeLeft;
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            base.DrawObjects(gameTime, GameObjects);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Objects["GameBG"].CustomRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Move crap by 3
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X, GameObjects[i].Location.Y + FallingSpeed);

                // Check if game object collides/intersects with catcher
                if (Objects.ContainsKey("ObjectCatcher") && Objects["ObjectCatcher"].Bounds.Intersects(GameObjects[i].Bounds))
                {
                    CollectedObjects.Add(GameObjects[i]);
                    GameObjects.Remove(GameObjects[i]);
                }

                // cleanup crapped shit
                // (normal human speak: remove objects once it exceeds the object catcher)
                // also remove all crap once time is up
                if ((Objects.ContainsKey("ObjectCatcher") && (GameObjects[i].Location.Y > Objects["ObjectCatcher"].Location.Y + 50)) || stopCreatingCrap)
                    GameObjects.Remove(GameObjects[i]);
            }
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                if (stopCreatingCrap)
                    Objects.Remove("ObjectCatcher");
                else
                    Objects["ObjectCatcher"].Location = new Vector2(MsOverlay.mouseBox.X - (Objects["ObjectCatcher"].Graphic.Width / 2), 500);
            }
        }
    }
}
