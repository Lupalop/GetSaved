using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Interface.Controls;
using Arkabound.Objects;

namespace Arkabound.Interface.Scenes
{
    public class CreditsScene : SceneBase
    {
        public CreditsScene(SceneManager sceneManager)
            : base(sceneManager, "Credits")
        {
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
                    Text = "Back",
                    Graphic = game.Content.Load<Texture2D>("menuBG"),
                    Location = new Vector2(5,5),
                    AlignToCenter = false,
                    spriteBatch = this.spriteBatch,
                    SpriteType = SpriteTypes.Static,
                    Rows = 1,
                    Columns = 3,
                    Font = fonts["default"],
                    LeftClickAction = () => sceneManager.currentScene = new MainMenuScene(sceneManager)
                }},
            };
            // Layout stuff
            spacing = 0;
        }

        string CreditsText = "Game Font:\n  Zilla Slab\n    Copyright 2017, The Mozilla Foundation\n    Licensed under the SIL Open Font License 1.1\n    http://scripts.sil.org/OFL \n\n" +
                             "Graphic Design and Lead Programmer:\n  Francis Dominic Fajardo\n\n" +
                             "Graphics used in game:\n  Microsoft (Emoji set, potentially non-free)\n  Images from DuckDuckGo Search:\n    https://d2v9y0dukr6mq2.cloudfront.net/video/thumbnail/\n    nWJ1xCb/pov-running-through-high-school-hallway-60fps_h09ooieg__F0000.png\n    https://cdn.wallpapersafari.com/15/28/a05csx.png\n\n" +
                             "NG Ideas:\n  Shannen Gabrielle Esporlas\n  Lara Nicole Meneses\n  MJ Moreno";

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
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
