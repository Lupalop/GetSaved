﻿using System;
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
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public class GameOneScene : SceneBase
    {
        public GameOneScene(SceneManager SceneManager, Difficulty Difficulty)
            : base(SceneManager, "Game 1 Scene: The Safety Kit")
        {
            GameDifficulty = Difficulty;
        }

        private Collection<string> FallingObjects = new Collection<string> {
			"Medkit", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "!Car",
			"!Donut", "!Shoes", "!Jewelry", "!Ball", "!Wall Clock", "!Chair", "!Bomb"
			};

        private Collection<GenericElement> GameObjects = new Collection<GenericElement>();
        private Collection<GenericElement> CollectedObjects = new Collection<GenericElement>();
        private Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();

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
        private int GenerationInterval;
        private float FallingSpeed;
        private int DistanceFromBottom;
        private bool IsGameEnd = false;

        private SoundEffect ObjectCaught;

        private Random RandNum = new Random();
        private Difficulty GameDifficulty;

        private Timer ProjectileGenerator;
        private Timer TimeLeftController;
        private Timer GameTimer;

        private void InitializeTimer()
        {
            // Initiailize timers
            ProjectileGenerator = new Timer(GenerationInterval) { AutoReset = true, Enabled = true };
            TimeLeftController = new Timer(1000)                { AutoReset = true, Enabled = true };
            GameTimer = new Timer(TimeLeft * 1000)              { AutoReset = false, Enabled = true };
            // Add the event handler to the timer object
            ProjectileGenerator.Elapsed += delegate
            { 
                GenerateFallingItems();
            };
            TimeLeftController.Elapsed += delegate
            {
                if (TimeLeft > 0)
                    TimeLeft--;
            };
            GameTimer.Elapsed += delegate
            {
                IsGameEnd = true;
                SceneManager.Overlays.Add("GameEnd", new GameEndOverlay(SceneManager, Games.FallingObjects, CollectedObjects, this));
            };
        }

        private void GenerateFallingItems()
        {
            if (!IsGameEnd)
            {
                // create new button object
                Image nwBtn = new Image("crap")
                {
                    Graphic = Game.Content.Load<Texture2D>("point"),
                    Location = new Vector2((float)RandNum.Next(5, (int)Game.GraphicsDevice.Viewport.Width - 5), 30),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch
                };
                string tex = FallingObjects[RandNum.Next(0, FallingObjects.Count)];
                nwBtn.MessageHolder.Add(tex);
                if (tex.Contains('!') || tex.Contains('~')) tex = tex.Remove(0, 1);
                nwBtn.Graphic = Images[tex.ToLower()];
                GameObjects.Add(nwBtn);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            foreach (var item in FallingObjects)
            {
                string it = item;
                if (it.Contains('!') || it.Contains('~'))
                    it = it.Remove(0, 1);
                Images.Add(it.ToLower(), Game.Content.Load<Texture2D>("falling-object/" + it));
            }

            Objects = new Dictionary<string, GenericElement> {
                { "GameBG", new Image("GameBG")
                {
                    Graphic = Game.Content.Load<Texture2D>("gameBG1"),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => Objects["GameBG"].DestinationRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height)
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
                { "ObjectCatcher", new Image("ObjectCatcher")
                {
                    Graphic = Game.Content.Load<Texture2D>("falling-object/briefcase"),
                    Location = new Vector2(5, Game.GraphicsDevice.Viewport.Height - 70),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                }},
                { "Timer", new Label("o-timer")
                {
                    Text = String.Format("{0} second(s) left", TimeLeft),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["o-default_l"],
                    OnUpdate = () => {
                        Label Timer = (Label)Objects["Timer"];
                        Timer.Location = new Vector2(Game.GraphicsDevice.Viewport.Width - Timer.Dimensions.X, 5);
                        Timer.Text = String.Format("{0} second(s) left", MathHelper.Clamp((int)TimeLeft, 0, 100));
                    }
                }}
            };

            ObjectCaught = Game.Content.Load<SoundEffect>("sfx/caught");

            SceneManager.AudioManager.PlaySong("hide-seek");
            DistanceFromBottom = -30;
        }

        public override void DelayLoadContent()
        {
            base.DelayLoadContent();

            switch (GameDifficulty)
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
            base.DrawObjects(GameTime, Objects);
            base.DrawObjects(GameTime, GameObjects);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateObjects(GameTime, Objects);
            base.UpdateObjects(GameTime, GameObjects);
            if (Objects.ContainsKey("ObjectCatcher"))
            {
                if (IsGameEnd)
                    Objects.Remove("ObjectCatcher");
                else
                    Objects["ObjectCatcher"].Location = new Vector2(
                        InputManager.MousePosition.X - (Objects["ObjectCatcher"].Graphic.Width / 2),
                        Game.GraphicsDevice.Viewport.Height - Objects["ObjectCatcher"].Bounds.Height + DistanceFromBottom);
            }
            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Moves Game object
                GameObjects[i].Location = new Vector2(GameObjects[i].Location.X, GameObjects[i].Location.Y + FallingSpeed);

                // Check if Game object collides/intersects with catcher
                if (Objects.ContainsKey("ObjectCatcher") && Objects["ObjectCatcher"].Bounds.Intersects(GameObjects[i].Bounds))
                {
                    ObjectCaught.Play();
                    CollectedObjects.Add(GameObjects[i]);
                    GameObjects.Remove(GameObjects[i]);
                    return;
                }

                // Remove objects once it exceeds the object catcher, this also removes all objects when time's up
                if ((Objects.ContainsKey("ObjectCatcher") && (GameObjects[i].Location.Y > Objects["ObjectCatcher"].Location.Y + 50)) || IsGameEnd)
                    GameObjects.Remove(GameObjects[i]);
            }
        }
    }
}
