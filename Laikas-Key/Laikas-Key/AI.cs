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
                    terminate = false;
                    break;
                }
                if (setup)
                {
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
                    foreach (Character c in BattleScreen.Instance.Enemies)
                    {
                        while (c.CurrMovementPoints > 0)
                        {
                            Character min = null;
                            int minDist = 10000;
                            foreach (Character p in Player.Party)
                            {
                                int currDist = Math.Abs(BattleScreen.Instance.Positions[c].X - BattleScreen.Instance.Positions[p].X)
                                    + Math.Abs(BattleScreen.Instance.Positions[c].Y - BattleScreen.Instance.Positions[p].Y);
                                if (currDist < minDist)
                                {
                                    minDist = currDist;
                                    min = p;
                                }
                            }
                            if (min == null) break;

                            bool move = true;
                            foreach (Attack a in c.KnownAttacks)
                            {
                                if(BattleScreen.Instance.SelectAttack(c, a))
                                {
                                    yield return 25;
                                    foreach(Point p in BattleScreen.Instance.SelectedValidMoves.Keys)
                                    {
                                        BattleScreen.Instance.RecalculateAOE(p.X, p.Y, a.AOE);
                                        if (min == null) break;
                                        if(BattleScreen.Instance.SelectedAOE.ContainsKey(BattleScreen.Instance.Positions[min]))
                                        {            
                                            BattleScreen.Instance.Attack(p.X, p.Y);
                                            yield return 25;
                                            move = false;
                                            break;
                                        }
                                    }
                                    BattleScreen.Instance.SelectedValidMoves.Clear();
                                    BattleScreen.Instance.SelectedAOE.Clear();
                                }
                            }

                            if (move)
                            {
                                Point nearest = new Point();
                                int minManhat = 10000;
                                Point curr = BattleScreen.Instance.Positions[c];
                                BattleScreen.Instance.SelectMove(c);
                                yield return 25;
                                foreach (Point p in BattleScreen.Instance.SelectedValidMoves.Keys)
                                {
                                    if (curr.Equals(p))
                                        continue;
                                    int currDist = Math.Abs(BattleScreen.Instance.Positions[min].X - p.X)
                                    + Math.Abs(BattleScreen.Instance.Positions[min].Y - p.Y);
                                    if (currDist < minManhat)
                                    {
                                        minManhat = currDist;
                                        nearest = p;
                                    }
                                }
                                BattleScreen.Instance.Move(nearest.X, nearest.Y);
                                yield return 25;
                            }
                        }
                    }

                    foreach (Character c in BattleScreen.Instance.Enemies)
                    {
                        c.CurrMovementPoints = c.MaxMovementPoints;
                    }
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
