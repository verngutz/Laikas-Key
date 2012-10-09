﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    class ChoiceScreen : MiScreen
    {
        public static ChoiceScreen Instance { get; set; }

        private string message;
        public string Message { set { message = value; } }

        private KeyValuePair<string, MiScript>[] choices;
        private int activeChoice;

        private MiAnimatingComponent background;
        private MiAnimatingComponent cursor;

        private bool entryExitMutex;

        public ChoiceScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                choices = new KeyValuePair<string, MiScript>[0];
                activeChoice = 0;
                background = new MiAnimatingComponent(game, 0, 400, MiResolution.VirtualWidth, MiResolution.VirtualHeight - 400, 0, 0, 0, 0);
                cursor = new MiAnimatingComponent(game, 0, 0, 50, 20);
                inputResponses[Controller.A] = new MiScript(Pressed);
                inputResponses[Controller.UP] = new MiScript(Upped);
                inputResponses[Controller.DOWN] = new MiScript(Downed);
                entryExitMutex = false;
            }
            else
            {
                throw new Exception("Message Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            background.AddTexture(Game.Content.Load<Texture2D>("BlackOut"), 0);
            cursor.AddTexture(Game.Content.Load<Texture2D>("Arrow"), 0);
        }

        public override void Update(GameTime gameTime)
        {
            background.Update(gameTime);
            cursor.Color = background.Color;
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(gameTime);
            int y = background.Position.Y + 50;
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), message, new Vector2(0, y), background.Color);
            foreach (KeyValuePair<string, MiScript> choice in choices)
            {
                y += 30;
                Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), choice.Key, new Vector2(75, y), background.Color);
            }
            cursor.Draw(gameTime);
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            if (entryExitMutex)
                yield break;

            entryExitMutex = true;
            background.AlphaOverTime.Keys.Add(new CurveKey(background.AlphaChangeTimer + 30, 255));
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
            background.AlphaOverTime.Keys.Add(new CurveKey(background.AlphaChangeTimer + 30, 0));
            background.AlphaChangeEnabled = true;
            yield return 30;
            background.AlphaChangeEnabled = false;
            Game.PopScreen();
            entryExitMutex = false;
        }

        public IEnumerator<ulong> Upped()
        {
            if (activeChoice > 0)
            {
                activeChoice--;
                cursor.Position = new Point(cursor.Position.X, cursor.Position.Y - 30);
            }
            yield break;
        }

        public IEnumerator<ulong> Downed()
        {
            if (activeChoice < choices.Length - 1)
            {
                activeChoice++;
                cursor.Position = new Point(cursor.Position.X, cursor.Position.Y + 30);
            }
            yield break;
        }

        public IEnumerator<ulong> Pressed()
        {
            Game.ScriptEngine.ExecuteScript(choices[activeChoice].Value);
            yield break;
        }

        public void SetChoices(params KeyValuePair<string, MiScript>[] choices)
        {
            this.choices = choices;
            activeChoice = 0;
            cursor.Position = new Point(0, 485);
        }
    }
}
