using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;
using Microsoft.Xna.Framework.Media;

namespace Maquina.UI.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager SceneManager)
            : base(SceneManager, "Main Menu")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "logo", new Image("logo") {
                    Graphic = Game.Content.Load<Texture2D>("gameLogo"),
                    SpriteBatch = this.SpriteBatch
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    SpriteBatch = this.SpriteBatch, 
                    Font = Fonts["default_m"]
                }},
                { "mb1", new MenuButton("mb", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("playBtn"), 
                    SpriteBatch = this.SpriteBatch, 
                    LeftClickAction = () => SceneManager.SwitchToScene(new NextGameScene(SceneManager)),
                    RightClickAction = () => SceneManager.SwitchToScene(new WorldSelectionScene(SceneManager))
                }},
                { "lb1", new Label("lb")
                {
                    Text = "Prototype Version",
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["o-default"]
                }},
                { "mb2", new MenuButton("mb", SceneManager)
                {
                    Text = "Credits",
                    Graphic = null,
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["default"],
                    LeftClickAction = () => SceneManager.SwitchToScene(new CreditsScene(SceneManager))
                }},
                { "mb3", new MenuButton("mb", SceneManager)
                {
                    Text = "Mute Audio",
                    Graphic = null,
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["default"],
                    LeftClickAction = () =>
                    {
                        SceneManager.AudioManager.ToggleMute();
                    }
                }}
            };
            SceneManager.AudioManager.PlaySong("flying-high");
            // Layout stuff
            ObjectSpacing = 12;
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
