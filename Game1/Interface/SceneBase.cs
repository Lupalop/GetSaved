using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Objects;

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

        public Vector2 ScreenCenter = new Vector2(-100, -100);

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

        public virtual void DrawObjectsFromBase(GameTime gameTime, ObjectBase[] Objects)
        {
            // Draw objects in the Object array
            foreach (ObjectBase Object in Objects)
            {
                Object.Draw(gameTime);
            }
        }

        public virtual void AlignObjectsToCenterUsingBase(GameTime gameTime, ObjectBase[] Objects)
        {
            // Dynamically compute for spacing between *centered* objects
            int distanceFromTop = 10;
            for (int i = 0; i < Objects.Length; i++)
            {
                ObjectBase Object = Objects[i];
                if (Object.Graphic != null && Object.AlignToCenter)
                {
                    Object.Location = new Vector2(ScreenCenter.X - (Object.Graphic.Width / 2),
                        distanceFromTop + Object.Graphic.Height);
                    distanceFromTop += Object.Graphic.Height;
                }
                else if (Object.AlignToCenter)
                {
                    Object.Location = new Vector2(ScreenCenter.X, distanceFromTop * i);
                }
                Object.Update(gameTime);
            }
        }
    }
}
