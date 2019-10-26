using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.Elements;

namespace Maquina.UI.Scenes
{
    public partial class WorldSelectionScene : Scene
    {
        public WorldSelectionScene() : base("Game Selection") {}

        public override void LoadContent()
        {
            InitializeComponent();

            BackgroundGameScene = new GameOneScene(Difficulty.Demo);
            BackgroundGameScene.LoadContent();
            difficulty = Difficulty.Easy;

            base.LoadContent();
        }

        private GameOneScene BackgroundGameScene;
        private Difficulty difficulty;

        private void ModifyDifficulty()
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    difficulty = Difficulty.Medium;
                    break;
                case Difficulty.Medium:
                    difficulty = Difficulty.Hard;
                    break;
                case Difficulty.Hard:
                    difficulty = Difficulty.EpicFail;
                    break;
                case Difficulty.EpicFail:
                    difficulty = Difficulty.Easy;
                    break;
            }
            DifficultyButton.MenuLabel = string.Format("Difficulty: {0}", difficulty);
            LogManager.Info(1, string.Format("Difficulty changed to: {0}", difficulty));
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            //BackgroundGameScene.Draw(gameTime);
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            base.DrawElements(gameTime, Elements);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            //BackgroundGameScene.Update(gameTime);
            base.Update(gameTime);
            base.UpdateElements(gameTime, Elements);
        }

        public override void Unload()
        {
            base.Unload();
            BackgroundGameScene.Unload();
        }
    }
}
