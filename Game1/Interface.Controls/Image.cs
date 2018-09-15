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
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
