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
        public static DialogScreen Instance { get; set; }

        private string message;
        public string Message { set { message = value; } }

        private Texture2D background;
        private Rectangle boundingRectangle;

        public DialogScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                boundingRectangle = new Rectangle(0, 400, 800, 200);
                inputResponses[Controller.A] = new MiScript(
                    delegate
                    {
                        Game.PopScreen();
                        return null;
                    });
            }
            else
            {
                throw new Exception("Dialog Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            background = Game.Content.Load<Texture2D>("BlackOut");
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Draw(background, boundingRectangle, Color.White);
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), message, new Vector2(0, 450), Color.White);
        }
    }
}
