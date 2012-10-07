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

        public TownScreen(MiGame game)
            : base(game)
        {
            //
            // Player Avatar
            //
            playerAvatar = new MiAnimatingComponent(game, 375, 275);

            inputResponses[Controller.START] = new MiScript(Escape);
        }

        public override void LoadContent()
        {
            playerAvatar.AddTexture(Game.Content.Load<Texture2D>("tao"), 0);
        }

        public override void Draw(GameTime gameTime)
        {
            playerAvatar.Draw(gameTime);
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }
    }
}
