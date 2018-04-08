using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        }

        private SceneBase _currentState;
        public SceneBase currentScene
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (Program.UseConsole)
                    Console.WriteLine("Switching to scene: " + value.sceneName);
                // Set current state to given scene
                _currentState = value;
            }
        }
        public Game game;
        public SpriteBatch spriteBatch;
        public Dictionary<string, SpriteFont> fonts;

        // List of scenes that are loaded above the current scene
        public List<SceneBase> overlays = new List<SceneBase>();

        public void Draw(GameTime gameTime)
        {
            currentScene.Draw(gameTime);
            // If there are overlays, call their draw method
            if (overlays.Count != 0)
            {
                foreach (SceneBase scb in overlays)
                {
                    scb.Draw(gameTime);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            currentScene.Update(gameTime);
            // If there are overlays, call their update method
            if (overlays.Count != 0)
            {
                foreach (SceneBase scb in overlays)
                {
                    scb.Update(gameTime);
                }
            }
        }
    }
}
