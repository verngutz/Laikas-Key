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
        private bool playerMoveMutex;
        private const int PLAYER_MOVE_SPEED = 25;

        private MiTileEngine tileEngine;
        private Dictionary<Point, MiScript> events;

        public TownScreen(MiGame game, MiTileEngine tileEngine)
            : base(game)
        {
            if (Instance == null)
            {
                //
                // Player Avatar
                //
                playerAvatar = new MiAnimatingComponent(Game, 0, 0, tileEngine.TileWidth, tileEngine.TileHeight);
                playerMoveMutex = false;

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
            tileEngine.Draw(gameTime);
            playerAvatar.Draw(gameTime);
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }

        public IEnumerator<ulong> MoveUp()
        {
            if (playerMoveMutex)
                yield break;

            playerAvatar.SpriteState = AvatarDirection.UP;
            if (tileEngine.MapPassability[playerY - 1, playerX])
            {
                playerMoveMutex = true;
                playerY--;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.SetMovement(0, tileEngine.TileHeight, PLAYER_MOVE_SPEED);
                    tileGraphic.MoveEnabled = true;
                }
                yield return PLAYER_MOVE_SPEED;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.MoveEnabled = false;
                }
                playerMoveMutex = false;
            }
            playerFront.X = playerX;
            playerFront.Y = playerY - 1;
            yield break;
        }

        public IEnumerator<ulong> MoveDown()
        {
            if (playerMoveMutex)
                yield break;

            playerAvatar.SpriteState = AvatarDirection.DOWN;
            if (tileEngine.MapPassability[playerY + 1, playerX])
            {
                playerMoveMutex = true;
                playerY++;
                foreach(MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.SetMovement(0, -tileEngine.TileHeight, PLAYER_MOVE_SPEED);
                    tileGraphic.MoveEnabled = true;
                }
                yield return PLAYER_MOVE_SPEED;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.MoveEnabled = false;
                }
                playerMoveMutex = false;
            }
            playerFront.X = playerX;
            playerFront.Y = playerY + 1;
            yield break;
        }

        public IEnumerator<ulong> MoveLeft()
        {
            if (playerMoveMutex)
                yield break;

            playerAvatar.SpriteState = AvatarDirection.LEFT;
            if (tileEngine.MapPassability[playerY, playerX - 1])
            {
                playerMoveMutex = true;
                playerX--;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.SetMovement(tileEngine.TileWidth, 0, PLAYER_MOVE_SPEED);
                    tileGraphic.MoveEnabled = true;
                }
                yield return PLAYER_MOVE_SPEED;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.MoveEnabled = false;
                }
                playerMoveMutex = false;
            }
            playerFront.Y = playerY;
            playerFront.X = playerX - 1;
            yield break;
        }

        public IEnumerator<ulong> MoveRight()
        {
            if (playerMoveMutex)
                yield break;

            playerAvatar.SpriteState = AvatarDirection.RIGHT;
            if (tileEngine.MapPassability[playerY, playerX + 1])
            {
                playerMoveMutex = true;
                playerX++;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.SetMovement(-tileEngine.TileWidth, 0, PLAYER_MOVE_SPEED);
                    tileGraphic.MoveEnabled = true;
                }
                yield return PLAYER_MOVE_SPEED;
                foreach (MiAnimatingComponent tileGraphic in tileEngine.MapGraphics)
                {
                    tileGraphic.MoveEnabled = false;
                }
                playerMoveMutex = false;
            }
            playerFront.Y = playerY;
            playerFront.X = playerX + 1;
            yield break;
        }

        public IEnumerator<ulong> ExamineFront()
        {
            if (events.ContainsKey(playerFront))
            {
                Game.ScriptEngine.ExecuteScript(events[playerFront]);
            }
            return null;
        }
    }
}
