using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laikas_Key
{
    class DialogScreen : MiScreen
    {
        private string message;
        public DialogScreen(MiGame game, string message)
            : base(game)
        {
            this.message = message;
            inputResponses[Controller.A] = new MiScript(
                delegate
                {
                    Game.PopScreen();
                    return null;
                });
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), message, new Vector2(0, 450), Color.White);
        }
    }
}
