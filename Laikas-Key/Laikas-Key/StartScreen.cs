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
        private MiScreen gameScreen;

        private MiAnimatingComponent newGameButtonBase;
        private MiAnimatingComponent quitGameButtonBase;
        private MiAnimatingComponent cursor;

        private MiButton newGameButton;
        private MiButton quitGameButton;

        public StartScreen(MiGame game)
            : base(game)
        {
            //
            // Game Screen
            //
            gameScreen = new WorldScreen(game);

            //
            // New Game Button
            //
            newGameButton = new MiButton();
            newGameButton.Pressed += new MiScript(
                delegate
                {
                    Game.ToUpdate.Pop();
                    Game.ToDraw.RemoveLast();
                    Game.ToUpdate.Push(gameScreen);
                    Game.ToDraw.AddLast(gameScreen);
                    return null;
                });
            newGameButtonBase = new MiAnimatingComponent(game, 100, 300);

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
            quitGameButtonBase = new MiAnimatingComponent(game, 100, 400);

            //
            // Cursor
            //
            cursor = new MiAnimatingComponent(game, 100, 300);

            ActiveButton = newGameButton;
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

        public override IEnumerator<ulong> Upped()
        {
            if (ActiveButton == quitGameButton)
            {
                cursor.Position = new Vector2(100, 300);
                ActiveButton = newGameButton;
            }
            yield break;
        }

        public override IEnumerator<ulong> Downed()
        {
            if (ActiveButton == newGameButton)
            {
                cursor.Position = new Vector2(100, 400);
                ActiveButton = quitGameButton;
            }
            yield break;
        }

        public override IEnumerator<ulong> Pressed()
        {
            ActiveButton.Pressed();
            yield break;
        }
    }
}
