using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laikas_Key
{
    class BattleScreen : MiScreen
    {
        public static BattleScreen Instance { set; get; }

        private enum BattleState { SETUP, CHARACTER_SELECT, CHARACTER_MOVE, ENEMY_TURN, NOTIF }
        private BattleState state;

        private MiTileEngine tileEngine;
        private List<Character> enemies;
        private Dictionary<Character, Point> positions;

        private MiAnimatingComponent cursor;
        private int cursorX;
        private int cursorY;

        private int setupIndex;

        private const int ALLOWED_INITIAL_REGION = 2;

        public BattleScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            if (Instance == null)
            {
                this.tileEngine = tileEngine;
                inputResponses[Controller.START] = new MiScript(Escape);
                inputResponses[Controller.LEFT] = new MiScript(Lefted);
                inputResponses[Controller.RIGHT] = new MiScript(Righted);
                inputResponses[Controller.UP] = new MiScript(Upped);
                inputResponses[Controller.DOWN] = new MiScript(Downed);
                inputResponses[Controller.A] = new MiScript(Pressed);

                Character grunt1 = new Character("grunt1", 5, 5, 5, 5, 5);
                Character grunt2 = new Character("grunt2", 5, 5, 5, 5, 5);
                Character grunt3 = new Character("grunt3", 5, 5, 5, 5, 5);
                enemies = new List<Character>() { grunt1, grunt2, grunt3 };

                Character you = new Character("you", 5, 5, 5, 5, 5);
                Character someGuy = new Character("someguy", 5, 5, 5, 5, 5);
                Character someOtherGuy = new Character("someotherguy",5, 5, 5, 5, 5);

                Player.Party.Add(you);
                Player.Party.Add(someGuy);
                Player.Party.Add(someOtherGuy);

                state = BattleState.NOTIF;

                positions = new Dictionary<Character, Point>();

                positions[grunt1] = new Point(10, 0);
                positions[grunt2] = new Point(10, 7);
                positions[grunt3] = new Point(11, 3);

                cursor = new MiAnimatingComponent(game, 0, 0, tileEngine.TileWidth, tileEngine.TileHeight);
                cursor.Color = Color.Yellow;
                cursorX = 0;
                cursorY = 0;
                setupIndex = 0;
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
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'r', 'g', 'g', 'r', 'g', 'r', 'r', 'g'},
                    {'g', 'g', 'r', 'r', 'r', 'g', 'g', 'r', 'g', 'g', 'r', 'g'},
                    {'g', 'r', 'r', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'r', 'g'},
                    {'g', 'g', 'r', 'g', 'g', 'g', 'g', 'r', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'r', 'r', 'r', 'g', 'g', 'r', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                },
                0, 0
            );
        }

        public override void LoadContent()
        {
            cursor.AddTexture(Game.Content.Load<Texture2D>("buttonHover"), 0);
        }

        public override void Draw(GameTime gameTime)
        {
            for(int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
            {
                for(int col = 0; col < ALLOWED_INITIAL_REGION; col++)
                {
                    tileEngine.MapGraphics[row, col].Color = state == BattleState.SETUP ? Color.Yellow : Color.White;
                }
            }
            tileEngine.Draw(gameTime);
            foreach (Character c in positions.Keys)
            {
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("taoDown"), tileEngine.BoundingRectangle(positions[c].X, positions[c].Y), Player.Party.Contains(c) ? Color.White : Color.Red);
                Game.SpriteBatch.DrawString(
                    Game.Content.Load<SpriteFont>("Fonts\\Default"), 
                    c.CurrHealth + "/" + c.MaxHealth,
                    new Vector2(tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).X, tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y == 0 ? tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y + 75 : tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y - 25),
                    Color.Red);
                Game.SpriteBatch.DrawString(
                    Game.Content.Load<SpriteFont>("Fonts\\Default"),
                    c.Name,
                    new Vector2(tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).X, tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y == 0 ? tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y + 50 : tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y - 50),
                    Color.Red);
            }
            if (cursor.Visible)
                cursor.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                if (state == BattleState.NOTIF)
                {
                    if (setupIndex < Player.Party.Count)
                    {
                        MessageScreen.Instance.Message = "Select Location for " + Player.Party[setupIndex].Name;
                        Game.PushScreen(MessageScreen.Instance);
                        Game.ScriptEngine.ExecuteScript(new MiScript(MessageScreen.Instance.EntrySequence));
                        state = BattleState.SETUP;
                    }
                }
                
                cursor.Update(gameTime);
            }
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            Enabled = false;
            MessageScreen.Instance.Message = "Setup Phase";
            Game.PushScreen(MessageScreen.Instance);
            IEnumerator<ulong> messageScreenEntry = MessageScreen.Instance.EntrySequence();
            do
            {
                yield return messageScreenEntry.Current;
            }
            while (messageScreenEntry.MoveNext());
            while (Game.InputHandler.Focused == MessageScreen.Instance)
            {
                yield return 5;
            }
            Enabled = true;
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }

        public IEnumerator<ulong> Upped()
        {
            if (cursorY > 0)
            {
                cursorY--;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
            }
            yield break;
        }

        public IEnumerator<ulong> Downed()
        {
            if (cursorY < tileEngine.MapGraphics.GetLength(0) - 1)
            {
                cursorY++;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
            }
            yield break;
        }

        public IEnumerator<ulong> Lefted()
        {
            if (cursorX > 0)
            {
                cursorX--;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
            }
            yield break;
        }

        public IEnumerator<ulong> Righted()
        {
            if (cursorX < tileEngine.MapGraphics.GetLength(1) - 1)
            {
                cursorX++;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
            }
            yield break;
        }

        public IEnumerator<ulong> Pressed()
        {
            if (state == BattleState.SETUP)
            {
                if (cursorX < ALLOWED_INITIAL_REGION && !positions.Values.Contains<Point>(new Point(cursorX, cursorY)) && tileEngine.MapPassability[cursorY, cursorX])
                {
                    positions[Player.Party[setupIndex]] = new Point(cursorX, cursorY);
                    setupIndex++;
                    state = BattleState.NOTIF;
                }
                else
                {
                    Enabled = false;
                    MessageScreen.Instance.Message = "Nigga you can't put that SHIT there.";
                    Game.PushScreen(MessageScreen.Instance);
                    IEnumerator<ulong> messageScreenEntry = MessageScreen.Instance.EntrySequence();
                    do
                    {
                        yield return messageScreenEntry.Current;
                    }
                    while (messageScreenEntry.MoveNext());
                    while (Game.InputHandler.Focused == MessageScreen.Instance)
                    {
                        yield return 5;
                    }
                    Enabled = true;
                }
            }
            yield break;
        }
    }
}
