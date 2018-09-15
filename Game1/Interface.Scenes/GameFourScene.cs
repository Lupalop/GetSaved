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
                    Graphic = game.Content.Load<Texture2D>("Hand"),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Columns = 2,
                    Rows = 1
                }},
                { "Hand2", new Image("hand2")
                {
                    Graphic = game.Content.Load<Texture2D>("Hand"),
                    AlignToCenter = false,                    
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    CurrentFrame = 1,
                    Columns = 2,
                    Rows = 1
                }}
            };

            MsOverlay = (MouseOverlay)sceneManager.overlays["mouse"];
            switch (difficulty)
            {
                case Difficulty.Easy:
                    timeLeft = 15.0;
                    projectileInterval = 500;
                    break;
                case Difficulty.Medium:
                    timeLeft = 10.0;
                    projectileInterval = 300;
                    break;
                case Difficulty.Hard:
                    timeLeft = 5.0;
                    projectileInterval = 200;
                    break;
                case Difficulty.EpicFail:
                    timeLeft = 15.0;
                    projectileInterval = 50;
                    break;
            }
            this.difficulty = difficulty;
            InitializeTimer();
        }

        private MouseOverlay MsOverlay;
        private List<ObjectBase> GameObjects = new List<ObjectBase>();
        private List<ObjectBase> CollectedObjects = new List<ObjectBase>();

        private double timeLeft;
        private int projectileInterval;

        private Difficulty difficulty;

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

        public override void Unload()
        {
            //
            ProjectileGenerator.Close();
            TimeLeftController.Close();
            GameTimer.Close();

            base.Unload();
        }

        private void OnProjectileGeneratorEnd(Object source, ElapsedEventArgs e)
        {
        }

        private void OnTimeLeftEnd(Object source, ElapsedEventArgs e)
        {
            if (timeLeft >= 1)
                timeLeft -= 1;
        }

        private void OnGameTimerEnd(Object source, ElapsedEventArgs e)
        {
            sceneManager.overlays.Add("gameEnd", new GameEndOverlay(sceneManager, Games.HelpOthersNow, null, this));
            GameTimer.Enabled = false;
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
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Objects["Hand1"].Location = new Vector2(game.GraphicsDevice.Viewport.Width - (Objects["Hand1"].Bounds.Width / 2) + 100, game.GraphicsDevice.Viewport.Height - Objects["Hand1"].Bounds.Height + 100);
            Objects["Hand2"].Location = new Vector2(-100, game.GraphicsDevice.Viewport.Height - Objects["Hand2"].Bounds.Height + 100);
            Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            base.UpdateObjects(gameTime, Objects);
            base.UpdateObjects(gameTime, GameObjects);
        }
    }
}
