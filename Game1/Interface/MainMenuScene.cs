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
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            base.sceneName = "Main Menu";
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Draw()
        {
            game.GraphicsDevice.Clear(Color.Red);
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
