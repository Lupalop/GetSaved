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
using System.IO;

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
            Objects = new Dictionary<string, GenericElement> {
                { "BackButton", new MenuButton("mb", SceneManager)
                {
                    Graphic = Game.Content.Load<Texture2D>("back-btn"),
                    Location = new Vector2(5,5),
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene(SceneManager))
                }},
                { "logo", new Image("logo") {
                    Graphic = Game.Content.Load<Texture2D>("gameLogo"),
                    SpriteBatch = this.SpriteBatch
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch, 
                    Font = Fonts["default_m"]
                }},
            };

            string[] CreditsText = File.ReadAllLines(Path.Combine(
                Platform.ContentRootDirectory, "credits.txt"));

            for (int i = 0; i < CreditsText.Length; i++)
            {
                // Header
                if (CreditsText[i].StartsWith("+"))
                {
                    Objects.Add("lb_header" + i, new Label("lbh")
                    {
                        Text = CreditsText[i].Substring(1),
                        ControlAlignment = ControlAlignment.Fixed,
                        SpriteBatch = this.SpriteBatch,
                        Font = Fonts["o-default_l"]
                    });
                    continue;
                }
                if (CreditsText[i].StartsWith("-"))
                {
                    Objects.Add("lb_o" + i, new Label("lbo")
                    {
                        Text = CreditsText[i].Substring(1),
                        ControlAlignment = ControlAlignment.Fixed,
                        SpriteBatch = this.SpriteBatch,
                        Font = Fonts["o-default_m"]
                    });
                    continue;
                }
                if (CreditsText[i].Trim() == "")
                {
                    // Regular text
                    Objects.Add("spacer" + i, new Label("spacer")
                    {
                        Text = CreditsText[i],
                        ControlAlignment = ControlAlignment.Fixed,
                        SpriteBatch = this.SpriteBatch,
                        Font = Fonts["default"]
                    });
                }
                // Regular text
                Objects.Add("lb" + i, new Label("lb")
                {
                    Text = CreditsText[i],
                    ControlAlignment = ControlAlignment.Fixed,
                    SpriteBatch = this.SpriteBatch,
                    Font = Fonts["default"]
                });
            }

            // Layout stuff
            ObjectSpacing = 0;
        }
        private float credPosition = 0;
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

            int distanceFromTop = (int)(ScreenCenter.Y - (GetAllObjectsHeight(Objects) / 2));
            int padding = 0;
            foreach (var item in Objects.Values)
            {
                if (item.Name == "mb")
                    continue;
                if (item.Name == "spacer")
                    padding = 10;
                credPosition -= 0.02f;
                distanceFromTop += padding;
                item.Location = new Vector2(ScreenCenter.X - (item.Bounds.Width / 2),
                    distanceFromTop + credPosition);
                distanceFromTop += item.Bounds.Height;
                distanceFromTop += (ObjectSpacing + padding);
                padding = 0;
            }

            if (credPosition <= -distanceFromTop)
                credPosition = 400;
        }
    }
}
