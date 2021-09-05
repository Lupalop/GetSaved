using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;
using Microsoft.Xna.Framework.Media;

namespace Maquina.UI.Scenes
{
    public partial class MainMenuScene : Scene
    {
        public MainMenuScene() : base("Main Menu") {}

        public override void LoadContent()
        {
            InitializeComponent();

            Application.Audio.PlaySong("flying-high");

            // Setup background game scene to show colorful stuff
            BackgroundGameScene = new GameOneScene(Difficulty.Demo);
            BackgroundGameScene.LoadContent();

            base.LoadContent();
        }

        private GameOneScene BackgroundGameScene;

        public override void Draw()
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            BackgroundGameScene.Draw();
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            SpriteBatch.End();
        }

        public override void Update()
        {
            BackgroundGameScene.Update();
            GuiUtils.UpdateElements(Elements);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BackgroundGameScene.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
