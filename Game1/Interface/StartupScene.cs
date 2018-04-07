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
    public class StartupScene : SceneBase
    {
        public StartupScene(SceneManager sceneManager) : base(sceneManager)
        {
            base.sceneName = "Startup";
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw()
        {
            game.GraphicsDevice.Clear(Color.PaleTurquoise);
            base.Draw();
        }

        public override void Update()
        {
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed)
                sceneManager.currentScene = new MainMenuScene(sceneManager);

            base.Update();
        }
    }
}
