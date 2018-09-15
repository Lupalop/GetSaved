using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkabound.Interface.Scenes
{
    public class DebugOverlay : OverlayBase
    {
        // FPS+O Counter
        bool[] isCounterVisible = new bool[4];
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        
        // Scene Manager Info
        string sceneInfoHeader = "\nScene Manager Information";
        string sceneCurrentHeader = "\nCurrent Scene: {0}";
        string sceneOverlayHeader = "\nOverlay Scenes ({0}):\n";
        string sceneOverlayList = "";
        string sceneObjectHeader = "\nObjects in Current Scene ({0}):\n";
        string sceneObjectList;

        // Mouse Coords
        string mouseCoordinates;

        public DebugOverlay(SceneManager sceneManager)
            : base(sceneManager, "Debug Overlay")
        {
        }

        public override void Update(GameTime gameTime)
        {
            // FPS Counter
            if (KeybdState.IsKeyDown(Keys.F2))
                isCounterVisible[0] = !isCounterVisible[0];
            if (KeybdState.IsKeyDown(Keys.F10))
                isCounterVisible[1] = !isCounterVisible[1];
            if (KeybdState.IsKeyDown(Keys.F11))
                isCounterVisible[2] = !isCounterVisible[2];
            if (KeybdState.IsKeyDown(Keys.F12))
                isCounterVisible[3] = !isCounterVisible[3];


            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            // List mouse coordinates
            if (isCounterVisible[3])
            {
                mouseCoordinates = sceneManager.overlays["mouse"].Objects["Mouse"].Bounds.ToString();
            }

            // List overlays currently loaded
            if (isCounterVisible[2])
            {
                sceneOverlayList = "";
                for (int i = 0; i < sceneManager.overlays.Count; i++)
                {
                    List<string> keyList = sceneManager.overlays.Keys.ToList();
                    sceneOverlayList += String.Format("Key {0}: {2}, Scene Name: {1} \n",
                        new object[] { i, sceneManager.overlays[keyList[i]].sceneName, keyList[i] });
                }
            }
            // List objects loaded
            if (isCounterVisible[1])
            {
                sceneObjectList = "";
                for (int i = 0; i < sceneManager.currentScene.Objects.Count; i++)
                {
                    List<string> keyList = sceneManager.currentScene.Objects.Keys.ToList();
                    sceneObjectList += String.Format("Key {0}: {2}, Object Name: {1}, Location: {3} \n",
                        new object[] { i, sceneManager.currentScene.Objects[keyList[i]].Name, keyList[i], sceneManager.currentScene.Objects[keyList[i]].Location.ToString() });
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // FPS Counter
            frameCounter++;

            spriteBatch.Begin();
            if (isCounterVisible[2])
            {
                string sceneManagerInfo = sceneInfoHeader + 
                    string.Format(sceneCurrentHeader, sceneManager.currentScene.sceneName) + 
                    string.Format(sceneOverlayHeader, sceneManager.overlays.Count) + 
                    sceneOverlayList;
                spriteBatch.DrawString(fonts["default"], sceneManagerInfo, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(fonts["default"], sceneManagerInfo, new Vector2(1, 1), Color.White);
            }
            if (isCounterVisible[1])
            {
                string objectInfo = string.Format(sceneObjectHeader, sceneManager.currentScene.Objects.Count) +
                                    sceneObjectList;
                spriteBatch.DrawString(fonts["default"], objectInfo, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(fonts["default"], objectInfo, new Vector2(1, 1), Color.White);
            }

            if (isCounterVisible[0])
            {
                string dbCounter = string.Format("FPS: {0}, Memory: {1}, Overlay scenes: {2}", frameRate, GC.GetTotalMemory(false), sceneManager.overlays.Count);
                spriteBatch.DrawString(fonts["default"], dbCounter, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(fonts["default"], dbCounter, new Vector2(1, 1), Color.White);
            }
            if (isCounterVisible[3])
            {
                spriteBatch.DrawString(fonts["default"], mouseCoordinates, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(fonts["default"], mouseCoordinates, new Vector2(1, 1), Color.White);
            }
            spriteBatch.End();
        }
    }
}
