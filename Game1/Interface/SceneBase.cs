﻿using System;
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
        public Dictionary<string, ObjectBase> Objects;
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

        public virtual void DrawObjects(GameTime gameTime, Dictionary<string, ObjectBase> objects)
        {
            DrawObjectsFromArray(gameTime, objects.Values.ToArray<ObjectBase>());
        }
        public virtual void DrawObjects(GameTime gameTime, List<ObjectBase> objects)
        {
            DrawObjectsFromArray(gameTime, objects.ToArray<ObjectBase>());
        }
        public virtual void DrawObjects(GameTime gameTime, ObjectBase[] objects)
        {
            DrawObjectsFromArray(gameTime, objects);
        }
        private void DrawObjectsFromArray(GameTime gameTime, ObjectBase[] objs)
        {
            // Draw objects in the Object array
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].Draw(gameTime);
            }
        }

        public virtual void UpdateObjects(GameTime gameTime, Dictionary<string, ObjectBase> objects)
        {
            UpdateObjectsFromArray(gameTime, objects.Values.ToArray<ObjectBase>());
        }
        public virtual void UpdateObjects(GameTime gameTime, List<ObjectBase> objects)
        {
            UpdateObjectsFromArray(gameTime, objects.ToArray<ObjectBase>());
        }
        public virtual void UpdateObjects(GameTime gameTime, ObjectBase[] objects)
        {
            UpdateObjectsFromArray(gameTime, objects);
        }
        private void UpdateObjectsFromArray(GameTime gameTime, ObjectBase[] objs)
        {
            // Dynamically compute for spacing between *centered* objects
            int distanceFromTop = 50;
            for (int i = 0; i < objs.Length; i++)
            {
                ObjectBase Object = objs[i];

                if (Object.AlignToCenter)
                {
                    if (Object.Graphic != null)
                    {
                        Object.Location = new Vector2(ScreenCenter.X - (Object.Graphic.Width / 2), distanceFromTop);
                        distanceFromTop += Object.Graphic.Height;
                    }
                    else
                    {
                        Object.Location = new Vector2(ScreenCenter.X, distanceFromTop);
                    }
                }

                Object.Update(gameTime);
            }
        }
    }
}
