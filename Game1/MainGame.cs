using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Maquina.UI;
using Maquina.UI.Scenes;
using Maquina.Content;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;
using Maquina.Entities;

namespace Maquina
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : MaquinaGame
    {
        public MainGame()
        {
            Content.RootDirectory = "Content";
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

            UserApplication.UserName = Application.Preferences.GetString("game.username", "Guest");
            UserApplication.Score = 0;
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
                Texture2D defaultTexture = (Texture2D)ContentFactory.TryGetResource("cursor-default");
                Application.SoftwareMouse.Sprite = new TextureAtlasSprite(defaultTexture, 2, 1);
            }

            await Task.Run(() =>
            {
                // Load game-specific resources
                string resourcePath = Path.Combine(Content.RootDirectory, ResourceXml);
                ContentManifest resources = XmlHelper.Load<ContentManifest>(resourcePath);
                ContentFactory.Source.Add("application-startup", resources.Load("general"));
#if DEBUG
                Application.Scenes.Overlays.Add(new DebugOverlay());
#endif
                // Setup first scene (Main Menu)
                Application.Scenes.SwitchToScene(new MainMenuScene());
            });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (UserApplication.UserName != "Guest")
            {
#if HAS_CONSOLE && LOG_GENERAL
                Console.WriteLine("Saving user information");
#endif
                UserApplication.SaveCurrentUser();
                UserApplication.SetNewHighscore();
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
            // Gamepad Back
            if (Application.Input.GamepadState.Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }

            // Alt + Enter
            if ((Application.Input.KeyDown(Keys.RightAlt) ||
                Application.Input.KeyDown(Keys.LeftAlt)) && Application.Input.KeyPressed(Keys.Enter))
            {
                Application.Display.ToggleFullScreen();
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
