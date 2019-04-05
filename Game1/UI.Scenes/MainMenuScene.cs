using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Maquina.Elements;
using Microsoft.Xna.Framework.Media;

namespace Maquina.UI.Scenes
{
    public class MainMenuScene : SceneBase
    {
        public MainMenuScene() : base("Main Menu") {}

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "logo", new Image("logo") {
                    Graphic = Game.Content.Load<Texture2D>("gameLogo"),
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    Font = Fonts["default_m"]
                }},
                { "mb1", new MenuButton("mb")
                {
                    Tooltip = "Play Game!",
                    Graphic = Game.Content.Load<Texture2D>("playBtn"), 
                    LeftClickAction = () => SceneManager.SwitchToScene(new WorldSelectionScene())
                }},
                { "lb1", new Label("lb")
                {
                    Text = "Prototype Version",
                    Font = Fonts["o-default"]
                }},
                { "container1", new ElementContainer("cr")
                {
                    ElementSpacing = 5,
                    ContainerAlignment = ContainerAlignment.Horizontal,
                    Children = new Dictionary<string, GenericElement> {
                        { "mb2", new MenuButton("mb")
                        {
                            Tooltip = "Credits",
                            Graphic = Game.Content.Load<Texture2D>("circle-btn"),
                            Icon = Game.Content.Load<Texture2D>("credits-btn"),
                            Font = Fonts["default"],
                            LeftClickAction = () => SceneManager.SwitchToScene(new CreditsScene())
                        }},
                        { "mb3", new MenuButton("mb")
                        {
                            Tooltip = "Mute Audio",
                            Graphic = Game.Content.Load<Texture2D>("circle-btn"),
                            Icon = Game.Content.Load<Texture2D>("sound-btn"),
                            Font = Fonts["default"],
                            LeftClickAction = () => Global.AudioManager.ToggleMute()
                        }},
                        { "mb4", new MenuButton("mb")
                        {
                            Tooltip = "View High Scores",
                            Graphic = Game.Content.Load<Texture2D>("circle-btn"),
                            Icon = Game.Content.Load<Texture2D>("highscore-btn"),
                            Font = Fonts["default"],
                            LeftClickAction = () => SceneManager.SwitchToScene(new HighScoreScene())
                        }},
                        { "mb5", new MenuButton("mb")
                        {
                            Tooltip = "Change User",
                            Graphic = Game.Content.Load<Texture2D>("circle-btn"),
                            Icon = Game.Content.Load<Texture2D>("user-btn"),
                            Font = Fonts["default"],
                            LeftClickAction = () => SceneManager.SwitchToScene(new UserProfileScene())
                        }}
                    }
                }}
            };
            Global.AudioManager.PlaySong("flying-high");
            // Layout stuff
            ObjectSpacing = 12;
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }
    }
}
