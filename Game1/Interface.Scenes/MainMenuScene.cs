using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface.Controls;
using Arkabound.Components;
using Arkabound.Objects;

namespace Arkabound.Interface.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene(SceneManager sceneManager)
            : base(sceneManager, "Main Menu")
        {
            Objects = new Dictionary<string, ObjectBase> {
                { "logo", new Image("logo") {
                    Graphic = game.Content.Load<Texture2D>("gameLogo"),
                    Location = ScreenCenter,
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
                    Text = "", 
                    Graphic = game.Content.Load<Texture2D>("playBtn"), 
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"],
                    LeftClickAction = () => sceneManager.currentScene = new NextGameScene(sceneManager),
                    RightClickAction = () => sceneManager.currentScene = new WorldSelectionScene(sceneManager)
                }},
                { "lb1", new Label("lb")
                {
                    Text = "Prototype Version (v1)",
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"]
                }}
            };
            // Layout stuff
            spacing = 20;
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
