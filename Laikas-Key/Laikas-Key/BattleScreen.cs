using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    class BattleScreen : MiScreen
    {
        public static BattleScreen Instance { set; get; }

        private MiTileEngine tileEngine;

        public BattleScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            if (Instance == null)
            {
                this.tileEngine = tileEngine;
                inputResponses[Controller.START] = new MiScript(Escape);
            }
            else
            {
                throw new Exception("Battle Screen Already Initialized");
            }
        }

        public void LoadMap()
        {
            tileEngine.LoadMap(
                new char[,]
                {
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'r', 'g', 'g', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'r', 'g', 'g', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'r', 'r', 'r', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'r', 'r', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'r', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'r', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'r', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'r', 'r', 'g', 'g'},
                    {'g', 'g', 'r', 'r', 'r', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                },
                0, 0
            );
        }

        public override void Draw(GameTime gameTime)
        {
            tileEngine.Draw(gameTime);
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            DialogScreen.Instance.Message = "Setup Phase";
            Game.PushScreen(DialogScreen.Instance);
            return DialogScreen.Instance.EntrySequence();
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }
    }
}
