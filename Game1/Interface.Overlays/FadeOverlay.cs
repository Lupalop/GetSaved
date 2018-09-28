using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using Maquina.Interface.Controls;

namespace Maquina.Interface.Scenes
{
    public class FadeOverlay : OverlayBase
    {
        public FadeOverlay(SceneManager sceneManager, string overlayKey)
            : base(sceneManager, "Fade Overlay")
        {
            this.overlayKey = overlayKey;
            this.fadeColor = Color.Black;
            this.fadeSpeed = 0.1f;
        }
        public FadeOverlay(SceneManager sceneManager, string overlayKey, Color fadeColor)
            : base(sceneManager, "Fade Overlay")
        {
            this.overlayKey = overlayKey;
            this.fadeColor = fadeColor;
            this.fadeSpeed = 0.1f;
        }

        Color fadeColor = Color.Red;
        public float fadeSpeed { get; set; }
        public override void LoadContent()
        {
            Texture2D Dummy = new Texture2D(game.GraphicsDevice, 1, 1);
            Dummy.SetData(new Color[] { fadeColor });
            Objects = new Dictionary<string, Objects.ObjectBase> {
                { "Background", new Image("Background")
                {
                    Graphic = Dummy,
                    DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    AlignToCenter = false,
                    Tint = fadeColor * Opacity,
                    spriteBatch = this.spriteBatch
                }}
            };

            Fader = new Timer(10) { Enabled = true, AutoReset = true };
            Fader.Elapsed += delegate { Opacity -= fadeSpeed; };
            base.LoadContent();
        }

        Timer Fader;
        private float Opacity = 1f;
        private string overlayKey;
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            Image BG = (Image)Objects["Background"];
            BG.DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            
            BG.Tint = fadeColor * Opacity;

            // Action when fade effect is done
            if (Opacity <= 0f)
            {
                // Remove this overlay
                sceneManager.overlays.Remove(overlayKey);
            }

            base.Update(gameTime);
        }
    }
}
