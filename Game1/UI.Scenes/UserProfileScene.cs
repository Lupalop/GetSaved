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
    public partial class UserProfileScene : Scene
    {
        public UserProfileScene() : base("User Profile") {}

        public override void LoadContent()
        {
            InitializeComponent();

            base.LoadContent();
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(GameTime, Elements);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            GuiUtils.UpdateElements(GameTime, Elements);
        }
    }
}
