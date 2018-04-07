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
//    public enum Scenes { Startup, MainMenu, Options, WorldSelection, LevelEditor, HighScores, PlayerSelection, GameWorld };
    public class SceneManager
    {
        public SceneManager(Game game, SpriteBatch spriteBatch)
        {
            //this.currentScene = currentScene;
            this.game = game;
            this.spriteBatch = spriteBatch;
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
                if (Program.OutputToConsole)
                    Console.WriteLine("Switching to scene: " + value.sceneName);
                // Set current state to given scene
                _currentState = value;
                // Call the scene's load content
                value.LoadContent();
            }
        }
        public Game game;
        public SpriteBatch spriteBatch;

        public void LoadContent()
        {
            currentScene.LoadContent();
        }
        public void Draw()
        {
            currentScene.Draw();
        }
        public void Update()
        {
            currentScene.Update();
        }
    }
}
