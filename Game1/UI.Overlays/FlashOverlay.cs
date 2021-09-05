using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Entities;

namespace Maquina.UI.Scenes
{
    public class FlashOverlay : Overlay, IDisposable
    {
        public FlashOverlay(string overlayKey, Texture2D image,
            float scale, int delay = 0)
            : base("Fade Overlay")
        {
            OverlayKey = overlayKey;
            FadeSpeed = 0.1f;
            FadeBackground = image;
            Scale = scale;
            Delay = delay;
        }

        private int Delay = 0;
        private float Opacity = 1f;
        private string OverlayKey;
        public Texture2D FadeBackground { get; set; }
        public float FadeSpeed { get; set; }

        private float Scale;
        private Timer DelayTimer;
        private bool IsReady = false;

        private Image Background;

        public override void LoadContent()
        {
            Background = new Image("Background")
            {
                Scale = this.Scale
            };
            Background.ElementUpdated += (sender, e) =>
            {
                Background.Sprite.Graphic = FadeBackground;
                Background.Sprite.Tint = Color.White * Opacity;
                Background.Location = new Point(
                    WindowBounds.Center.X - (Background.Bounds.Width / 2),
                    WindowBounds.Center.Y - (Background.Bounds.Height / 2));
            };

            if (Delay > 0)
            {
                DelayTimer = new Timer()
                {
                    AutoReset = false,
                    Enabled = true,
                    Interval = Delay
                };
                DelayTimer.Elapsed += delegate
                {
                    IsReady = true;
                };
            }
            base.LoadContent();
        }

        public override void Draw()
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            SpriteBatch.End();
        }

        public override void Update()
        {
            if (IsReady)
            {
                Opacity -= FadeSpeed;
            }

            GuiUtils.UpdateElements(Elements);

            // Remove overlay when opacity below 0
            if (Opacity <= 0f)
            {
                Application.Scenes.Overlays.Remove(OverlayKey);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FadeBackground.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
