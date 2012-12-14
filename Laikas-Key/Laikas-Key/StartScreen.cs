using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    class StartScreen : MiScreen
    {
        public static StartScreen Instance { set; get; }

        private MiAnimatingComponent newGameButtonBase;
        private MiAnimatingComponent quitGameButtonBase;

        private MiButton newGameButton;
        private MiButton quitGameButton;

        private MiAnimatingComponent background;

        private MiAnimatingComponent cursor;

        public StartScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                background = new MiAnimatingComponent(game,0, 0, 1280, 800);
                cursor = new MiAnimatingComponent(game, 683, 328, 33, 35);

                //
                // New Game Button
                //
                newGameButton = new MiButton();
                newGameButton.Pressed +=
                    delegate
                    {
                        WorldScreen.Instance.Activate();
                        return null;
                    };
                newGameButtonBase = new MiAnimatingComponent(game, 733, 278, 502, 107);

                //
                // Quit Game Button
                //
                quitGameButton = new MiButton();
                quitGameButton.Pressed +=
                    delegate
                    {
                        Game.Exit();
                        return null;
                    };
                quitGameButtonBase = new MiAnimatingComponent(game, 750, 418, 488, 127, 0, 0, 0, 0);

                //
                // Default Active Button
                //
                ActiveButton = newGameButton;

                //
                // Reponses to Input
                //
                inputResponses[Controller.UP] = Upped;
                inputResponses[Controller.DOWN] = Downed;
                inputResponses[Controller.A] = Pressed;
            }
            else
            {
                throw new Exception("Start Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            newGameButtonBase.AddTexture(Game.Content.Load<Texture2D>("Main Menu\\playglow"), 0);
            quitGameButtonBase.AddTexture(Game.Content.Load<Texture2D>("Main Menu\\quitglow"), 0);
            background.AddTexture(Game.Content.Load<Texture2D>("Main Menu\\MainMenu_BG"), 0);
            cursor.AddTexture(Game.Content.Load<Texture2D>("Main Menu\\pointer"), 0);
        }

        public override void Update(GameTime gameTime)
        {
            newGameButtonBase.Update(gameTime);
            quitGameButtonBase.Update(gameTime);
            background.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            
            background.Draw(gameTime);
            //newGameButtonBase.Draw(gameTime);
            //quitGameButtonBase.Draw(gameTime);
            cursor.Draw(gameTime);
        }

        private IEnumerator<ulong> Upped()
        {
            if (ActiveButton == quitGameButton)
            {
                ActiveButton = newGameButton;
                cursor.Position = new Point(cursor.Position.X, 328);
                quitGameButtonBase.Color = Color.Transparent;
                newGameButtonBase.Color = Color.White;
            }
            yield break;
        }

        private IEnumerator<ulong> Downed()
        {
            if (ActiveButton == newGameButton)
            {
                ActiveButton = quitGameButton;
                cursor.Position = new Point(cursor.Position.X, 478);
                quitGameButtonBase.Color = Color.White;
                newGameButtonBase.Color = Color.Transparent;
            }
            yield break;
        }

        public IEnumerator<ulong> Pressed()
        {
            ActiveButton.Pressed();
            yield break;
        }
    }
}
