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
        public UserProfileScene() : base("User Profile") {}

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
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene())
                }},
                { "lb1", new Label("lb")
                {
                    Text = String.Format("Are you {0}?", UserGlobal.UserName),
                    Font = Fonts["o-default_l"],
                    SpriteBatch = this.SpriteBatch
                }},
                { "lb2", new Label("lb")
                {
                    Text = String.Format("You currently have {0} points!", UserGlobal.Score),
                    Font = Fonts["default_m"],
                    SpriteBatch = this.SpriteBatch
                }},
                { "lb3", new Label("lb")
                {
                    Text = "If no, type your name at the box\n below and confirm.",
                    Font = Fonts["default_m"],
                    SpriteBatch = this.SpriteBatch
                }},
                { "tb1", new TextBox("tb", SceneManager)
                {
                    SpriteBatch = this.SpriteBatch,
                    OnInput = () => Objects["lb4"].Tint = Color.Transparent,
                }},
                { "mb2", new MenuButton("mb", SceneManager)
                {
                    Tooltip = "Clicking here will clear your points\n and change the active user.",
                    Text = "Confirm and change user",
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () =>
                    {
                        TextBox textbox = (TextBox)Objects["tb1"];
                        // Show the validation warning when textbox is left blank.
                        if (textbox.Text.Trim() == "")
                        {
                            Objects["lb4"].Tint = Color.White;
                            return;
                        }
                        UserGlobal.UserName = textbox.Text;
                        UserGlobal.Score = 0;

                        SceneManager.SwitchToScene(new MainMenuScene());
                    }
                }},
                { "lb4", new Label("lb")
                {
                    Text = "Leaving the name field blank is bad.",
                    Font = Fonts["default"],
                    Tint = Color.Transparent,
                    SpriteBatch = this.SpriteBatch
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
