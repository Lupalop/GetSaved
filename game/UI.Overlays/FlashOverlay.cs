﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;

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

        public override void LoadContent()
        {
            Objects = new Dictionary<string, GenericElement> {
                { "Background", new Image("Background")
                {
                    ControlAlignment = Alignment.Fixed,
                    OnUpdate = (element) => {
                        element.Graphic = FadeBackground;
                        element.Tint = Color.White * Opacity;
                        element.Location = new Vector2(
                            ScreenCenter.X - (element.Bounds.Width / 2),
                            ScreenCenter.Y - (element.Bounds.Height / 2));
                    },
                    Scale = this.Scale
                }}
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

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsReady)
            {
                Opacity -= FadeSpeed;
            }

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
