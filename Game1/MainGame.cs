using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Maquina.UI;
using Maquina.UI.Scenes;
using Maquina.Resources;

namespace Maquina
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        SceneManager SceneManager;
        InputManager InputManager;
        Dictionary<string, SpriteFont> Fonts;
        Dictionary<string, Song> Songs;

        private int LastWindowWidth = 800;
        private int LastWindowHeight = 600;

        public MainGame()
        {
            // Initialize graphics manager
            Graphics = new GraphicsDeviceManager(this);
            // Set root directory where content files will be loaded
            Content.RootDirectory = Platform.ContentRootDirectory;
            // Make the mouse invisible (perhaps, this should be placed somewhere else)
            IsMouseVisible = false;
            // Set the default window size to (800x600)
            Graphics.PreferredBackBufferWidth = LastWindowWidth;
            Graphics.PreferredBackBufferHeight = LastWindowHeight;
            // Allow the window to be resized by the user
            Window.AllowUserResizing = false;
            Window.Title = "Get Saved";
            // Initialize the Fonts dictionary
            Fonts = new Dictionary<string, SpriteFont>();
            Songs = new Dictionary<string, Song>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create instance of SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            // Create instance of the Content Manager
            ContentManager<ResourceContent> resources = new ContentManager<ResourceContent>();
            resources.Content = resources.Initialize(Utils.CreateLocation(
                new string[] { Platform.ContentRootDirectory, Platform.ResourceXml }));
            // Load resources
            Fonts = resources.Content.LoadContent(ResourceType.Fonts, this) as Dictionary<string, SpriteFont>;
            Songs = resources.Content.LoadContent(ResourceType.BGM, this) as Dictionary<string, Song>;
            // Create instance
            InputManager = new InputManager(this);
            // Initialize the Scene Manager
            SceneManager = new SceneManager(this, SpriteBatch, Fonts, Songs, null, InputManager);
            // Register Overlays in the scene manager
            SceneManager.Overlays.Add("mouse", new MouseOverlay(SceneManager, Content.Load<Texture2D>("mouseCur")));
#if DEBUG
            SceneManager.Overlays.Add("debug", new DebugOverlay(SceneManager));
#endif
            // Setup first scene (Main Menu)
            SceneManager.SwitchToScene(new MainMenuScene(SceneManager), true);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
#if DEBUG
            Console.WriteLine("Unloading game content");
#endif
            SceneManager.Dispose();
            SpriteBatch.Dispose();
            Graphics.Dispose();
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Back/Esc.
            if (InputManager.GamepadState.Buttons.Back == ButtonState.Pressed || InputManager.KeyPressed(Keys.Escape))
                Exit();

            // Alt + Enter
            if ((InputManager.KeyDown(Keys.RightAlt) || InputManager.KeyDown(Keys.LeftAlt)) && InputManager.KeyPressed(Keys.Enter))
            {
                if (Graphics.IsFullScreen)
                {
                    Graphics.PreferredBackBufferHeight = LastWindowHeight;
                    Graphics.PreferredBackBufferWidth = LastWindowWidth;
                }
                else
                {
                    Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                    Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                }
                Graphics.ToggleFullScreen();
            }

            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Scale images using nearest neighbor
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            SceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
