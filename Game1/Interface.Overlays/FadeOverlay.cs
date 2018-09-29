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
            this.OverlayKey = overlayKey;
            this.FadeColor = Color.Black;
            this.FadeSpeed = 0.1f;
        }

        public FadeOverlay(SceneManager sceneManager, string overlayKey, Color fadeColor)
            : base(sceneManager, "Fade Overlay")
        {
            this.OverlayKey = overlayKey;
            this.FadeColor = fadeColor;
            this.FadeSpeed = 0.1f;
        }

        private Timer Fader;
        private float Opacity = 1f;
        private string OverlayKey;
        private Texture2D FadeBackground;
        private Color _fadeColor;

        public float FadeSpeed { get; set; }
        public Color FadeColor
        {
            get
            {
                return _fadeColor;
            }
            set
            {
                _fadeColor = value;
                Texture2D Dummy = new Texture2D(game.GraphicsDevice, 1, 1);
                Dummy.SetData(new Color[] { value });
                FadeBackground = Dummy;
            }
        }

        public override void LoadContent()
        {
            Objects = new Dictionary<string, Objects.ObjectBase> {
                { "Background", new Image("Background")
                {
                    DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    AlignToCenter = false,
                    Tint = FadeColor * Opacity,
                    spriteBatch = this.spriteBatch,
                    OnUpdate = () => {
                        Image BG = (Image)Objects["Background"];
                        BG.Graphic = FadeBackground;
                        BG.DestinationRectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
                        BG.Tint = FadeColor * Opacity;
                    }
                }}
            };
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);

            // Remove overlay when opacity below 0
            if (Opacity <= 0f)
            {
                sceneManager.overlays.Remove(OverlayKey);
            }
        }

        public override void DelayLoadContent()
        {
            Fader = new Timer(10) { Enabled = true, AutoReset = true };
            Fader.Elapsed += delegate { Opacity -= FadeSpeed; };
            base.DelayLoadContent();
        }

        public override void Unload()
        {
            Fader.Close();
            base.Unload();
        }
    }
}
