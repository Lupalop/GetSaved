using Maquina.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.UI.Scenes
{
    public partial class GameThreeScene
    {
        private Image GameBG;
        private Image PlayerElement;
        //private ProgressBar ProgressBar;
        private Button BackButton;
        private Label ScoreCounterLabel;
        private Canvas GameCanvas;
        private Canvas UICanvas;

        private void InitializeComponent()
        {
            /* UI Elements */

            GameBG = new Image("GameBG");
            GameBG.IgnoreApplicationScale = true;
            GameBG.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("game-bg-3"];
            GameBG.Sprite.DestinationRectangle = Application.Display.WindowBounds;

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

            BackButton = new Button("BackButton");
            BackButton.Tooltip.Text = "Back";
            BackButton.Background.Graphic = (TextureSprite)ContentFactory.TryGetResource("back-btn"];
            BackButton.Location = new Point(5, 5);
            BackButton.LayerDepth = 0.1f;
            BackButton.OnLeftClick += (sender, e) => Application.Scenes.SwitchToScene(new MainMenuScene());

            PlayerElement = new Image("Player");
            PlayerElement.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("character"];
            PlayerElement.Sprite.SpriteType = SpriteType.Animated;
            PlayerElement.Sprite.Columns = 3;
            PlayerElement.Sprite.Rows = 1;

            ScoreCounterLabel = new Label("o-timer");
            ScoreCounterLabel.AutoPosition = true;
            ScoreCounterLabel.VerticalAlignment = VerticalAlignment.Top;
            ScoreCounterLabel.HorizontalAlignment = HorizontalAlignment.Right;
            ScoreCounterLabel.Sprite.Font = Application.Fonts["o-default_l"];
            ScoreCounterLabel.Sprite.LayerDepth = 0.1f;

            ScoreCounterLabel.ElementUpdated += (sender, e) =>
            {
                ScoreCounterLabel.Sprite.Text = string.Format("Score: {0}", Score);
            };

            UICanvas = new Canvas("mainContainer")
            {
                Children =
                {
                    { GameBG.Name, GameBG },
                    { BackButton.Name, BackButton },
                    //{ ProgressBar.Name, ProgressBar },
                    { ScoreCounterLabel.Name, ScoreCounterLabel },
                    { PlayerElement.Name, PlayerElement },
                },
            };

            GameCanvas = new Canvas("gameCanvas");

            Entities.Add(UICanvas.Name, UICanvas);
            Entities.Add(GameCanvas.Name, GameCanvas);

            Application.Display.ResolutionChanged += Display_ResolutionChanged;
        }

        private void Display_ResolutionChanged(object sender, EventArgs e)
        {
            Rectangle screenRectangle = ((DisplayManager)sender).WindowBounds;
            GameBG.Sprite.DestinationRectangle = screenRectangle;
            GameCanvas.Bounds = screenRectangle;
            UICanvas.Bounds = screenRectangle;
            UpdateInitialPosition();
        }
    }
}
