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
    public partial class CreditsScene : Scene
    {
        public CreditsScene() : base("Credits") {}

        public override void LoadContent()
        {
            InitializeComponent();

            ScrollPosition = WindowBounds.Height;

            string[] CreditsText = File.ReadAllLines(Path.Combine(
                Global.Content.RootDirectory, "credits.txt"));

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
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Font = Global.Fonts["o-default_l"]
                    });
                    continue;
                }

                // Medium text (with shadow)
                if (CreditsText[i].StartsWith("-"))
                {
                    ScrollingElements.Add("label_m_s" + i, new Label("label_m_s")
                    {
                        Text = CreditsText[i].Substring(1),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Font = Global.Fonts["o-default_m"]
                    });
                    continue;
                }

                // Large text (no shadow)
                if (CreditsText[i].StartsWith("="))
                {
                    ScrollingElements.Add("label_l" + i, new Label("label_l")
                    {
                        Text = CreditsText[i].Substring(1),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Font = Global.Fonts["default_l"]
                    });
                    continue;
                }

                // Medium text (no shadow)
                if (CreditsText[i].StartsWith("_"))
                {
                    ScrollingElements.Add("label_m" + i, new Label("label_m")
                    {
                        Text = CreditsText[i].Substring(1),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Font = Global.Fonts["default_m"]
                    });
                    continue;
                }

                // Spacers (empty lines)
                if (CreditsText[i].Trim() == "")
                {
                    ScrollingElements.Add("spacer" + i, new Image("spacer" + i)
                    {
                        Size = new Point(10)
                    });
                    continue;
                }

                // Regular text
                ScrollingElements.Add("label" + i, new Label("label")
                {
                    Text = CreditsText[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Font = Global.Fonts["default"]
                });
            }

            base.LoadContent();
        }

        public override void Draw(GameTime GameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(GameTime);
            base.DrawElements(GameTime, Elements);
            base.DrawElements(GameTime, ScrollingElements);
            SpriteBatch.End();
        }

        private float ScrollPosition;
        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            base.UpdateElements(GameTime, Elements);
            base.UpdateElements(GameTime, ScrollingElements);

            ScrollContainer.Location = new Point(
                ScrollContainer.Location.X,
                (int)ScrollPosition);

            if (!Global.Input.MouseDown(MouseButton.Left))
                ScrollPosition -= 1.5f;

            if (ScrollPosition <= -ScrollContainer.ActualSize.Y)
            {
                ScrollPosition = WindowBounds.Height;
            }
        }

        public override void Unload()
        {
            base.Unload();
            DisposeElements(Elements);
            DisposeElements(ScrollingElements);
        }
    }
}
