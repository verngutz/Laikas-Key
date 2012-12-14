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
        private int playerFrontX;
        private int playerFrontY;
        private const int PLAYER_MOVE_SPEED = 25;

        private MiTileEngine tileEngine;

        private bool playerMoveMutex;

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
                    playerFrontX = playerX;
                    playerFrontY = playerY - 1;
                    break;
                case AvatarDirection.DOWN:
                    playerFrontX = playerX;
                    playerFrontY = playerY + 1;
                    break;
                case AvatarDirection.LEFT:
                    playerFrontX = playerX - 1;
                    playerFrontY = playerY;
                    break;
                case AvatarDirection.RIGHT:
                    playerFrontX = playerX + 1;
                    playerFrontY = playerY;
                    break;
            }

            tileEngine.LoadMap(l.Map,
                MiResolution.VirtualWidth / 2 - playerAvatar.Width / 2 - playerX * playerAvatar.Width, 
                MiResolution.VirtualHeight / 2 - playerAvatar.Height / 2 - playerY * playerAvatar.Height
            );

            playerAvatar.BoundingRectangle = tileEngine.BoundingRectangle(playerX, playerY);
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
            playerFrontX = playerX;
            playerFrontY = playerY - 1;
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
            playerFrontX = playerX;
            playerFrontY = playerY + 1;
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
            playerFrontY = playerY;
            playerFrontX = playerX - 1;
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
            playerFrontY = playerY;
            playerFrontX = playerX + 1;
            yield break;
        }

        public IEnumerator<ulong> ExamineFront()
        {
            if (playerFrontX == 3 && playerFrontY == 3)
            {
                ChoiceScreen.Show("Choose your fate.",
                    new Choice("Traditionalist",
                        delegate
                        {
                            MessageScreen.Show("Your backwardness is preventing equality for all.");
                            return null;
                        }),
                    new Choice("Futurist",
                        delegate
                        {
                            MessageScreen.Show("Your technology is destroying the earth.");
                            return null;
                        })
                );
            }
            else if (playerFrontX == 5 && playerFrontY == 1)
            {
                ChoiceScreen.Show("Would you like to know more about the war?",
                    new Choice("Yes",
                        delegate
                        {
                            MessageScreen.Show("This war is rooted in the differences of Traditionalists and Futurists.");
                            return null;
                        }),
                    new Choice("No",
                        delegate
                        {
                            MessageScreen.Show("You don't really care do you?");
                            return null;
                        })
                );
            }
            else if (playerFrontX == 5 && playerFrontY == 5)
            {
                ChoiceScreen.Show("Would you rather fight or flee?",
                    new Choice("Fight",
                        delegate
                        {
                            MessageScreen.Show("Careful, don't forget about your team");
                            return null;
                        }),
                    new Choice("Flee",
                        delegate
                        {
                            MessageScreen.Show("You can't always run...");
                            return null;
                        })
                );
            }
            return null;
        }
    }
}
