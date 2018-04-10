using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkabound.Interface
{
    public class DebugOverlay : SceneBase
    {
        // FPS+O Counter
        bool isCounterVisible;
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        
        // Scene Manager Info
        string sceneInfoHeader = "\nScene Manager Information";
        string sceneCurrentHeader = "\nCurrent Scene: {0}";
        string sceneOverlayHeader = "\nOverlay Scenes ({0}):\n";
        string sceneOverlayList = "";

        public DebugOverlay(SceneManager sceneManager)
            : base(sceneManager, "Debug Overlay")
        {
        }

        public override void Update(GameTime gameTime)
        {
            // FPS Counter
            if (KeybdState.IsKeyDown(Keys.F2))
                isCounterVisible = true;
            if (KeybdState.IsKeyUp(Keys.F2))
                isCounterVisible = false;

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            // Temporary mouse hiding
            if (KeybdState.IsKeyDown(Keys.F12))
            {
                if (sceneManager.overlays.ContainsKey("mouse"))
                    sceneManager.overlays.Remove("mouse");
            }
            if (KeybdState.IsKeyUp(Keys.F12))
            {
                if (!sceneManager.overlays.ContainsKey("mouse"))
                    sceneManager.overlays.Add("mouse", new MouseOverlay(sceneManager));
            }

            // List overlays currently loaded
            if (KeybdState.IsKeyDown(Keys.F11))
            {
                sceneOverlayList = "";
                if (sceneManager.overlays.Count != 0)
                {
                    for (int i = 0; i < sceneManager.overlays.Count; i++)
                    {
                        List<string> keyList = sceneManager.overlays.Keys.ToList();
                        sceneOverlayList += i+1 + ". " + sceneManager.overlays[keyList[i]].sceneName + " : " + keyList[i] + "\n";
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // FPS Counter
            frameCounter++;

            spriteBatch.Begin();
            if (KeybdState.IsKeyDown(Keys.F11))
            {
                string sceneManagerInfo = sceneInfoHeader + 
                    string.Format(sceneCurrentHeader, sceneManager.currentScene.sceneName) + 
                    string.Format(sceneOverlayHeader, sceneManager.overlays.Count) + 
                    sceneOverlayList;
                spriteBatch.DrawString(fonts["default"], sceneManagerInfo, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(fonts["default"], sceneManagerInfo, new Vector2(1, 1), Color.White);
            }

            if (isCounterVisible)
            {
                string dbCounter = string.Format("FPS: {0}, Memory: {1}, Overlay scenes: {2}", frameRate, GC.GetTotalMemory(false), sceneManager.overlays.Count);
                spriteBatch.DrawString(fonts["default"], dbCounter, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(fonts["default"], dbCounter, new Vector2(1, 1), Color.White);
            }
            spriteBatch.End();
        }
    }
}
