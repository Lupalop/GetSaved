using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Arkabound.Interface
{
    public class SceneManager
    {
        /// <summary>
        /// Associates the scene manager to a game instance
        /// </summary>
        /// <param name="game">The game instance to attach.</param>
        /// <param name="spriteBatch">The sprite batch present in the game instance</param>
        public SceneManager(Game game, SpriteBatch spriteBatch, Dictionary<string, SpriteFont> fonts)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.fonts = fonts;
            this.overlays = new Dictionary<string, SceneBase>();
        }

        private SceneBase _currentScene;
        public SceneBase currentScene
        {
            get
            {
                return _currentScene;
            }
            set
            {
                if (Program.OutputMessages)
                    Console.WriteLine("Switching to scene: " + value.sceneName);
                // Unload previous scene
                if (_currentScene != null)
                    _currentScene.Unload();
                // Set current state to given scene
                _currentScene = value;
                // Show a fade effect to hide first frame misposition
                string overlayKey = String.Format("fade-{0}", value);
                if (!overlays.ContainsKey(overlayKey))
                    overlays.Add(overlayKey, new Scenes.FadeOverlay(this, overlayKey));
            }
        }

        public Game game;
        public SpriteBatch spriteBatch;
        // List of fonts that are loaded in game
        public Dictionary<string, SpriteFont> fonts;
        // List of scenes that are loaded above the current scene
        public Dictionary<string, SceneBase> overlays;

        public KeyboardState KeybdState;
        public GamePadState GamePdState;
        public MouseState MsState;
        public TouchCollection TouchState;

        public void Draw(GameTime gameTime)
        {
            currentScene.Draw(gameTime);
            // If there are overlays, call their draw method
            for (int i = 0; i < overlays.Count; i++)
            {
                overlays[overlays.Keys.ToList()[i]].Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
            UpdateKeys(currentScene);
            // If there are overlays, call their update method
            if (overlays.Count != 0)
            {
                for (int i = 0; i < overlays.Count; i++)
                {
                    SceneBase scb = overlays[overlays.Keys.ToList()[i]];
                    scb.Update(gameTime);
                    UpdateKeys(scb);
                }
            }
        }

        public void UpdateKeys(SceneBase scb)
        {
            scb.KeybdState = KeybdState;
            scb.GamePdState = GamePdState;
            scb.MsState = MsState;
            scb.TouchState = TouchState;
        }
    }
}
