using Maquina.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class GameTwoScene : Scene
    {
        private Image GameBG;
        //private ProgressBar ProgressBar;
        private MenuButton BackButton;
        private Image PlayerElement;
        private Label TimerLabel;
        private Label DeathTimerLabel;
        private Label HelpLabel;
        private Image PointA;
        private Image PointB;
        private Canvas UICanvas;
        private Canvas GameCanvas;

        private void InitializeComponent()
        {
            /* UI Elements */

            GameBG = new Image("GameBG");
            GameBG.Sprite.Graphic = Global.Textures["game-bg-4_1"];
            GameBG.Sprite.DestinationRectangle = new Rectangle(0, 0,
                Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            GameBG.ElementUpdated += GameBG_ElementUpdated;

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

            DeathTimerLabel = new Label("timer");
            DeathTimerLabel.Sprite.Tint = Color.Red;
            DeathTimerLabel.ElementUpdated += DeathTimerLabel_ElementUpdated;
            DeathTimerLabel.AutoPosition = true;
            DeathTimerLabel.HorizontalAlignment = HorizontalAlignment.Center;
            DeathTimerLabel.VerticalAlignment = VerticalAlignment.Center;
            DeathTimerLabel.Sprite.Font = Global.Fonts["o-default_xl"];

            HelpLabel = new Label("helplabel");
            HelpLabel.AutoPosition = true;
            HelpLabel.HorizontalAlignment = HorizontalAlignment.Center;
            HelpLabel.VerticalAlignment = VerticalAlignment.Center;
            HelpLabel.Sprite.Font = Global.Fonts["o-default_l"];

            /* Game Elements */

            PlayerElement = new Image("Player");
            PlayerElement.Sprite.Graphic = Global.Textures["character"];
            PlayerElement.Sprite.Columns = 3;
            PlayerElement.Sprite.Rows = 1;
            PlayerElement.Sprite.SpriteType = SpriteType.Animated;
            PlayerElement.Sprite.SpriteEffects = SpriteEffects.FlipHorizontally;

            PointA = new Image("PointA");
            PointA.Sprite.Graphic = Global.Textures["starting-point"];

            PointB = new Image("PointB");
            PointB.Sprite.Graphic = Global.Textures["exit-label"];
            PointB.Size = PointA.Size;

            /* Canvas Elements */

            UICanvas = new Canvas("mainContainer")
            {
                Children =
                {
                    { GameBG.Name, GameBG },
                    { BackButton.Name, BackButton },
                    //{ ProgressBar.Name, ProgressBar },
                    { TimerLabel.Name, TimerLabel },
                    { DeathTimerLabel.Name, DeathTimerLabel },
                    { HelpLabel.Name, HelpLabel },
                },
            };

            GameCanvas = new Canvas("gameCanvas")
            {
                Children =
                {
                    { PlayerElement.Name, PlayerElement },
                    { PointA.Name, PointA },
                    { PointB.Name, PointB },
                },
            };

            Elements.Add(UICanvas.Name, UICanvas);
            Elements.Add(GameCanvas.Name, GameCanvas);

            Global.Display.ResolutionChanged += Display_ResolutionChanged;
        }

        private void UpdatePoints()
        {
            PointA.Location = new Point(WindowBounds.Center.X + 250, WindowBounds.Center.Y + 185);
            PointB.Location = new Point(WindowBounds.Center.X - 300, WindowBounds.Center.Y + 185);
            PlayerElement.Location = PointA.Location;
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            Rectangle screenRectangle = ((DisplayManager)sender).WindowBounds;
            GameBG.Sprite.DestinationRectangle = screenRectangle;
            GameCanvas.Bounds = screenRectangle;
            UICanvas.Bounds = screenRectangle;
            UpdatePoints();
        }

        private void DeathTimerLabel_ElementUpdated(object sender, EventArgs e)
        {
            DeathTimerLabel.Sprite.Text = DeathTimeLeft.ToString();
        }

        private void GameBG_ElementUpdated(object sender, EventArgs e)
        {
            if (CurrentStage != 3 && CurrentGame == Games.EscapeEarthquake)
            {
                if (!ShakeToLeft)
                {
                    ShakeFactor++;
                    if (ShakeFactor == 3)
                        ShakeToLeft = true;
                }
                else
                {
                    ShakeFactor--;
                    if (ShakeFactor == -3)
                        ShakeToLeft = false;
                }
                GameBG.Sprite.DestinationRectangle = new Rectangle(ShakeFactor, 0,
                    Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            }
        }
    }
}
