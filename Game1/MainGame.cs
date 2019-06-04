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
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;

namespace Maquina
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : MaquinaGame
    {
        public MainGame()
        {
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

            // Set window title
            Window.Title = "Get Saved";

            UserGlobal.UserName = PreferencesManager.GetCharPref("game.username", "Guest");
            UserGlobal.Score = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override async void LoadContent()
        {
            base.LoadContent();

            if (!IsMouseVisible)
            {
                SceneManager.Overlays.Add("mouse", new MouseOverlay(Global.Textures["cursor-default"]));
            }

            await Task.Run(() =>
            {
                ContentLoader<ResourceContent> resources = new ContentLoader<ResourceContent>();
                // Load game-specific resources
                resources.Content = resources.Initialize(
                    Path.Combine(Global.ContentRootDirectory, "gameresources.xml"));
                // Load resources
                Global.Fonts = Global.Fonts.MergeWith(
                    resources.Content.Load(ResourceType.Fonts) as Dictionary<string, SpriteFont>);
                Global.BGM = Global.BGM.MergeWith(
                    resources.Content.Load(ResourceType.BGM) as Dictionary<string, Song>);
                Global.SFX = Global.SFX.MergeWith(
                    resources.Content.Load(ResourceType.SFX) as Dictionary<string, SoundEffect>);
                Global.Textures = Global.Textures.MergeWith(
                    resources.Content.Load(ResourceType.Textures) as Dictionary<string, Texture2D>);
#if DEBUG
                SceneManager.Overlays.Add("debug", new DebugOverlay());
#endif
                // Setup first scene (Main Menu)
                SceneManager.SwitchToScene(new MainMenuScene());
            });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (UserGlobal.UserName != "Guest")
            {
#if HAS_CONSOLE && LOG_GENERAL
                Console.WriteLine("Saving user information");
#endif
                UserGlobal.SaveCurrentUser();
                UserGlobal.SetNewHighscore();
            }
            
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Back/Esc.
            if (InputManager.GamepadState.Buttons.Back == ButtonState.Pressed)
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
                    LastWindowWidth = Graphics.PreferredBackBufferWidth;
                    LastWindowHeight = Graphics.PreferredBackBufferHeight;
                    Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                    Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                }
                Graphics.ToggleFullScreen();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
