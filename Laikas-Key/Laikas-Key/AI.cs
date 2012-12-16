using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    static class AI
    {
        private static Random random;
        private static Laikas game;
        public static void Init(Laikas game)
        {
            AI.game = game;
            random = new Random();
        }

        public static void Terminate()
        {
            terminate = true;
        }

        private static bool setup = true;
        private static bool terminate = false;
        public static IEnumerator<ulong> Stupid()
        {
            while (true)
            {
                if (terminate)
                {
                    System.Console.WriteLine("Terminated");
                    terminate = false;
                    break;
                }
                if (setup)
                {
                    System.Console.WriteLine("Setup");
                    foreach (Character c in BattleScreen.Instance.Enemies)
                    {
                        Point pos;
                        do
                        {
                            pos.X = (int)(random.Next(game.TileEngine.MapGraphics.GetLength(1) - 1 - BattleScreen.ALLOWED_INITIAL_REGION, game.TileEngine.MapGraphics.GetLength(1) - 1));
                            pos.Y = (int)(random.Next(0, game.TileEngine.MapGraphics.GetLength(0) - 1));
                        }
                        while(!game.TileEngine.MapPassability[pos.Y, pos.X] || BattleScreen.Instance.Positions.ContainsValue(pos));
                        BattleScreen.Instance.Positions[c] = pos;
                    }
                    setup = false;
                }
                else if(BattleScreen.Instance.State == BattleScreen.BattleState.ENEMY_TURN)
                {
                    BattleScreen.Instance.State = BattleScreen.BattleState.NOTIF;
                }
                yield return 5;
            }
            System.Console.WriteLine("Setup back to true");
            setup = true;
            yield break;
        }
    }
}
