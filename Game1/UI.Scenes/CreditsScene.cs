using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI.Controls;
using Maquina.Objects;

namespace Maquina.UI.Scenes
{
    public class CreditsScene : SceneBase
    {
        public CreditsScene(SceneManager SceneManager)
            : base(SceneManager, "Credits")
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
                     "Graphics used in Game:\n" +
                     "  Microsoft (Emoji set, potentially non-free)\n  Images from DuckDuckGo Search:\n    https://d2v9y0dukr6mq2.cloudfront.net/video/thumbnail/\n    nWJ1xCb/pov-running-through-high-school-hallway-60fps_h09ooieg__F0000.png\n    https://cdn.wallpapersafari.com/15/28/a05csx.png\n\n" +
                     "Code contribution:\n" +
                     "  ProgressBar code, 2009 Luke Rymarz\n  www.lukerymarz.com\n\n" +
                     "Music and SFX:\n" +
                     "  Music - Purple Planet Music, www.purple-planet.com\n  SFX - Eric Matyas, www.soundimage.org";

            Objects = new Dictionary<string, GenericElement> {
                { "logo", new Label("logo")
                {
                    Text = "Get Saved:",
                    SpriteBatch = this.SpriteBatch, 
                    Font = Fonts["default_l"]
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    SpriteBatch = this.SpriteBatch, 
                    Font = Fonts["default_m"]
                }},
                { "lb1", new Label("lb")
                {
                    Text = CreditsText,
                    Location = ScreenCenter,
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["default"]
                }},
                { "BackButton", new MenuButton("mb", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},
            };
            // Layout stuff
            ObjectSpacing = 0;
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin();
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
