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
    public abstract class SceneBase
    {
        public SceneBase(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            game = sceneManager.game;
            spriteBatch = sceneManager.spriteBatch;
        }

        public SceneManager sceneManager;
        public Game game;
        public SpriteBatch spriteBatch;
        public string sceneName;

        public virtual void LoadContent()
        {
            if (Program.OutputToConsole)
                Console.WriteLine("Loading content in: " + sceneName);
        }

        public virtual void Draw()
        {
            if (Program.OutputToConsole)
                Console.WriteLine("Drawing from scene: " + sceneName);
        }

        public virtual void Update()
        {
            if (Program.OutputToConsole)
                Console.WriteLine("Updating from scene: "  + sceneName);
        }
    }
}
