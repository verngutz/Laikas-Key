using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Laikas_Key
{
    class MessageScreen : DialogScreen
    {
        private const int HEIGHT = 600;
        private SoundEffect press;
        public static MessageScreen Instance { get; set; }

        private string message;
        private MiAnimatingComponent background;
        private bool entryExitMutex;

        public MessageScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                background = new MiAnimatingComponent(game, 0, HEIGHT, MiResolution.VirtualWidth, MiResolution.VirtualHeight - HEIGHT, 0, 0, 0, 0);
                inputResponses[Controller.A] = ExitSequence;
                entryExitMutex = false;
            }
            else
            {
                throw new Exception("Dialog Screen Already Initialized");
            }
        }

        public static void Show(string message)
        {
            Instance.message = message;
            Instance.Game.PushScreen(Instance);
            Instance.Game.ScriptEngine.ExecuteScript(Instance.EntrySequence);
            Instance.press.Play();
        }

        public static void Hide()
        {
            Instance.Game.ScriptEngine.ExecuteScript(Instance.ExitSequence);
            Instance.Game.PopScreen();
        }

        public override void LoadContent()
        {
            background.AddTexture(Game.Content.Load<Texture2D>("BlackOut"), 0);
            press = Game.Content.Load<SoundEffect>("Sounds\\button");
        }

        public override void Update(GameTime gameTime)
        {
            background.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(gameTime);
             Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), message, new Vector2(25, background.Position.Y + 50), background.Color);
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            if (entryExitMutex)
                yield break;

            entryExitMutex = true;
            background.SetAlpha(255, 30);
            background.AlphaChangeEnabled = true;
            yield return 30;
            background.AlphaChangeEnabled = false;
            entryExitMutex = false;
        }

        public override IEnumerator<ulong> ExitSequence()
        {
            if (entryExitMutex)
                yield break;

            entryExitMutex = true;
            background.SetAlpha(0, 30);
            background.AlphaChangeEnabled = true;
            yield return 30;
            background.AlphaChangeEnabled = false;
            Game.PopScreen();
            entryExitMutex = false;
        }
    }
}
