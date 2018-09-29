using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Interface.Controls;
using Maquina.Objects;

namespace Maquina.Interface.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager sceneManager)
            : base(sceneManager, "Main Menu")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, ObjectBase> {
                { "logo", new Image("logo") {
                    Graphic = game.Content.Load<Texture2D>("gameLogo"),
                    spriteBatch = this.spriteBatch
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"]
                }},
                { "mb1", new MenuButton("mb", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("playBtn"), 
                    spriteBatch = this.spriteBatch, 
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager),
                    RightClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                }},
                { "lb1", new Label("lb")
                {
                    Text = "Prototype Version",
                    spriteBatch = this.spriteBatch,
                    Font = fonts["o-default"]
                }},
                { "mb2", new MenuButton("mb", sceneManager)
                {
                    Text = "Credits",
                    Graphic = null,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default_m"],
                    LeftClickAction = () => sceneManager.currentScene = new CreditsScene(sceneManager)
                }}
            };
            sceneManager.PlayBGM("flying-high");
            // Layout stuff
            spacing = 20;
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
