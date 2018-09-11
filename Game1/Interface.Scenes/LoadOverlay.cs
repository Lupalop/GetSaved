using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Components;

namespace Arkabound.Interface.Scenes
{
    public class LoadOverlay : SceneBase
    {
        public LoadOverlay(SceneManager sceneManager, float Interval, string overlayKey)
            : base(sceneManager, "Load Overlay")
        {
            this.Interval = Interval;
            this.overlayKey = overlayKey;
        }
        public LoadOverlay(SceneManager sceneManager, float Interval, string overlayKey, Action afterLoad)
            : base(sceneManager, "Load Overlay")
        {
            this.Interval = Interval;
            this.overlayKey = overlayKey;
            this.afterLoad = afterLoad;
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }

        private float Rotation = 0f;
        private float Opacity = 1f;
        private float Interval = 1f;
        private string overlayKey;
        public Action afterLoad;
        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            Vector2 ScreenCenter = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width / 2, game.GraphicsDevice.Viewport.Bounds.Height / 2);
            for (int i = 0; i < 6; i++)
                spriteBatch.DrawString(fonts["default_m"], "*", ScreenCenter, Color.DarkGoldenrod * Opacity, Rotation + i, new Vector2(0, 0), 2.5f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(fonts["default_m"], "Loading", new Vector2(ScreenCenter.X - 40, ScreenCenter.Y + 50), Color.DarkGoldenrod * Opacity, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        private bool[] isTimerCreated = new bool[4];
        private bool fadeNow;
        public override void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();

            // Spin effect
            if (!isTimerCreated[3])
            {
                for (int i = 0; i < 150 * 2; i++) Timer.Create(i * .05f, () => Rotation += 0.25f);
                isTimerCreated[3] = true;
            }
            // Action when fade effect is done
            if (Opacity <= 0f && !isTimerCreated[2])
            {
                // If afterLoad is defined, invoke action
                if (afterLoad != null) Timer.Create(.4f, () => afterLoad.Invoke());
                // Remove this overlay
                Timer.Create(.5f, () => sceneManager.overlays.Remove(overlayKey));
                isTimerCreated[2] = true;
            }
            // Signal that fade effect should start
            if (!isTimerCreated[0])
            {
                Timer.Create(Interval, () => fadeNow = true);
                isTimerCreated[0] = true;
            }
            // Actual fade effect
            if (!isTimerCreated[1] && fadeNow)
            {
                for (int i = 0; i < 25; i++) Timer.Create(i * .1f, () => Opacity -= 0.1f);
                isTimerCreated[1] = true;
            }

            base.Update(gameTime);
        }
    }
}
