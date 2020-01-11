using Maquina.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class GameOneScene
    {
        private Image GameBG;
        //private ProgressBar ProgressBar;
        private MenuButton BackButton;
        private Image ObjectCatcher;
        private Label TimerLabel;
        private Canvas UICanvas;
        private Canvas GameCanvas;

        private void InitializeComponent()
        {
            GameBG = new Image("GameBG");
            GameBG.Sprite.Graphic = Global.Textures["game-bg-1"];
            GameBG.IgnoreGlobalScale = true;
            GameBG.Sprite.DestinationRectangle = WindowBounds;

            /*
            // TODO: Restore once we get progress bar reimplemented in platform
            ProgressBar = new ProgressBar("ProgressBar", new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, 32))
            {
                ControlAlignment = Alignment.Fixed,
            };
            ProgressBar.ElementUpdated += (element) =>
            {
                ProgressBar a = (ProgressBar)element;
                a.value = (float)TimeLeft;
            };
            */

            BackButton = new MenuButton("BackButton");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = Global.Textures["back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.LayerDepth = 0.1f;
            BackButton.OnLeftClick += (sender, e) => Global.Scenes.SwitchToScene(new MainMenuScene());

            ObjectCatcher = new Image("ObjectCatcher");
            ObjectCatcher.Sprite.Graphic = Global.Textures["object-catcher"];
            ObjectCatcher.Location = new Point(5, Game.GraphicsDevice.Viewport.Height - 70);
            ObjectCatcher.ElementUpdated += ObjectCatcher_ElementUpdated;

            TimerLabel = new Label("o-timer");
            TimerLabel.AutoPosition = true;
            TimerLabel.VerticalAlignment = VerticalAlignment.Top;
            TimerLabel.HorizontalAlignment = HorizontalAlignment.Right;
            TimerLabel.Sprite.Font = Global.Fonts["o-default_l"];
            TimerLabel.Sprite.LayerDepth = 0.1f;

            TimerLabel.ElementUpdated += (sender, e) =>
            {
                TimerLabel.Sprite.Text = MathHelper.Clamp((int)TimeLeft, 0, 100).ToString();
            };

            UICanvas = new Canvas("mainContainer")
            {
                Children =
                {
                    { GameBG.Name, GameBG },
                    { BackButton.Name, BackButton },
                    //{ ProgressBar.Name, ProgressBar },
                    { TimerLabel.Name, TimerLabel },
                    { ObjectCatcher.Name, ObjectCatcher },
                },
            };

            GameCanvas = new Canvas("gameCanvas");

            Elements.Add(UICanvas.Name, UICanvas);
            Elements.Add(GameCanvas.Name, GameCanvas);

            Global.Display.ResolutionChanged += Display_ResolutionChanged;
        }

        private void ObjectCatcher_ElementUpdated(object sender, EventArgs e)
        {
            if (IsGameEnd)
            {
                Elements.Remove("ObjectCatcher");
            }
            else
            {
                int mouseX = Global.Input.MousePosition.X - (ObjectCatcher.Sprite.Graphic.Width / 2);
                int distance = WindowBounds.Bottom - (ObjectCatcher.ActualSize.Y * 2);
                ObjectCatcher.Location = new Point(mouseX, distance);
            }
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            Rectangle screenRectangle = ((DisplayManager)sender).WindowBounds;
            GameBG.Sprite.DestinationRectangle = screenRectangle;
            GameCanvas.Bounds = screenRectangle;
            UICanvas.Bounds = screenRectangle;
        }
    }
}
