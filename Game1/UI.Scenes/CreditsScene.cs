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
                // Spacers (empty lines)
                if (CreditsText[i].Trim() == "")
                {
                    ScrollingElements.Add("spacer" + i, new Image("spacer" + i)
                    {
                        Size = new Point(10)
                    });
                    continue;
                }

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

                    Image elementImage = new Image("image");
                    elementImage.Sprite.Graphic = graphic;

                    ScrollingElements.Add("image" + i, elementImage);
                    continue;
                }

                Label elementLabel = new Label("label")
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                // Large text (with shadow)
                if (CreditsText[i].StartsWith("+"))
                {
                    elementLabel.Sprite.Text = CreditsText[i].Substring(1);
                    elementLabel.Sprite.Font = Global.Fonts["o-default_l"];
                }
                // Medium text (with shadow)
                else if (CreditsText[i].StartsWith("-"))
                {
                    elementLabel.Sprite.Text = CreditsText[i].Substring(1);
                    elementLabel.Sprite.Font = Global.Fonts["o-default_m"];
                }
                // Large text (no shadow)
                else if (CreditsText[i].StartsWith("="))
                {
                    elementLabel.Sprite.Text = CreditsText[i].Substring(1);
                    elementLabel.Sprite.Font = Global.Fonts["default_l"];
                }
                // Medium text (no shadow)
                else if (CreditsText[i].StartsWith("_"))
                {
                    elementLabel.Sprite.Text = CreditsText[i].Substring(1);
                    elementLabel.Sprite.Font = Global.Fonts["default_m"];
                }
                // Regular text
                else
                {
                    elementLabel.Sprite.Text = CreditsText[i];
                    elementLabel.Sprite.Font = Global.Fonts["default"];
                }

                ScrollingElements.Add("label" + i, elementLabel);
            }

            base.LoadContent();
        }

        public override void Draw()
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            GuiUtils.DrawElements(ScrollingElements);
            SpriteBatch.End();
        }

        private float ScrollPosition;
        public override void Update()
        {
            GuiUtils.UpdateElements(Elements);
            GuiUtils.UpdateElements(ScrollingElements);

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
    }
}
