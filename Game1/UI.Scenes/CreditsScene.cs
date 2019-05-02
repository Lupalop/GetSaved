using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;
using System.IO;

namespace Maquina.UI.Scenes
{
    public class CreditsScene : Scene
    {
        public CreditsScene() : base("Credits") {}

        private Dictionary<string, GenericElement> ScrollingElements;
        private float ScrollPosition = 0;

        public override void LoadContent()
        {
            base.LoadContent();
            Objects = new Dictionary<string, GenericElement> {
                { "BackButton", new MenuButton("mb")
                {
                    Tooltip = "Back",
                    Graphic = Global.Textures["back-btn"],
                    Location = new Vector2(5, 5),
                    ControlAlignment = ControlAlignment.Fixed,
                    LeftClickAction = () => SceneManager.SwitchToScene(new MainMenuScene())
                }}
            };

            ScrollingElements = new Dictionary<string, GenericElement>();
            string[] CreditsText = File.ReadAllLines(Path.Combine(
                Global.ContentRootDirectory, "credits.txt"));

            // Loop to parse contents of credits file
            for (int i = 0; i < CreditsText.Length; i++)
            {
                // Image
                if (CreditsText[i].StartsWith("~"))
                {
                    // Try to check if graphic is already loaded
                    Texture2D graphic = null;
                    string graphicName = CreditsText[i].Substring(1);
                    if (!Global.Textures.ContainsKey(graphicName))
                    {
                        Global.Textures.Add(graphicName, Game.Content.Load<Texture2D>(graphicName));
                    }
                    graphic = Global.Textures[graphicName];

                    ScrollingElements.Add("image" + i, new Image("image")
                    {
                        Graphic = graphic,
                    });
                    continue;
                }

                // Large text (with shadow)
                if (CreditsText[i].StartsWith("+"))
                {
                    ScrollingElements.Add("label_l_s" + i, new Label("label_l_s")
                    {
                        Text = CreditsText[i].Substring(1),
                        ControlAlignment = ControlAlignment.Fixed,
                        Font = Fonts["o-default_l"]
                    });
                    continue;
                }

                // Medium text (with shadow)
                if (CreditsText[i].StartsWith("-"))
                {
                    ScrollingElements.Add("label_m_s" + i, new Label("label_m_s")
                    {
                        Text = CreditsText[i].Substring(1),
                        ControlAlignment = ControlAlignment.Fixed,
                        Font = Fonts["o-default_m"]
                    });
                    continue;
                }

                // Large text (no shadow)
                if (CreditsText[i].StartsWith("="))
                {
                    ScrollingElements.Add("label_l" + i, new Label("label_l")
                    {
                        Text = CreditsText[i].Substring(1),
                        ControlAlignment = ControlAlignment.Fixed,
                        Font = Fonts["default_l"]
                    });
                    continue;
                }

                // Medium text (no shadow)
                if (CreditsText[i].StartsWith("_"))
                {
                    ScrollingElements.Add("label_m" + i, new Label("label_m")
                    {
                        Text = CreditsText[i].Substring(1),
                        ControlAlignment = ControlAlignment.Fixed,
                        Font = Fonts["default_m"]
                    });
                    continue;
                }

                // Spacers (empty lines)
                if (CreditsText[i].Trim() == "")
                {
                    ScrollingElements.Add("spacer" + i, new Label("spacer")
                    {
                        Text = CreditsText[i],
                        ControlAlignment = ControlAlignment.Fixed,
                        Font = Fonts["default"]
                    });
                    continue;
                }

                // Regular text
                ScrollingElements.Add("label" + i, new Label("label")
                {
                    Text = CreditsText[i],
                    ControlAlignment = ControlAlignment.Fixed,
                    Font = Fonts["default"]
                });
            }
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(GameTime);
            base.DrawObjects(GameTime, Objects);
            base.DrawObjects(GameTime, ScrollingElements);
            SpriteBatch.End();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateObjects(GameTime, Objects);
            base.UpdateObjects(GameTime, ScrollingElements);

            int distanceFromTop = (int)(ScreenCenter.Y - (GetAllObjectsHeight(Objects) / 2));
            int padding = 0;

            foreach (var item in ScrollingElements.Values)
            {
                if (item.Name == "spacer")
                {
                    padding = 10;
                }

                ScrollPosition -= 0.02f;
                distanceFromTop += padding;

                item.Location = new Vector2(ScreenCenter.X - (item.Bounds.Width / 2),
                    distanceFromTop + ScrollPosition);

                distanceFromTop += item.Bounds.Height;
                distanceFromTop += padding;
                padding = 0;
            }

            if (ScrollPosition <= -distanceFromTop)
                ScrollPosition = 400;
        }
    }
}
