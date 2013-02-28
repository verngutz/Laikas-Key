using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Choice = System.Collections.Generic.KeyValuePair<string, MiUtil.MiScript>;
namespace Laikas_Key
{
    class BattleScreen : MiScreen
    {
        public static BattleScreen Instance { set; get; }

        public enum BattleState { SETUP, CHARACTER_SELECT, CHARACTER_MOVE, CHARACTER_ATTACK, ENEMY_TURN, NOTIF }
        public BattleState State { get; set; }

        private MiTileEngine tileEngine;
        private List<Character> enemies;
        public List<Character> Enemies { get { return enemies; } }
        private Dictionary<Character, Point> positions;
        public Dictionary<Character, Point> Positions { get { return positions; } }

        private MiAnimatingComponent cursor;
        private int cursorX;
        private int cursorY;

        private int setupIndex;
        public int SetupIndex { get { return setupIndex; } }

        bool colorChanged = false;

        public const int ALLOWED_INITIAL_REGION = 2;
        private Character selectedCharacter;
        private int selectedCharacterX;
        private int selectedCharacterY;
        private Dictionary<Point, int> selectedValidMoves;
        public Dictionary<Point, int> SelectedValidMoves { get { return selectedValidMoves; } }
        private Dictionary<Point, int> selectedAOE;
        public Dictionary<Point, int> SelectedAOE { get { return selectedAOE; } }
        private Attack selectedAttack;
        private int selectedCharacterMovePtsUsed;

        private Random random;

        public BattleScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            if (Instance == null)
            {
                flash = new MiAnimatingComponent(Game, 0, 0, 1280, 800, 0, 0, 0, 0);
                this.tileEngine = tileEngine;
                inputResponses[Controller.START] = Escaped;
                inputResponses[Controller.LEFT] = Lefted;
                inputResponses[Controller.RIGHT] = Righted;
                inputResponses[Controller.UP] = Upped;
                inputResponses[Controller.DOWN] = Downed;
                inputResponses[Controller.A] = Pressed;

                positions = new Dictionary<Character, Point>();

                cursor = new MiAnimatingComponent(game, 0, 0, 0, 0);
                cursor.Color = Color.Yellow;

                selectedValidMoves = new Dictionary<Point, int>();
                selectedAOE = new Dictionary<Point, int>();

                random = new Random();
            }
            else
            {
                throw new Exception("Battle Screen Already Initialized");
            }
        }

        public void Activate(LocationData l)
        {
            tileEngine.LoadMap(l.Map, 50, 50);
            Game.PushScreen(this);
            Game.ScriptEngine.ExecuteScript(EntrySequence);
            cursorX = 0;
            cursorY = 0;
            cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
            setupIndex = 0;

            enemies = EnemyGenerator.Generate();
            State = BattleState.NOTIF;
            positions.Clear();

            foreach (Character c in Player.Party)
            {
                c.CurrMovementPoints = c.MaxMovementPoints;
            }
            
            Game.ScriptEngine.ExecuteScript(AI.Stupid);
        }

        public override void LoadContent()
        {
            cursor.AddTexture(Game.Content.Load<Texture2D>("buttonHover"), 0);
            flash.AddTexture(Game.Content.Load<Texture2D>("button"), 0);
        }

        public override void Draw(GameTime gameTime)
        {
            tileEngine.Draw(gameTime);
            foreach (Character c in positions.Keys)
            {
                Rectangle cBounds = tileEngine.BoundingRectangle(positions[c].X, positions[c].Y);
                if (State == BattleState.CHARACTER_ATTACK && selectedAOE.ContainsKey(positions[c]))
                {
                    if (Player.Party.Contains(c))
                    {
                        Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), cBounds, Color.Red);
                    }
                    else
                    {
                        Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("Town View\\GenericEnemy"), cBounds, Color.Red);
                    }
                }
                else if (State == BattleState.CHARACTER_ATTACK && selectedValidMoves.ContainsKey(positions[c]))
                {
                    if (Player.Party.Contains(c))
                    {
                        Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), cBounds, Color.Yellow);
                    }
                    else
                    {
                        Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("Town View\\GenericEnemy"), cBounds, Color.Yellow);
                    }
                }
                else
                {
                    if (Player.Party.Contains(c))
                    {
                        Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), cBounds, Color.White);
                    }
                    else
                    {
                        Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("Town View\\GenericEnemy"), cBounds, Color.White);
                    }
                }
            }
            foreach (Character c in positions.Keys)
            {
                Rectangle cBounds = tileEngine.BoundingRectangle(positions[c].X, positions[c].Y);
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("button"), new Rectangle(cBounds.X, cBounds.Top - 40, cBounds.Width, 20), Color.Red);
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("button"), new Rectangle(cBounds.X, cBounds.Top - 40, c.CurrHealth * cBounds.Width / c.MaxHealth, 20), Color.Green);
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("button"), new Rectangle(cBounds.X, cBounds.Top - 20, cBounds.Width, 20), Color.Brown);
                Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>("button"), new Rectangle(cBounds.X, cBounds.Top - 20, c.CurrMovementPoints * cBounds.Width / c.MaxMovementPoints, 20), Color.Blue);
                Game.SpriteBatch.DrawString(
                    Game.Content.Load<SpriteFont>("Fonts\\Default"),
                    c.Name,
                    new Vector2(tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).X, tileEngine.BoundingRectangle(positions[c].X, positions[c].Y).Y - 40),
                    Color.White);
            }
            switch (State)
            {
                case BattleState.CHARACTER_ATTACK:
                case BattleState.CHARACTER_MOVE:
                case BattleState.CHARACTER_SELECT:
                case BattleState.SETUP:
                    cursor.Draw(gameTime);
                    break;
            }

            flash.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            flash.Update(gameTime);
            if (Game.InputHandler.Focused is DialogScreen)
            {
                return;
            }

            if (Player.Party.Count == 0)
            {
                ChoiceScreen.Show("You died!", new Choice("Yeah Whatever...", Escaped));
                return;
            }

            if (enemies.Count == 0)
            {
                ChoiceScreen.Show("You won!", new Choice("Yeah Whatever...", Escaped));
                return;
            }

            if(!waiting)
            switch (State)
            {
                case BattleState.NOTIF:
                    if (setupIndex < Player.Party.Count)
                    {
                        MessageScreen.Show("Select Location for " + Player.Party[setupIndex].Name);
                        for (int row = 0; row < tileEngine.MapGraphics.GetLength(0); row++)
                        {
                            for (int col = 0; col < ALLOWED_INITIAL_REGION; col++)
                            {
                                tileEngine.MapGraphics[row, col].Color = Color.Yellow;
                            }
                        }
                        colorChanged = true;
                        State = BattleState.SETUP;
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
                        ChoiceScreen.Show("What to do?",
                            new Choice("Fight", Fight),
                            new Choice("End Turn",
                                delegate
                                {
                                    foreach (Character c in Player.Party)
                                    {
                                        c.CurrMovementPoints = c.MaxMovementPoints;
                                    }
                                    State = BattleState.ENEMY_TURN;
                                    return null;
                                }),
                            new Choice("Run", ExitSequence));
                    }
                    break;
            }
                
            cursor.Update(gameTime);
        }

        private bool waiting = false;
        public IEnumerator<ulong> Fight()
        {
            MessageScreen.Show("Select a character to move.");
            waiting = true;
            while (Game.InputHandler.Focused is DialogScreen)
                yield return 5;
            waiting = false;
            State = BattleState.CHARACTER_SELECT;
            yield break;
        }

        public override IEnumerator<ulong> EntrySequence()
        {
            MessageScreen.Show("Setup Phase");
            while (Game.InputHandler.Focused is DialogScreen)
                yield return 5;
        }

        public override IEnumerator<ulong> ExitSequence()
        {
            AI.Terminate();
            WorldScreen.Instance.Activate();
            yield break;
        }

        public IEnumerator<ulong> MoveCursor(TownScreen.AvatarDirection dir)
        {
            bool updated = false;
            switch (dir)
            {
                case TownScreen.AvatarDirection.UP:
                    if (cursorY > 0)
                    {
                        cursorY--;
                        updated = true;
                    }
                    break;
                case TownScreen.AvatarDirection.DOWN:
                    if (cursorY < tileEngine.MapGraphics.GetLength(0) - 1)
                    {
                        cursorY++;
                        updated = true;
                    }
                    break;
                case TownScreen.AvatarDirection.LEFT:
                    if (cursorX > 0)
                    {
                        cursorX--;
                        updated = true;
                    }
                    break;
                case TownScreen.AvatarDirection.RIGHT:
                    if (cursorX < tileEngine.MapGraphics.GetLength(1) - 1)
                    {
                        cursorX++;
                        updated = true;
                    }
                    break;
            }
            if (updated)
            {
                cursor.BoundingRectangle = tileEngine.BoundingRectangle(cursorX, cursorY);
                if (State == BattleState.CHARACTER_ATTACK)
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

        public IEnumerator<ulong> Upped() { return MoveCursor(TownScreen.AvatarDirection.UP); }
        public IEnumerator<ulong> Downed() { return MoveCursor(TownScreen.AvatarDirection.DOWN); }
        public IEnumerator<ulong> Lefted() { return MoveCursor(TownScreen.AvatarDirection.LEFT); }
        public IEnumerator<ulong> Righted() { return MoveCursor(TownScreen.AvatarDirection.RIGHT); }

        public IEnumerator<ulong> Escaped()
        {
            switch (State)
            {
                case BattleState.CHARACTER_SELECT:
                    State = BattleState.NOTIF;
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
                    State = BattleState.CHARACTER_SELECT;
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
                    State = BattleState.CHARACTER_SELECT;
                    return null;
                default:
                    return ExitSequence();
            }
        }

        public bool SelectMove(Character c)
        {
            if (c.CurrMovementPoints <= 0) return false;
            selectedCharacter = c;
            selectedCharacterX = positions[c].X;
            selectedCharacterY = positions[c].Y;
            positions.Remove(c);
            selectedValidMoves.Clear();
            MapFloodFill(selectedCharacterX, selectedCharacterY, c.CurrMovementPoints, false, Color.Yellow, selectedValidMoves);
            System.Console.WriteLine(c.Name + ": " + selectedCharacterX + ", " + selectedCharacterY);
            foreach(Point p in selectedValidMoves.Keys)
                System.Console.WriteLine(p.X + " " + p.Y);
            colorChanged = true;
            return true;
        }

        public bool SelectAttack(Character c, Attack a)
        {
            if (c.CurrMovementPoints < a.MovementCost) return false;
            selectedCharacter = c;
            selectedAttack = a;
            int x = positions[c].X;
            int y = positions[c].Y;
            positions.Remove(c);
            MapFloodFill(x, y, a.Range, true, Color.Yellow, selectedValidMoves);
            RecalculateAOE(x, y, a.AOE);
            colorChanged = true;
            positions[c] = new Point(x, y);
            return true;
        }

        public bool Move(int x, int y)
        {
            Point pos = new Point(x, y);
            if (selectedValidMoves.ContainsKey(pos))
            {
                positions[selectedCharacter] = pos;
                selectedCharacterMovePtsUsed = Math.Abs(x - selectedCharacterX) + Math.Abs(y - selectedCharacterY);
                selectedCharacter.CurrMovementPoints -= selectedCharacterMovePtsUsed;
                selectedValidMoves.Clear();
                return true;
            }
            return false;
        }

        public bool Attack(int x, int y)
        {
            Point pos = new Point(x, y);
            if (selectedValidMoves.ContainsKey(pos))
            {
                selectedCharacter.CurrMovementPoints -= selectedAttack.MovementCost;
                foreach (Character c in positions.Keys)
                {
                    if (Math.Abs(positions[c].X - x) + Math.Abs(positions[c].Y - y) <= selectedAttack.AOE)
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
                            c.CurrHealth -= damage;
                        }
                    }
                }
                foreach (Character c in Player.Party.FindAll(c => c.IsDead()))
                {
                    positions.Remove(c);
                }
                foreach (Character c in enemies.FindAll(c => c.IsDead()))
                {
                    positions.Remove(c);
                }
                Player.Party.RemoveAll(c => c.IsDead());
                enemies.RemoveAll(c => c.IsDead());
                selectedValidMoves.Clear();
                selectedAOE.Clear();
                Game.ScriptEngine.ExecuteScript(Flash);
                return true;
            }
            return false;
        }

        public IEnumerator<ulong> Pressed()
        {
            switch (State)
            {
                case BattleState.SETUP:
                    if (cursorX < ALLOWED_INITIAL_REGION && !positions.Values.Contains<Point>(new Point(cursorX, cursorY)) && tileEngine.MapPassability[cursorY, cursorX])
                    {
                        positions[Player.Party[setupIndex]] = new Point(cursorX, cursorY);
                        setupIndex++;
                        State = BattleState.NOTIF;
                    }
                    else
                    {
                        MessageScreen.Show("You can't use that position.");
                        while (Game.InputHandler.Focused is DialogScreen)
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
                                MessageScreen.Show(c.Name + " has no movement points left and can't do anymore actions.");
                            }
                            else
                            {
                                ChoiceScreen.Show("What will " + c.Name + " do with his remaining " + c.CurrMovementPoints + " movement points?",
                                    new Choice("Attack",
                                        delegate
                                        {
                                            selectedCharacterX = cursorX;
                                            selectedCharacterY = cursorY;
                                            Choice[] attacks = new Choice[c.KnownAttacks.Count+1];
                                            for (int i = 0; i < c.KnownAttacks.Count; i++)
                                            {
                                                Attack curr = c.KnownAttacks[i];
                                                attacks[i] = new Choice(curr.Name,
                                                    delegate
                                                    {
                                                        if (c.CurrMovementPoints < curr.MovementCost)
                                                        {
                                                            MessageScreen.Show(c.Name + " does not have enough movement points for that.");
                                                        }
                                                        else
                                                        {
                                                            SelectAttack(c, curr);
                                                            State = BattleState.CHARACTER_ATTACK;
                                                        }
                                                        return null;
                                                    });
                                            }
                                            attacks[attacks.Length - 1] = new Choice("Nevermind", MiScreen.DoNothing);
                                            ChoiceScreen.Show("Which Attack?", attacks);
                                            return null;
                                        }),
                                    new Choice("Move",
                                        delegate
                                        {
                                            SelectMove(c);
                                            State = BattleState.CHARACTER_MOVE;
                                            return null;
                                        }),
                                    new Choice("Nevermind", MiScreen.DoNothing)
                                );
                            }
                            break;
                        }
                    }
                    break;
                case BattleState.CHARACTER_MOVE:
                    if (Move(cursorX, cursorY))
                        State = BattleState.NOTIF;
                    else
                    {
                        MessageScreen.Show("You can't choose that position");
                        while (Game.InputHandler.Focused is DialogScreen)
                        {
                            yield return 5;
                        }
                    }
                    break;
                case BattleState.CHARACTER_ATTACK:
                    if (Attack(cursorX, cursorY))
                    {
                        colorChanged = true;
                        State = BattleState.NOTIF;
                    }
                    else
                    {
                        MessageScreen.Show("Target out of range.");
                        while (Game.InputHandler.Focused is DialogScreen)
                        {
                            yield return 5;
                        }
                    }
                    break;
            }
            yield break;
        }

        public void RecalculateAOE(int x, int y, int aoe)
        {
            SelectedAOE.Clear();
            MapFloodFill(x, y, aoe, true, Color.Salmon, selectedAOE);
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

        public void Undo()
        {
            positions[selectedCharacter] = new Point(selectedCharacterX, selectedCharacterY);
            selectedCharacter.CurrMovementPoints += selectedCharacterMovePtsUsed;
            selectedCharacterMovePtsUsed = 0;
        }

        private MiAnimatingComponent flash;
        public IEnumerator<ulong> Flash()
        {
            flash.AlphaChangeEnabled = true;
            flash.AlphaOverTime.Keys.Add(new CurveKey(flash.AlphaChangeTimer + 12, 255));
            flash.AlphaOverTime.Keys.Add(new CurveKey(flash.AlphaChangeTimer + 25, 0));
            yield return 37;
            flash.AlphaChangeEnabled = false;
        }
    }
}
