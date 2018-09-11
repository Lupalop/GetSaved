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
        public SceneBase(SceneManager sceneManager, string sceneName)
        {
            // Scene name assignment
            if (sceneName != null || sceneName.Trim() != "")
                this.sceneName = sceneName;
            // Assign values to important variables
            this.sceneManager = sceneManager;
            game = sceneManager.game;
            spriteBatch = sceneManager.spriteBatch;
            fonts = sceneManager.fonts;
            // Load the scene's content
            LoadContent();
        }

        public SceneManager sceneManager;
        public Game game;
        public SpriteBatch spriteBatch;
        public Dictionary<string, SpriteFont> fonts;
        public string sceneName = "Unnamed Scene";

        public Vector2 ScreenCenter;

        public KeyboardState KeybdState;
        public GamePadState GamePdState;
        public MouseState MsState;

        public virtual void LoadContent()
        {
            if (Program.UseConsole)
                Console.WriteLine("Loading content in: " + sceneName);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Program.UseConsole && Program.VerboseMessages)
                Console.WriteLine("Drawing from scene: " + sceneName);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Program.UseConsole && Program.VerboseMessages)
                Console.WriteLine("Updating from scene: "  + sceneName);
            ScreenCenter = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width / 2, game.GraphicsDevice.Viewport.Bounds.Height / 2);
        }
    }
}
