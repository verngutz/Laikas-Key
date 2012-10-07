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

        public BattleScreen(MiGame game)
            : base(game)
        {
            inputResponses[Controller.START] = new MiScript(Escape);
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }
    }
}
