using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;

namespace Laikas_Key
{
    class BattleScreen : MiScreen
    {
        public static BattleScreen Instance { set; get; }

        private MiTileEngine tileEngine;

        public BattleScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            this.tileEngine = tileEngine;
            inputResponses[Controller.START] = new MiScript(Escape);
        }

        public void LoadMap()
        {
            tileEngine.LoadMap(
                new char[,]
                {
                    {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                    {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                    {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                    {'r', 'g', 'r', 't', 'r', 'g', 'r'},
                    {'r', 'g', 'r', 'g', 'r', 'g', 'r'},
                    {'r', 'g', 'r', 'g', 'g', 'g', 'r'},
                    {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
                },
                0, 0
            );
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }
    }
}
