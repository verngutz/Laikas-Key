using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    class TownScreen : MiScreen
    {
        public static TownScreen Instance { set; get; }

        private MiAnimatingComponent playerAvatar;
        private MiTileEngine tileEngine;

        private int playerX;
        private int playerY;

        public TownScreen(MiGame game)
            : base(game)
        {
            //
            // Player Avatar
            //
            playerAvatar = new MiAnimatingComponent(game, 375, 275, 50, 50);
            playerX = 1;
            playerY = 1;

            //
            // Town Map
            //
            tileEngine = new MiTileEngine(game, 50, 50);
            tileEngine.AddTileType('g', "Grass", true);
            tileEngine.AddTileType('r', "Road", false);

            inputResponses[Controller.START] = new MiScript(Escape);
            inputResponses[Controller.UP] = new MiScript(MoveUp);
            inputResponses[Controller.DOWN] = new MiScript(MoveDown);
            inputResponses[Controller.LEFT] = new MiScript(MoveLeft);
            inputResponses[Controller.RIGHT] = new MiScript(MoveRight);
            inputResponses[Controller.A] = new MiScript(ExamineFront);
        }

        public override void LoadContent()
        {
            playerAvatar.AddTexture(Game.Content.Load<Texture2D>("tao"), 0);
            tileEngine.LoadContent();
            tileEngine.LoadMap(
                new char[,]
                {
                    {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                    {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                    {'r', 'g', 'g', 'r', 'g', 'g', 'r'},
                    {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                    {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
                },
                playerAvatar.Position.X - playerX * playerAvatar.Width, playerAvatar.Position.Y - playerY * playerAvatar.Height
            );
        }

        public override void Draw(GameTime gameTime)
        {
            tileEngine.Draw(gameTime);
            playerAvatar.Draw(gameTime);
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }

        public IEnumerator<ulong> MoveUp()
        {
            if (tileEngine.MapPassability[playerX, playerY - 1])
            {
            }
            yield break;
        }

        public IEnumerator<ulong> MoveDown()
        {
            if (tileEngine.MapPassability[playerX, playerY + 1])
            {
            }
            yield break;
        }

        public IEnumerator<ulong> MoveLeft()
        {
            if (tileEngine.MapPassability[playerX - 1, playerY])
            {
            }
            yield break;
        }

        public IEnumerator<ulong> MoveRight()
        {
            if (tileEngine.MapPassability[playerX + 1, playerY])
            {
            }
            yield break;
        }

        public IEnumerator<ulong> ExamineFront()
        {
            yield break;
        }
    }
}
