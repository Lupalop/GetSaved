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
    public class CreditsScene : SceneBase
    {
        public CreditsScene(SceneManager sceneManager)
            : base(sceneManager, "Credits")
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            string CreditsText = "Game Font:\n" + 
                     "  Zilla Slab\n    Copyright 2017, The Mozilla Foundation\n    Licensed under the SIL Open Font License 1.1\n    http://scripts.sil.org/OFL \n\n" +
                     "Key People:\n" +
                     "  Graphic Design and Lead Programmer: Francis Dominic Fajardo\n" +
                     "  Ideas: Shannen Gabrielle Esporlas,  Lara Nicole Meneses,  MJ Moreno\n\n" +
                     "Graphics used in game:\n" +
                     "  Microsoft (Emoji set, potentially non-free)\n  Images from DuckDuckGo Search:\n    https://d2v9y0dukr6mq2.cloudfront.net/video/thumbnail/\n    nWJ1xCb/pov-running-through-high-school-hallway-60fps_h09ooieg__F0000.png\n    https://cdn.wallpapersafari.com/15/28/a05csx.png\n\n" +
                     "Code contribution:\n" +
                     "  ProgressBar code, 2009 Luke Rymarz\n  www.lukerymarz.com\n\n" +
                     "Music and SFX:\n" +
                     "  Music - Purple Planet Music, www.purple-planet.com\n  SFX - Eric Matyas, www.soundimage.org";

            Objects = new Dictionary<string, ObjectBase> {
                { "logo", new Label("logo")
                {
                    Text = "Get Saved:",
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_l"]
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    spriteBatch = this.spriteBatch, 
                    Font = fonts["default_m"]
                }},
                { "lb1", new Label("lb")
                {
                    Text = CreditsText,
                    Location = ScreenCenter,
                    spriteBatch = this.spriteBatch,
                    Font = fonts["default"]
                }},
                { "BackButton", new MenuButton("mb", sceneManager)
                {
                    Graphic = game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    LeftClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
            };
            // Layout stuff
            spacing = 0;
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
