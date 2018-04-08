﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface;

namespace Arkabound
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SceneManager sceneManager;
        Dictionary<string, SpriteFont> fonts;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Setup any components
            Components.Add(new Arkabound.Components.TimerManager(this));
            // Initialize the Fonts dictionary
            fonts = new Dictionary<string, SpriteFont>();
            // Base init
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load all fonts into the Fonts dictionary
            fonts["default"] = Content.Load<SpriteFont>("ZillaSlab");
            fonts["symbol"] = Content.Load<SpriteFont>("ZillaSlabSymbols");
            // Setup FPS counter
            Components.Add(new Arkabound.Components.FPSCounter(this, spriteBatch, fonts["default"]));
            // Setup the Scene Manager
            sceneManager = new SceneManager(this, spriteBatch, fonts);
            sceneManager.currentScene = new StartupScene(sceneManager);
            // Initialize the mouse as an overlay
            sceneManager.overlays.Add(new MouseScene(sceneManager));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState KeybdState = Keyboard.GetState();
            GamePadState GamePdState = GamePad.GetState(PlayerIndex.One);
            MouseState MsState = Mouse.GetState();

            if (GamePdState.Buttons.Back == ButtonState.Pressed || KeybdState.IsKeyDown(Keys.Escape))
                Exit();
            if ((KeybdState.IsKeyDown(Keys.RightAlt) || KeybdState.IsKeyDown(Keys.LeftAlt)) && KeybdState.IsKeyDown(Keys.Enter))
                graphics.ToggleFullScreen();

            sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            sceneManager.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
