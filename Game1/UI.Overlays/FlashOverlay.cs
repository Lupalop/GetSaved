using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;
using System.Timers;

namespace Maquina.UI.Scenes
{
    public class FlashOverlay : OverlayBase, IDisposable
    {
        public FlashOverlay(SceneManager sceneManager, string overlayKey, Texture2D image, float scale)
            : base(sceneManager, "Fade Overlay")
        {
            this.OverlayKey = overlayKey;
            this.FadeSpeed = 0.1f;
            this.FadeBackground = image;
            this.Scale = scale;
        }

        private float Opacity = 1f;
        private string OverlayKey;
        public Texture2D FadeBackground { get; set; }
        public float FadeSpeed { get; set; }

        private float Scale;

        public override void LoadContent()
        {
            Objects = new Dictionary<string, GenericElement> {
                { "Background", new Image("Background")
                {
                    SpriteBatch = this.SpriteBatch,
                    OnUpdate = () => {
                        Image BG = (Image)Objects["Background"];
                        BG.Graphic = FadeBackground;
                        BG.Tint = Color.White * Opacity;
                    },
                    Scale = this.Scale
                }}
            };
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            Opacity -= FadeSpeed;

            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);

            // Remove overlay when opacity below 0
            if (Opacity <= 0f)
            {
                SceneManager.Overlays.Remove(OverlayKey);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                FadeBackground.Dispose();
            }
        }
    }
}
