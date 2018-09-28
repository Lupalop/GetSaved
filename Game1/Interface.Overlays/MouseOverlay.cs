using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Objects;
using Maquina.Interface.Controls;

namespace Maquina.Interface.Scenes
{
    public class MouseOverlay : OverlayBase {
    
        public MouseOverlay(SceneManager sceneManager)
            : base(sceneManager, "Mouse Overlay")
        {
            Objects = new Dictionary<string, ObjectBase> {
                { "Mouse", new Image("mouse")
                {
                    spriteBatch = this.spriteBatch, 
                    Graphic = game.Content.Load<Texture2D>("mouseCur"),
                    SpriteType = SpriteTypes.Static,
                    AlignToCenter = false,
                    Location = MsState.Position.ToVector2(),
                    Rows = 1,
                    Columns = 2,
                }}
            };
        }

        public Rectangle Bounds;

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            Image Mouse = (Image)Objects["Mouse"];
            Mouse.Location = MsState.Position.ToVector2();
            Bounds = Mouse.Bounds;

            Mouse.CurrentFrame = 0;
            // Selected effect - when left button is pressed, cursor turns to green
            if ((MsState.LeftButton == ButtonState.Pressed || MsState.RightButton == ButtonState.Pressed))
            {
                Mouse.CurrentFrame = 1;
            }
            
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
