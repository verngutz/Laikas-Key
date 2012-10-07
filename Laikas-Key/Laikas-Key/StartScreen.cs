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
        private MiAnimatingComponent cursor;

        private MiButton newGameButton;
        private MiButton quitGameButton;

        public StartScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                //
                // New Game Button
                //
                newGameButton = new MiButton();
                newGameButton.Pressed += new MiScript(
                    delegate
                    {
                        Game.RemoveAllScreens();
                        Game.PushScreen(WorldScreen.Instance);
                        return null;
                    });
                newGameButtonBase = new MiAnimatingComponent(game, 100, 300, 100, 75);

                //
                // Quit Game Button
                //
                quitGameButton = new MiButton();
                quitGameButton.Pressed += new MiScript(
                    delegate
                    {
                        Game.Exit();
                        return null;
                    });
                quitGameButtonBase = new MiAnimatingComponent(game, 100, 400, 100, 75);

                //
                // Cursor
                //
                cursor = new MiAnimatingComponent(game, 100, 300, 100, 75);

                //
                // Default Active Button
                //
                ActiveButton = newGameButton;

                //
                // Reponses to Input
                //
                inputResponses[Controller.UP] = new MiScript(Upped);
                inputResponses[Controller.DOWN] = new MiScript(Downed);
                inputResponses[Controller.A] = new MiScript(Pressed);
            }
            else
            {
                throw new Exception("Start Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            newGameButtonBase.AddTexture(Game.Content.Load<Texture2D>("button"), 0);
            quitGameButtonBase.AddTexture(Game.Content.Load<Texture2D>("button"), 0);
            cursor.AddTexture(Game.Content.Load<Texture2D>("buttonHover"), 0);
        }

        public override void Update(GameTime gameTime)
        {
            newGameButtonBase.Update(gameTime);
            quitGameButtonBase.Update(gameTime);
            cursor.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            newGameButtonBase.Draw(gameTime);
            quitGameButtonBase.Draw(gameTime);
            cursor.Draw(gameTime);
        }

        private IEnumerator<ulong> Upped()
        {
            if (ActiveButton == quitGameButton)
            {
                cursor.Position = newGameButtonBase.Position;
                ActiveButton = newGameButton;
            }
            yield break;
        }

        private IEnumerator<ulong> Downed()
        {
            if (ActiveButton == newGameButton)
            {
                cursor.Position = quitGameButtonBase.Position;
                ActiveButton = quitGameButton;
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
