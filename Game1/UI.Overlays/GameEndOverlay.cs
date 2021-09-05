using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.UI;
using Maquina.Entities;
using System.Collections.ObjectModel;

namespace Maquina.UI.Scenes
{
    public partial class GameEndOverlay : Overlay
    {
        public GameEndOverlay(Games currentGame,
            Collection<Entity> passedMessage, Scene parentScene, Difficulty currentDifficulty)
            : base("Game End Overlay", parentScene)
        {
            CurrentGame = currentGame;
            ParentScene = parentScene;
            CurrentDifficulty = currentDifficulty;
            PassedMessage = passedMessage;
            DisableParentSceneGui = true;
        }

        Difficulty CurrentDifficulty { get; set; }
        Collection<Entity> PassedMessage { get; set; }

        public override void LoadContent()
        {
            InitializeComponent();

            switch (CurrentGame)
            {
                case Games.FallingObjects:
                    Game1End(PassedMessage);
                    break;
                case Games.EscapeEarthquake:
                case Games.EscapeFire:
                    Game2End();
                    break;
                case Games.RunningForTheirLives:
                    Game3End();
                    break;
                case Games.HelpOthersNow:
                    Game4End(PassedMessage);
                    break;
            }

            // Show a fade effect in order for this overlay's appearance to be not abrupt
            if (!Application.Scenes.Overlays.ContainsKey("fade-gameEnd"))
                Application.Scenes.Overlays.Add("fade-gameEnd", new FadeOverlay("fade-gameEnd"));
            base.LoadContent();
        }

        Games CurrentGame;
        public override void Draw()
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            GuiUtils.DrawElements(Elements);
            SpriteBatch.End();
        }

        public override void Update()
        {
            GuiUtils.UpdateElements(Elements);
        }

        public void SetGameEndGraphic(GameEndStates endState)
        {
            switch (endState)
            {
                case GameEndStates.TimesUp:
                default:
                    TimesUp.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("game-end-time"];
                    break;
                case GameEndStates.GameOver:
                    TimesUp.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("game-end-lose"];
                    break;
                case GameEndStates.GameWon:
                    TimesUp.Sprite.Graphic = (TextureSprite)ContentFactory.TryGetResource("game-end-win"];
                    break;
            }
        }

        public void Game2End()
        {
            // Cast
            GameTwoScene scene = (GameTwoScene)ParentScene;
            //
            if (scene.IsTimedOut)
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
                SetPointsEarned(0);
                return;
            }
            //
            if (!scene.IsLevelPassed)
            {
                SetGameEndGraphic(GameEndStates.GameOver);
                SetPointsEarned(0);
                return;
            }
            SetGameEndGraphic(GameEndStates.GameWon);
            SetPointsEarned(100 * MathHelper.Clamp((int)scene.TimeLeft, 1, int.MaxValue));
        }

        public void Game3End()
        {
            // Cast
            GameThreeScene scene = (GameThreeScene)ParentScene;
            // Hardcoded to show Game over, no timer, no finish line
            SetGameEndGraphic(GameEndStates.GameOver);
            SetPointsEarned((int)scene.Score);
        }

        public void Game1End(Collection<Entity> CollectedElements)
        {
            int correctItems = 0;
            int incorrectItems = 0;
            int totalItems = 0;
            // Count correct item
            foreach (FallingItem item in CollectedElements)
            {
                if (!item.IsEmergencyItem)
                {
                    incorrectItems++;
                    continue;
                }
                correctItems++;
            }

            totalItems = (correctItems - incorrectItems);

            if (incorrectItems <= 1)
            {
                SetGameEndGraphic(GameEndStates.GameWon);
            }
            else
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
            }

            //
            StackPanel itemContainer = new StackPanel("container-items")
            {
                Orientation = Orientation.Horizontal
            };
            int[] IncorrectItemIDs = new int[100];
            foreach (FallingItem item in CollectedElements)
            {
                if (item.IsEmergencyItem)
                {
                    continue;
                }

                IncorrectItemIDs[item.ItemID] += 1;
            }

            Collection<string> AvailableItems = new Collection<string> {
			    "Medkit", "Can", "Bottle", "Money", "Clothing", "Flashlight", "Whistle", "Car",
			    "Donut", "Shoes", "Jewelry", "Ball", "Wall Clock", "Chair", "Bomb"
			};

            for (int i = 0; i < IncorrectItemIDs.Length; i++)
            {
                if (IncorrectItemIDs[i] == 0)
                {
                    continue;
                }

                Button itemIcon = new Button("icon");
                itemIcon.Background.SpriteType = SpriteType.None;
                itemIcon.Tooltip.Text = AvailableItems[i];
                itemIcon.Background.Graphic = Game.Content.Load<Texture2D>("falling-object/" + AvailableItems[i]);

                Label itemCountLabel = new Label("lb");
                itemCountLabel.Sprite.Text = string.Format("x{0}", IncorrectItemIDs[i]);

                itemContainer.Children.Add("elemContainer" + i, new StackPanel("cr")
                {
                    Orientation = Orientation.Vertical,
                    Children =
                    {
                        { "icon", itemIcon },
                        { "count", itemCountLabel },
                    },
                });
            }

            Label incorrectItemCount = new Label("IncorrectItem");
            incorrectItemCount.Sprite.Text = "Incorrect items: " + incorrectItems;
            incorrectItemCount.Sprite.Font = Application.Fonts["default_m"];

            Label correctItemCount = new Label("CorrectItem");
            correctItemCount.Sprite.Text = "Correct items: " + correctItems;
            correctItemCount.Sprite.Font = Application.Fonts["default_m"];

            InfoContainer.Children.Add("IncorrectItem", incorrectItemCount);
            InfoContainer.Children.Add("CorrectItem", correctItemCount);
            InfoContainer.Children.Add(itemContainer.Name, itemContainer);

            SetPointsEarned(50 * MathHelper.Clamp(totalItems, 0, int.MaxValue));
        }

        public void Game4End(Collection<Entity> CollectedElements)
        {
            int peopleSaved = 0;
            int peopleDied = 0;
            int peopleTotal = 0;
            // Count item
            foreach (Helpman crap in CollectedElements)
            {
                if (!crap.IsAlive)
                {
                    peopleDied++;
                    continue;
                }
                peopleSaved++;
            }

            peopleTotal = (peopleSaved - peopleDied);

            if (peopleDied <= 1)
            {
                SetGameEndGraphic(GameEndStates.GameWon);
            }
            else
            {
                SetGameEndGraphic(GameEndStates.TimesUp);
            }

            Label incorrectItemCount = new Label("IncorrectItem");
            incorrectItemCount.Sprite.Text = "People Died: " + peopleDied;
            incorrectItemCount.Sprite.Font = Application.Fonts["default_m"];

            Label correctItemCount = new Label("CorrectItem");
            correctItemCount.Sprite.Text = "People Saved: " + peopleSaved;
            correctItemCount.Sprite.Font = Application.Fonts["default_m"];

            InfoContainer.Children.Add("container-main", new StackPanel("cr")
            {
                Orientation = Orientation.Horizontal,
                Children = {
                    { "container-list", new StackPanel("cr")
                    {
                        Children =
                        {
                            { "IncorrectCrap", incorrectItemCount },
                            { "CorrectCrap", correctItemCount },
                        }
                    }},
                }
            });

            SetPointsEarned(50 * MathHelper.Clamp(peopleTotal, 0, int.MaxValue));
        }

        public void SetPointsEarned(int points)
        {
            UserApplication.Score += points;

            Label pointsEarnedLabel = new Label("points");
            pointsEarnedLabel.Sprite.Text = String.Format("You earned {0} points!", points);
            pointsEarnedLabel.Sprite.Font = Application.Fonts["o-default_m"];

            Label pointsTotalLabel = new Label("points");
            pointsTotalLabel.Sprite.Text = String.Format("{0}, you have {1} points in total.", UserApplication.UserName, UserApplication.Score);
            pointsTotalLabel.Sprite.Font = Application.Fonts["o-default"];

            InfoContainer.Children.Add("PointsEarned", pointsEarnedLabel);
            InfoContainer.Children.Add("TotalPointsEarned", pointsTotalLabel);
        }
    }
}
