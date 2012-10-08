using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laikas_Key
{
    class MessageScreen : MiScreen
    {
        public static MessageScreen Instance { get; set; }

        private string message;
        public string Message { set { message = value; } }

        private MiAnimatingComponent background;

        public MessageScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                background = new MiAnimatingComponent(game, 0, 400, 800, 200, 0, 0, 0, 0);
                inputResponses[Controller.A] = new MiScript(ExitSequence);
            }
            else
            {
                throw new Exception("Dialog Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            background.AddTexture(Game.Content.Load<Texture2D>("BlackOut"), 0);
        }

        public override void Update(GameTime gameTime)
        {
            background.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(gameTime);
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), message, new Vector2(0, 450), background.Color);
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            background.AlphaOverTime.Keys.Add(new CurveKey(background.AlphaChangeTimer + 30, 255));
            background.AlphaChangeEnabled = true;
            yield return 30;
            background.AlphaChangeEnabled = false;
        }

        public override IEnumerator<ulong> ExitSequence()
        {
            background.AlphaOverTime.Keys.Add(new CurveKey(background.AlphaChangeTimer + 30, 0));
            background.AlphaChangeEnabled = true;
            yield return 30;
            background.AlphaChangeEnabled = false;
            Game.PopScreen();
        }
    }
}
