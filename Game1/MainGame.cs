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
using System.IO;

namespace Maquina
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private SceneManager SceneManager;
        private LocaleManager LocaleManager;
        private InputManager InputManager;
        private PreferencesManager PreferencesManager;
        private Dictionary<string, SpriteFont> Fonts;
        private Dictionary<string, Song> Songs;

        private int LastWindowWidth;
        private int LastWindowHeight;

        public MainGame()
        {
            // Initialize graphics manager
            Graphics = new GraphicsDeviceManager(this);
            // Set root directory where content files will be loaded
            Content.RootDirectory = Platform.ContentRootDirectory;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create instance
            PreferencesManager = new PreferencesManager();
            LocaleManager = new LocaleManager(PreferencesManager);
            InputManager = new InputManager(this);

            // Use prefs
            IsMouseVisible = PreferencesManager.GetBoolPref("app.window.isMouseVisible", false);
            LastWindowWidth = PreferencesManager.GetIntPref("app.window.width", 800);
            LastWindowHeight = PreferencesManager.GetIntPref("app.window.height", 600);
            Window.AllowUserResizing = PreferencesManager.GetBoolPref("app.window.allowUserResizing", false);
            // Identify if we should go fullscreen
            if (PreferencesManager.GetBoolPref("app.window.fullscreen", false))
            {
                Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;

                Graphics.ToggleFullScreen();
            }
            else
            {
                Graphics.PreferredBackBufferWidth = LastWindowWidth;
                Graphics.PreferredBackBufferHeight = LastWindowHeight;
            }
            Graphics.ApplyChanges();

            // Set window title
            Window.Title = "Get Saved";

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
            resources.Content = resources.Initialize(Path.Combine(
                Platform.ContentRootDirectory, Platform.ResourceXml));
            // Load resources
            Fonts = resources.Content.LoadContent(ResourceType.Fonts, this) as Dictionary<string, SpriteFont>;
            Songs = resources.Content.LoadContent(ResourceType.BGM, this) as Dictionary<string, Song>;
            // Initialize the Scene Manager
            SceneManager = new SceneManager(this, SpriteBatch, Fonts, Songs, LocaleManager, InputManager);
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
            // Save window height if not in fullscreen
            if (!Graphics.IsFullScreen)
            {
                PreferencesManager.SetIntPref("app.window.width", Graphics.PreferredBackBufferWidth);
                PreferencesManager.SetIntPref("app.window.height", Graphics.PreferredBackBufferHeight);
            }
            // Dispose content
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
                    PreferencesManager.SetBoolPref("app.window.fullscreen", false);
                }
                else
                {
                    LastWindowWidth = Graphics.PreferredBackBufferWidth;
                    LastWindowHeight = Graphics.PreferredBackBufferHeight;
                    Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                    Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    PreferencesManager.SetBoolPref("app.window.fullscreen", true);
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
