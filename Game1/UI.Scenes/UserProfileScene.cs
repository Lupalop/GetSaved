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
    public class UserProfileScene : SceneBase
    {
        public UserProfileScene(SceneManager SceneManager)
            : base(SceneManager, "User Profile")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "mb1", new MenuButton("mb", SceneManager)
                {
                    Tooltip = "Back",
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},

                { "mb3", new MenuButton("mb", SceneManager)
                {
                    Text = "The Safety Kit",
                    SpriteBatch = this.SpriteBatch,
                }},
            };
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(GameTime);
            base.DrawObjects(GameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateObjects(GameTime, Objects);
        }
    }
}
