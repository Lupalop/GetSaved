﻿using System;
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
    public class MainMenuScene : Scene
    {
        public MainMenuScene() : base("Main Menu") {}

        public override void LoadContent()
        {
            base.LoadContent();

            Objects = new Dictionary<string, GenericElement> {
                { "logo", new Image("logo") {
                    Graphic = Global.Textures["gameLogo"],
                }},
                { "tagline", new Label("tagline")
                {
                    Text = "Disaster Preparedness for Everyone!",
                    Font = Fonts["default_m"]
                }},
                { "mb1", new MenuButton("mb")
                {
                    Tooltip = "Play Game!",
                    Graphic = Global.Textures["playBtn"],
                    LeftClickAction = () => SceneManager.SwitchToScene(new WorldSelectionScene())
                }},
                { "lb1", new Label("lb")
                {
                    Text = "Prototype Version",
                    Font = Fonts["o-default"]
                }},
                { "container1", new StackPanel("cr")
                {
                    ElementMargin = new Region(0, 5, 0, 0),
                    Orientation = Orientation.Horizontal,
                    Children = {
                        { "mb2", new MenuButton("mb")
                        {
                            Tooltip = "Credits",
                            Graphic = Global.Textures["circle-btn"],
                            Icon = Global.Textures["credits-btn"],
                            Font = Fonts["default"],
                            LeftClickAction = () => SceneManager.SwitchToScene(new CreditsScene())
                        }},
                        { "mb3", new MenuButton("mb")
                        {
                            Tooltip = "Mute Audio",
                            Graphic = Global.Textures["circle-btn"],
                            Icon = Global.Textures["sound-btn"],
                            Font = Fonts["default"],
                            LeftClickAction = () => Global.AudioManager.ToggleMute()
                        }},
                        { "mb4", new MenuButton("mb")
                        {
                            Tooltip = "View High Scores",
                            Graphic = Global.Textures["circle-btn"],
                            Icon = Global.Textures["highscore-btn"],
                            Font = Fonts["default"],
                            LeftClickAction = () => SceneManager.SwitchToScene(new HighScoreScene())
                        }},
                        { "mb5", new MenuButton("mb")
                        {
                            Tooltip = "Change User",
                            Graphic = Global.Textures["circle-btn"],
                            Icon = Global.Textures["user-btn"],
                            Font = Fonts["default"],
                            LeftClickAction = () => SceneManager.SwitchToScene(new UserProfileScene())
                        }}
                    }
                }}
            };
            //Global.AudioManager.PlaySong("flying-high");
            // Layout stuff
            ObjectSpacing = 12;
            BackgroundGameScene = new GameOneScene(Difficulty.Demo);
            BackgroundGameScene.LoadContent();
            BackgroundGameScene.DelayLoadContent();
        }

        private GameOneScene BackgroundGameScene;

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.FromNonPremultiplied(244, 157, 0, 255));
            BackgroundGameScene.Draw(gameTime);
            SpriteBatch.Begin(SpriteSortMode.BackToFront);
            base.Draw(gameTime);
            base.DrawObjects(gameTime, Objects);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            BackgroundGameScene.Update(gameTime);
            base.Update(gameTime);
            base.UpdateObjects(gameTime, Objects);
        }

        public override void Unload()
        {
            base.Unload();
            BackgroundGameScene.Unload();
        }
    }
}
