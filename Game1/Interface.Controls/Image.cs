using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arkabound.Objects;
using Arkabound.Interface.Scenes;
using Arkabound.Components;

namespace Arkabound.Interface.Controls
{
    public class Image : ObjectBase
    {
        // ObjectBase provides everything needed for an image, except that it is abstract...
        public Image(string ObjectName)
            : base(ObjectName)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
