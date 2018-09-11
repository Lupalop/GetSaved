using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Components;

namespace Arkabound.Interface.Scenes
{
    public class StartupScene : SceneBase
    {
        public StartupScene(SceneManager sceneManager)
            : base(sceneManager, "Startup")
        {
        }

        public override void LoadContent()
        {
            sceneManager.overlays.Add("load", new LoadOverlay(sceneManager, 1f, "load", () => sceneManager.currentScene = new MainMenuScene(sceneManager)));
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
           base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
