using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Choice = System.Collections.Generic.KeyValuePair<string, MiUtil.MiScript>;
namespace Laikas_Key
{
    class TownScreen : MiScreen
    {
        public static TownScreen Instance { set; get; }

        public enum AvatarDirection { UP, DOWN, LEFT, RIGHT }
        private MiAnimatingComponent playerAvatar;

        private int playerX;
        private int playerY;
        private Point playerFront;
        public bool PlayerMoveEnabled { get; set; }
        private const int PLAYER_MOVE_SPEED = 25;

        private MiTileEngine tileEngine;
        private Dictionary<Point, MiScript> events;

        public TownScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            if (Instance == null)
            {
                playerAvatar = new MiAnimatingComponent(Game, 0, 0, tileEngine.TileWidth, tileEngine.TileHeight);
                PlayerMoveEnabled = true;

                this.tileEngine = tileEngine;

                inputResponses[Controller.START] = Escape;
                inputResponses[Controller.UP] = MoveUp;
                inputResponses[Controller.DOWN] = MoveDown;
                inputResponses[Controller.LEFT] = MoveLeft;
                inputResponses[Controller.RIGHT] = MoveRight;
                inputResponses[Controller.A] = ExamineFront;
            }
            else
            {
                throw new Exception("Town Screen Already Initialized"); 
            }
        }

        public void Activate(LocationData l)
        {
            playerX = l.TownEntryX;
            playerY = l.TownEntryY;
            playerAvatar.SpriteState = l.TownEntryDirection;
            switch (l.TownEntryDirection)
            {
                case AvatarDirection.UP:
                    playerFront.X = playerX;
                    playerFront.Y = playerY - 1;
                    break;
                case AvatarDirection.DOWN:
                    playerFront.X = playerX;
                    playerFront.Y = playerY + 1;
                    break;
                case AvatarDirection.LEFT:
                    playerFront.X = playerX - 1;
                    playerFront.Y = playerY;
                    break;
                case AvatarDirection.RIGHT:
                    playerFront.X = playerX + 1;
                    playerFront.Y = playerY;
                    break;
            }

            tileEngine.LoadMap(l.Map,
                MiResolution.VirtualWidth / 2 - playerAvatar.Width / 2 - playerX * playerAvatar.Width, 
                MiResolution.VirtualHeight / 2 - playerAvatar.Height / 2 - playerY * playerAvatar.Height
            );

            playerAvatar.BoundingRectangle = tileEngine.BoundingRectangle(playerX, playerY);

            events = l.Events;
            Game.PushScreen(this);
            Game.ScriptEngine.ExecuteScript(EntrySequence);
        }

        public override void LoadContent()
        {
            playerAvatar.AddTexture(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), AvatarDirection.UP, 0);
            playerAvatar.AddTexture(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), AvatarDirection.DOWN, 0);
            playerAvatar.AddTexture(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), AvatarDirection.LEFT, 0);
            playerAvatar.AddTexture(Game.Content.Load<Texture2D>("Town View\\GenericFriend"), AvatarDirection.RIGHT, 0);
        }

        public override void Update(GameTime gameTime)
        {
            tileEngine.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), playerX + " " + playerY, new Vector2(20, 20), Color.White);
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), playerFront.ToString(), new Vector2(20, 40), Color.White);
            tileEngine.Draw(gameTime);
            playerAvatar.Draw(gameTime);
        }

        public IEnumerator<ulong> MoveUp() { return Move(AvatarDirection.UP); }
        public IEnumerator<ulong> MoveDown() { return Move(AvatarDirection.DOWN); }
        public IEnumerator<ulong> MoveLeft() { return Move(AvatarDirection.LEFT); }
        public IEnumerator<ulong> MoveRight() { return Move(AvatarDirection.RIGHT); }

        public IEnumerator<ulong> ExamineFront()
        {
            if (PlayerMoveEnabled)
            {
                if (events.ContainsKey(playerFront))
                {
                    return events[playerFront]();
                }
            }
            return null;
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }

        private IEnumerator<ulong> Move(AvatarDirection dir)
        {
            if (PlayerMoveEnabled)
            {
                PlayerMoveEnabled = false;
                playerAvatar.SpriteState = dir;
                int newX = playerX;
                int newY = playerY;
                int tileXMovement = 0;
                int tileYMovement = 0;
                switch (dir)
                {
                    case AvatarDirection.UP:
                        newY--;
                        tileYMovement = tileEngine.TileHeight;
                        break;
                    case AvatarDirection.DOWN:
                        newY++;
                        tileYMovement = -tileEngine.TileHeight;
                        break;
                    case AvatarDirection.LEFT:
                        newX--;
                        tileXMovement = tileEngine.TileWidth;
                        break;
                    case AvatarDirection.RIGHT:
                        newX++;
                        tileXMovement = -tileEngine.TileWidth;
                        break;
                }
                if (tileEngine.MapPassability[newY, newX])
                {
                    playerX = newX;
                    playerY = newY;
                    foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                    {
                        tileGraphic.SetMovement(tileXMovement, tileYMovement, PLAYER_MOVE_SPEED);
                        tileGraphic.MoveEnabled = true;
                    }
                    yield return PLAYER_MOVE_SPEED;
                    foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                    {
                        tileGraphic.MoveEnabled = false;
                    }
                }
                switch (dir)
                {
                    case AvatarDirection.UP:
                        playerFront.X = playerX;
                        playerFront.Y = playerY - 1;
                        break;
                    case AvatarDirection.DOWN:
                        playerFront.X = playerX;
                        playerFront.Y = playerY + 1;
                        break;
                    case AvatarDirection.LEFT:
                        playerFront.X = playerX - 1;
                        playerFront.Y = playerY;
                        break;
                    case AvatarDirection.RIGHT:
                        playerFront.X = playerX + 1;
                        playerFront.Y = playerY;
                        break;
                }
                PlayerMoveEnabled = true;
                yield break;
            }
            yield break;
        }
    }
}
