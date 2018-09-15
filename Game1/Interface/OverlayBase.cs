using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arkabound.Objects;
using Arkabound.Interface.Controls;

namespace Arkabound.Interface
{
    public abstract class OverlayBase : SceneBase
    {
        public OverlayBase(SceneManager sceneManager, string sceneName, SceneBase parentScene)
            : base(sceneManager, sceneName)
        {
            ParentScene = parentScene;
        }

        public OverlayBase(SceneManager sceneManager, string sceneName)
            : base(sceneManager, sceneName)
        {
        }

        public SceneBase ParentScene { get; set; }

        public virtual void DisableAllMenuButtons(Dictionary<string, ObjectBase> objects)
        {
            DisableAllMenuButtonsFromArray(objects.Values.ToArray<ObjectBase>());
        }
        public virtual void DisableAllMenuButtons(List<ObjectBase> objects)
        {
            DisableAllMenuButtonsFromArray(objects.ToArray<ObjectBase>());
        }
        public virtual void DisableAllMenuButtons(ObjectBase[] objects)
        {
            DisableAllMenuButtonsFromArray(objects);
        }
        private void DisableAllMenuButtonsFromArray(ObjectBase[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is MenuButton)
                {
                    MenuButton mb = (MenuButton)objects[i];
                    mb.Disabled = true;
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (ParentScene != null) DisableAllMenuButtons(ParentScene.Objects);
        }
    }
}
