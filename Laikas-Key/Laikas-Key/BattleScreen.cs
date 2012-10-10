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

        public enum BattleState { SETUP, CHARACTER_SELECT, CHARACTER_MOVE, CHARACTER_ATTACK, ENEMY_TURN, NOTIF }
        private BattleState state;
        public BattleState State { get { return state; } }

        private MiTileEngine tileEngine;
        private List<Character> enemies;
        private Dictionary<Character, Point> positions;

        private MiAnimatingComponent cursor;
        private int cursorX;
        private int cursorY;

        private int setupIndex;
        public int SetupIndex { get { return setupIndex; } }

        bool colorChanged = false;

        private const int ALLOWED_INITIAL_REGION = 2;
        private Character selectedCharacter;
        private int selectedCharacterX;
        private int selectedCharacterY;
        private Dictionary<Point, int> selectedValidMoves;
        private Dictionary<Point, int> selectedAOE;
        private Attack selectedAttack;
        private int selectedCharacterMovePtsUsed;

        private Random random;

        public BattleScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            if (Instance == null)
            {
                this.tileEngine = tileEngine;
                inputResponses[Controller.START] = new MiScript(Escaped);
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
                you.KnownAttacks.Add(Attack.shootGun);
                you.KnownAttacks.Add(Attack.swingSword);

                Character someGuy = new Character("someguy", 5, 5, 5, 5, 5);
                someGuy.KnownAttacks.Add(Attack.shootGun);

                Character someOtherGuy = new Character("someotherguy",5, 5, 5, 5, 5);
                someOtherGuy.KnownAttacks.Add(Attack.swingSword);

                Player.Party.Add(you);
                Player.Party.Add(someGuy);
                Player.Party.Add(someOtherGuy);

                state = BattleState.NOTIF;

                positions = new Dictionary<Character, Point>();
                positions[grunt1] = new Point(10, 0);
                positions[grunt2] = new Point(10, 6);
                positions[grunt3] = new Point(11, 3);

                cursor = new MiAnimatingComponent(game, 0, 50, tileEngine.TileWidth, tileEngine.TileHeight);
                cursor.Color = Color.Yellow;
                cursorX = 0;
                cursorY = 0;
                setupIndex = 0;

                selectedValidMoves = new Dictionary<Point, int>();
                selectedAOE = new Dictionary<Point, int>();

                random = new Random();
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
                    {'g', 'g', 'r', 'r', 'r', 'g', 'g', 'r', 'g', 'g', 'g', 'g'},
                    {'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g'},
                },
                0, 50
            );
        }

        public override void LoadContent()
        {
            cursor.AddTexture(Game.Content.Load<Texture2D>("buttonHover"), 0);
        }

        public override void Draw(GameTime gameTime)
        {
            tileEngine.Draw(gameTime);
            foreach (Character c in positions.Keys)
            {
                Rectangle cBounds = tileEngine.BoundingRectangle(positions[c].X, positions[c].Y);
                if (state == BattleState.CHARACTER_ATTACK && selectedAOE.ContainsKey(positions[c]))
                    Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("taoDown"), cBounds, Player.Party.Contains(c) ? Color.Red : Color.DarkRed);
                else if(state == BattleState.CHARACTER_ATTACK && selectedValidMoves.ContainsKey(positions[c]))
                    Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("taoDown"), cBounds, Player.Party.Contains(c) ? Color.Yellow : Color.DarkGoldenrod);
                else
                    Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("taoDown"), cBounds, Player.Party.Contains(c) ? Color.White : Color.Gray);
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("button"), new Rectangle(cBounds.X, cBounds.Bottom - 20, cBounds.Width, 20), Color.Brown);
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("button"), new Rectangle(cBounds.X, cBounds.Bottom - 20, c.CurrHealth * cBounds.Width / c.MaxHealth, 20), Color.Green);
                Game.SpriteBatch.DrawString(
                    Game.Content.Load<SpriteFont>("Fonts\\Default"),
                    c.Name,
                    new Vector2(tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).X, tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y),
                    Color.Green);
            }
            switch (state)
            {
                case BattleState.CHARACTER_ATTACK:
                case BattleState.CHARACTER_MOVE:
                case BattleState.CHARACTER_SELECT:
                case BattleState.SETUP:
                    cursor.Draw(gameTime);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.InputHandler.Focused == MessageScreen.Instance || Game.InputHandler.Focused == ChoiceScreen.Instance)
            {
                return;
            }

            if (Player.Party.Count == 0)
            {
                ChoiceScreen.Instance.Message = "You FUCKING died, BITCH!";
                ChoiceScreen.Instance.SetChoices(new KeyValuePair<string, MiScript>("Yeah Whatever...", new MiScript(Escaped)));
                Game.PushScreen(ChoiceScreen.Instance);
                Game.ScriptEngine.ExecuteScript(new MiScript(ChoiceScreen.Instance.EntrySequence));
                return;
            }

            if (enemies.Count == 0)
            {
                ChoiceScreen.Instance.Message = "You FUCKING won, BITCH!";
                ChoiceScreen.Instance.SetChoices(new KeyValuePair<string, MiScript>("Yeah Whatever...", new MiScript(Escaped)));
                Game.PushScreen(ChoiceScreen.Instance);
                Game.ScriptEngine.ExecuteScript(new MiScript(ChoiceScreen.Instance.EntrySequence));
                return;
            }

            switch (state)
            {
                case BattleState.NOTIF:
                    if (setupIndex < Player.Party.Count)
                    {
                        MessageScreen.Instance.Message = "Select Location for " + Player.Party[setupIndex].Name;
                        Game.PushScreen(MessageScreen.Instance);
                        Game.ScriptEngine.ExecuteScript(new MiScript(MessageScreen.Instance.EntrySequence));
                        for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                        {
                            for (int col = 0; col < ALLOWED_INITIAL_REGION; col++)
                            {
                                tileEngine.MapGraphics[row, col].Color = Color.Yellow;
                            }
                        }
                        colorChanged = true;
                        state = BattleState.SETUP;
                    }
                    else
                    {
                        if (colorChanged)
                        {
                            for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                            {
                                for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                                {
                                    tileEngine.MapGraphics[row, col].Color = Color.White;
                                }
                            }
                            colorChanged = false;
                        }
                        ChoiceScreen.Instance.Message = "What to do?";
                        ChoiceScreen.Instance.SetChoices(
                            new KeyValuePair<string, MiScript>("Fight",
                                delegate
                                {
                                    state = BattleState.CHARACTER_SELECT;
                                    return ChoiceScreen.Instance.ExitSequence();
                                }),
                            new KeyValuePair<string, MiScript>("End Turn",
                                delegate
                                {
                                    state = BattleState.ENEMY_TURN;
                                    Game.ScriptEngine.ExecuteScript(new MiScript(EnemyTurn));
                                    return ChoiceScreen.Instance.ExitSequence();
                                }),
                            new KeyValuePair<string, MiScript>("Run", new MiScript(ExitSequence)));
                        Game.PushScreen(ChoiceScreen.Instance);
                        Game.ScriptEngine.ExecuteScript(ChoiceScreen.Instance.EntrySequence);
                    }
                    break;
            }
                
            cursor.Update(gameTime);
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            MessageScreen.Instance.Message = "Setup Phase";
            Game.PushScreen(MessageScreen.Instance);
            Game.ScriptEngine.ExecuteScript(MessageScreen.Instance.EntrySequence);
            while (Game.InputHandler.Focused == MessageScreen.Instance)
            {
                yield return 5;
            }
        }

        public override IEnumerator<ulong> ExitSequence()
        {
            foreach (Character c in Player.Party)
            {
                positions.Remove(c);
            }
            setupIndex = 0;
            state = BattleState.NOTIF;
            Game.RemoveAllScreens();
            Game.PushScreen(WorldScreen.Instance);
            yield break;
        }

        public IEnumerator<ulong> Upped()
        {
            if (cursorY > 0)
            {
                cursorY--;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
                if (state == BattleState.CHARACTER_ATTACK)
                {
                    for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                    {
                        for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                        {
                            tileEngine.MapGraphics[row, col].Color = Color.White;
                        }
                    }
                    selectedValidMoves.Clear();
                    selectedAOE.Clear();
                    MapFloodFill(selectedCharacterX, selectedCharacterY, selectedAttack.Range, true, Color.Yellow, selectedValidMoves);
                    if (selectedValidMoves.ContainsKey(new Point(cursorX, cursorY)))
                        MapFloodFill(cursorX, cursorY, selectedAttack.AOE, true, Color.Salmon, selectedAOE);
                }
            }
            yield break;
        }

        public IEnumerator<ulong> Downed()
        {
            if (cursorY < tileEngine.MapGraphics.GetLength(0) - 1)
            {
                cursorY++;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
                if (state == BattleState.CHARACTER_ATTACK)
                {
                    for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                    {
                        for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                        {
                            tileEngine.MapGraphics[row, col].Color = Color.White;
                        }
                    }
                    selectedValidMoves.Clear();
                    selectedAOE.Clear();
                    MapFloodFill(selectedCharacterX, selectedCharacterY, selectedAttack.Range, true, Color.Yellow, selectedValidMoves);
                    if (selectedValidMoves.ContainsKey(new Point(cursorX, cursorY)))
                        MapFloodFill(cursorX, cursorY, selectedAttack.AOE, true, Color.Salmon, selectedAOE);
                }
            }
            yield break;
        }

        public IEnumerator<ulong> Lefted()
        {
            if (cursorX > 0)
            {
                cursorX--;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
                if (state == BattleState.CHARACTER_ATTACK)
                {
                    for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                    {
                        for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                        {
                            tileEngine.MapGraphics[row, col].Color = Color.White;
                        }
                    }
                    selectedValidMoves.Clear();
                    selectedAOE.Clear();
                    MapFloodFill(selectedCharacterX, selectedCharacterY, selectedAttack.Range, true, Color.Yellow, selectedValidMoves);
                    if (selectedValidMoves.ContainsKey(new Point(cursorX, cursorY)))
                        MapFloodFill(cursorX, cursorY, selectedAttack.AOE, true, Color.Salmon, selectedAOE);
                }
            }
            yield break;
        }

        public IEnumerator<ulong> Righted()
        {
            if (cursorX < tileEngine.MapGraphics.GetLength(1) - 1)
            {
                cursorX++;
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
                if (state == BattleState.CHARACTER_ATTACK)
                {
                    for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                    {
                        for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                        {
                            tileEngine.MapGraphics[row, col].Color = Color.White;
                        }
                    }
                    selectedValidMoves.Clear();
                    selectedAOE.Clear();
                    MapFloodFill(selectedCharacterX, selectedCharacterY, selectedAttack.Range, true, Color.Yellow, selectedValidMoves);
                    if (selectedValidMoves.ContainsKey(new Point(cursorX, cursorY)))
                        MapFloodFill(cursorX, cursorY, selectedAttack.AOE, true, Color.Salmon, selectedAOE);
                }
            }
            yield break;
        }

        public IEnumerator<ulong> Escaped()
        {
            switch (state)
            {
                case BattleState.CHARACTER_SELECT:
                    state = BattleState.NOTIF;
                    return null;
                case BattleState.CHARACTER_MOVE:
                    positions[selectedCharacter] = new Point(selectedCharacterX, selectedCharacterY);
                    for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                    {
                        for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                        {
                            tileEngine.MapGraphics[row, col].Color = Color.White;
                        }
                    }
                    selectedValidMoves.Clear();
                    colorChanged = false;
                    state = BattleState.CHARACTER_SELECT;
                    return null;
                case BattleState.CHARACTER_ATTACK:
                    for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                    {
                        for (int col = 0; col < tileEngine.MapGraphics.GetLength(1); col++)
                        {
                            tileEngine.MapGraphics[row, col].Color = Color.White;
                        }
                    }
                    selectedValidMoves.Clear();
                    selectedAOE.Clear();
                    colorChanged = false;
                    state = BattleState.CHARACTER_SELECT;
                    return null;
                default:
                    return ExitSequence();
            }
        }

        public IEnumerator<ulong> Pressed()
        {
            switch (state)
            {
                case BattleState.SETUP:
                    if (cursorX < ALLOWED_INITIAL_REGION && !positions.Values.Contains<Point>(new Point(cursorX, cursorY)) && tileEngine.MapPassability[cursorY, cursorX])
                    {
                        positions[Player.Party[setupIndex]] = new Point(cursorX, cursorY);
                        setupIndex++;
                        state = BattleState.NOTIF;
                    }
                    else
                    {
                        MessageScreen.Instance.Message = "Nigga you can't put that SHIT there.";
                        Game.PushScreen(MessageScreen.Instance);
                        Game.ScriptEngine.ExecuteScript(new MiScript(MessageScreen.Instance.EntrySequence));
                        while (Game.InputHandler.Focused == MessageScreen.Instance)
                        {
                            yield return 5;
                        }
                    }
                    break;
                case BattleState.CHARACTER_SELECT:
                    foreach (Character c in Player.Party)
                    {
                        if (positions[c].X == cursorX && positions[c].Y == cursorY)
                        {
                            if (c.CurrMovementPoints <= 0)
                            {
                                MessageScreen.Instance.Message = "Nigga " + c.Name + " has no movement points left and can't do anymore SHIT.";
                                Game.PushScreen(MessageScreen.Instance);
                                Game.ScriptEngine.ExecuteScript(new MiScript(MessageScreen.Instance.EntrySequence));
                            }
                            else
                            {
                                selectedCharacter = c;
                                ChoiceScreen.Instance.Message = "What will " + c.Name + " do with his remaining " + c.CurrMovementPoints + " movement points?";
                                ChoiceScreen.Instance.SetChoices(
                                    new KeyValuePair<string, MiScript>("Attack",
                                        delegate
                                        {
                                            Game.PopScreen();
                                            selectedCharacterX = cursorX;
                                            selectedCharacterY = cursorY;
                                            ChoiceScreen.Instance.Message = "Which Attack?";
                                            KeyValuePair<string, MiScript>[] attacks = new KeyValuePair<string, MiScript>[c.KnownAttacks.Count+1];
                                            for (int i = 0; i < c.KnownAttacks.Count; i++)
                                            {
                                                Attack curr = c.KnownAttacks[i];
                                                attacks[i] = new KeyValuePair<string,MiScript>(curr.Name,new MiScript(
                                                    delegate
                                                    {
                                                        if (c.CurrMovementPoints < curr.MovementCost)
                                                        {
                                                            MessageScreen.Instance.Message = "Nigga " + c.Name + " has not enough moves to do that SHIT.";
                                                            Game.PushScreen(MessageScreen.Instance);
                                                            return MessageScreen.Instance.EntrySequence();
                                                        }
                                                        else
                                                        {
                                                            selectedAttack = curr;
                                                            positions.Remove(c);
                                                            MapFloodFill(cursorX, cursorY, curr.Range, true, Color.Yellow, selectedValidMoves);
                                                            MapFloodFill(cursorX, cursorY, selectedAttack.AOE, true, Color.Salmon, selectedAOE);
                                                            positions[c] = new Point(cursorX, cursorY);
                                                            state = BattleState.CHARACTER_ATTACK;
                                                            return ChoiceScreen.Instance.ExitSequence();
                                                        }
                                                    }));
                                            }
                                            attacks[attacks.Length - 1] = new KeyValuePair<string, MiScript>("Nevermind", new MiScript(ChoiceScreen.Instance.ExitSequence));
                                            ChoiceScreen.Instance.SetChoices(attacks);
                                            Game.PushScreen(ChoiceScreen.Instance);
                                            return ChoiceScreen.Instance.EntrySequence();
                                        }),
                                    new KeyValuePair<string, MiScript>("Move",
                                        delegate
                                        {
                                            selectedCharacterX = cursorX;
                                            selectedCharacterY = cursorY;
                                            positions.Remove(c);
                                            MapFloodFill(cursorX, cursorY, c.CurrMovementPoints, false, Color.Yellow, selectedValidMoves);
                                            colorChanged = true;
                                            state = BattleState.CHARACTER_MOVE;
                                            return ChoiceScreen.Instance.ExitSequence();
                                        }),
                                    new KeyValuePair<string, MiScript>("Nevermind", new MiScript(ChoiceScreen.Instance.ExitSequence)));
                                Game.PushScreen(ChoiceScreen.Instance);
                                Game.ScriptEngine.ExecuteScript(new MiScript(ChoiceScreen.Instance.EntrySequence));
                            }
                            break;
                        }
                    }
                    break;
                case BattleState.CHARACTER_MOVE:
                    if (selectedValidMoves.ContainsKey(new Point(cursorX, cursorY)))
                    {
                        positions[selectedCharacter] = new Point(cursorX, cursorY);
                        selectedCharacterMovePtsUsed = Math.Abs(cursorX - selectedCharacterX) + Math.Abs(cursorY - selectedCharacterY);
                        selectedCharacter.CurrMovementPoints -= selectedCharacterMovePtsUsed;
                        selectedValidMoves.Clear();
                        state = BattleState.NOTIF;
                    }
                    else
                    {
                        MessageScreen.Instance.Message = "Nigga you can't put that SHIT there.";
                        Game.PushScreen(MessageScreen.Instance);
                        Game.ScriptEngine.ExecuteScript(new MiScript(MessageScreen.Instance.EntrySequence));
                        while (Game.InputHandler.Focused == MessageScreen.Instance)
                        {
                            yield return 5;
                        }
                    }
                    break;
                case BattleState.CHARACTER_ATTACK:
                    if (selectedValidMoves.ContainsKey(new Point(cursorX, cursorY)))
                    {
                        selectedCharacter.CurrMovementPoints -= selectedAttack.MovementCost;
                        foreach (Character c in positions.Keys)
                        {
                            if (Math.Abs(positions[c].X - cursorX) + Math.Abs(positions[c].Y - cursorY) <= selectedAttack.AOE)
                            {
                                double dodgeChance = c.Speed * 0.0005;
                                if (random.NextDouble() > dodgeChance)
                                {
                                    int damage = (int)(
                                        (0.5 + random.NextDouble() * 1.5)
                                        * (selectedCharacter.Will * selectedAttack.TraditionalBaseDamage + selectedCharacter.Mind * selectedAttack.FuturistBaseDamage)
                                        - (0.5 + random.NextDouble())
                                        * (c.Will * selectedAttack.TraditionalBaseDamage + c.Mind * selectedAttack.FuturistBaseDamage)
                                        * c.Vitality / 20);
                                    Console.WriteLine(c.Name + " received " + damage + " damage.");
                                    c.CurrHealth -= damage;
                                }
                            }
                        }
                        foreach(Character c in Player.Party.FindAll(c => c.IsDead()))
                        {
                            positions.Remove(c);
                        }
                        foreach (Character c in enemies.FindAll(c => c.IsDead()))
                        {
                            positions.Remove(c);
                        }
                        Player.Party.RemoveAll(c => c.IsDead());
                        enemies.RemoveAll(c => c.IsDead());
                        Console.WriteLine(enemies.Count);
                        colorChanged = true;
                        selectedValidMoves.Clear();
                        selectedAOE.Clear();
                        state = BattleState.NOTIF;
                    }
                    else
                    {
                        MessageScreen.Instance.Message = "Nigga that SHIT's too far to hit.";
                        Game.PushScreen(MessageScreen.Instance);
                        Game.ScriptEngine.ExecuteScript(new MiScript(MessageScreen.Instance.EntrySequence));
                        while (Game.InputHandler.Focused == MessageScreen.Instance)
                        {
                            yield return 5;
                        }
                    }
                    break;
            }
            yield break;
        }

        private void MapFloodFill(int x, int y, int radius, bool includeOccupied, Color color, Dictionary<Point, int> toFill)
        {
            Point p = new Point(x, y);

            if (toFill.ContainsKey(p) && toFill[p] >= radius
                || radius < 0 
                || x < 0 
                || x >= tileEngine.MapGraphics.GetLength(1) 
                || y < 0 
                || y >= tileEngine.MapGraphics.GetLength(0)
                || (!includeOccupied && positions.Values.Contains<Point>(p))
                || !tileEngine.MapPassability[y, x])
                return;

            tileEngine.MapGraphics[y, x].Color = color;
            toFill[p] = radius;
            MapFloodFill(x - 1, y, radius - 1, includeOccupied, color, toFill);
            MapFloodFill(x + 1, y, radius - 1, includeOccupied, color, toFill);
            MapFloodFill(x, y - 1, radius - 1, includeOccupied, color, toFill);
            MapFloodFill(x, y + 1, radius - 1, includeOccupied, color, toFill);
        }

        public IEnumerator<ulong> EnemyTurn()
        {
            foreach (Character c in Player.Party)
            {
                c.CurrMovementPoints = c.MaxMovementPoints;
            }
            state = BattleState.NOTIF;
            yield break;
        }

        public void Undo()
        {
            positions[selectedCharacter] = new Point(selectedCharacterX, selectedCharacterY);
            selectedCharacter.CurrMovementPoints += selectedCharacterMovePtsUsed;
            selectedCharacterMovePtsUsed = 0;
        }
    }
}
