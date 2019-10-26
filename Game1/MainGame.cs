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

            UserGlobal.UserName = PreferencesManager.GetStringPreference("game.username", "Guest");
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
                SoftwareMouse.MouseSprite = Global.Textures["cursor-default"];
            }

            await Task.Run(() =>
            {
                // Load game-specific resources
                ResourceManifest resources = XmlHelper.Load<ResourceManifest>(
                    Path.Combine(Content.RootDirectory, Global.ResourceXml));
                ResourceGroup group = resources.Load("general");
                // Load resources
                Global.Fonts = Global.Fonts.MergeWith(
                    group.FontDictionary as Dictionary<string, SpriteFont>);
                Global.BGM = Global.BGM.MergeWith(
                    group.BGMDictionary as Dictionary<string, Song>);
                Global.SFX = Global.SFX.MergeWith(
                    group.SFXDictionary as Dictionary<string, SoundEffect>);
                Global.Textures = Global.Textures.MergeWith(
                    group.TextureDictionary as Dictionary<string, Texture2D>);
            });

#if DEBUG
            Global.Scenes.Overlays.Add("debug", new DebugOverlay());
#endif
            // Setup first scene (Main Menu)
            Global.Scenes.SwitchToScene(new MainMenuScene());
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
            if ((InputManager.KeyDown(Keys.RightAlt) ||
                InputManager.KeyDown(Keys.LeftAlt)) && InputManager.KeyPressed(Keys.Enter))
            {
                DisplayManager.ToggleFullScreen();
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
