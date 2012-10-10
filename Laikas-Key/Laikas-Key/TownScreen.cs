using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Laikas_Key
{
    class TownScreen : MiScreen
    {
        public static TownScreen Instance { set; get; }

        private enum AvatarDirection { UP, DOWN, LEFT, RIGHT }
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
                playerX = 1;
                playerY = 1;
                playerFrontX = 1;
                playerFrontY = 2;
                playerAvatar = new MiAnimatingComponent(Game, 0, 0, tileEngine.TileWidth, tileEngine.TileHeight);
                playerAvatar.SpriteState = AvatarDirection.DOWN;
                playerMoveMutex = false;

                this.tileEngine = tileEngine;

                inputResponses[Controller.START] = new MiScript(Escape);
                inputResponses[Controller.UP] = new MiScript(MoveUp);
                inputResponses[Controller.DOWN] = new MiScript(MoveDown);
                inputResponses[Controller.LEFT] = new MiScript(MoveLeft);
                inputResponses[Controller.RIGHT] = new MiScript(MoveRight);
                inputResponses[Controller.A] = new MiScript(ExamineFront);
            }
            else
            {
                throw new Exception("Town Screen Already Initialized"); 
            }
        }

        public void LoadMap()
        {
            tileEngine.LoadMap(
                new char[,]
                {
                    {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                    {'r', 'g', 'g', 'g', 'g', 'w', 'r'},
                    {'r', 'g', 'r', 'r', 'g', 'g', 'r'},
                    {'r', 'g', 'g', 't', 'r', 'g', 'r'},
                    {'r', 'g', 'r', 'g', 'r', 'g', 'r'},
                    {'r', 'g', 'r', 'g', 'g', 'q', 'r'},
                    {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
                },
                MiResolution.VirtualWidth / 2 - playerAvatar.Width / 2 - playerX * playerAvatar.Width, 
                MiResolution.VirtualHeight / 2 - playerAvatar.Height / 2 - playerY * playerAvatar.Height
            );

            playerAvatar.BoundingRectangle = tileEngine.BoundingRectangle(playerX, playerY);
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
                    tileGraphic.YPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.Y + tileEngine.TileHeight));
                    tileGraphic.XPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.X));
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
                    tileGraphic.YPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.Y - tileEngine.TileHeight));
                    tileGraphic.XPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.X));
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
                    tileGraphic.XPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.X + tileEngine.TileWidth));
                    tileGraphic.YPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.Y));
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
                    tileGraphic.XPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.X - tileEngine.TileWidth));
                    tileGraphic.YPositionOverTime.Keys.Add(new CurveKey(tileGraphic.MoveTimer + PLAYER_MOVE_SPEED, tileGraphic.Position.Y));
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
                ChoiceScreen.Instance.Message = "Choose your fate.";
                ChoiceScreen.Instance.SetChoices(
                    new KeyValuePair<string, MiScript>("Traditionalist", new MiScript(
                        delegate 
                        {
                            Game.PopScreen();
                            MessageScreen.Instance.Message = "Your backwardness is preventing equality for all.";
                            Game.PushScreen(MessageScreen.Instance);
                            return MessageScreen.Instance.EntrySequence();
                        })),
                    new KeyValuePair<string, MiScript>("Futurist", new MiScript(
                        delegate
                        {
                            Game.PopScreen();
                            MessageScreen.Instance.Message = "Your technology is destroying the earth.";
                            Game.PushScreen(MessageScreen.Instance);
                            return MessageScreen.Instance.EntrySequence();
                        }))
                );

                Game.PushScreen(ChoiceScreen.Instance);
                return ChoiceScreen.Instance.EntrySequence();
            }
            else if (playerFrontX == 5 && playerFrontY == 1)
            {
                ChoiceScreen.Instance.Message = "Would you like to know more about the war?";
                ChoiceScreen.Instance.SetChoices(
                    new KeyValuePair<string, MiScript>("Yes", new MiScript(
                        delegate
                        {
                            Game.PopScreen();
                            MessageScreen.Instance.Message = "This war is rooted in the differences of Traditionalists and Futurists.";
                            Game.PushScreen(MessageScreen.Instance);
                            return MessageScreen.Instance.EntrySequence();
                        })),
                    new KeyValuePair<string, MiScript>("No", new MiScript(
                        delegate
                        {
                            Game.PopScreen();
                            MessageScreen.Instance.Message = "You don't really care do you?";
                            Game.PushScreen(MessageScreen.Instance);
                            return MessageScreen.Instance.EntrySequence();
                        }))
                );

                Game.PushScreen(ChoiceScreen.Instance);
                return ChoiceScreen.Instance.EntrySequence();
            }
            else if (playerFrontX == 5 && playerFrontY == 5)
            {
                ChoiceScreen.Instance.Message = "Would you rather fight or flee?";
                ChoiceScreen.Instance.SetChoices(
                    new KeyValuePair<string, MiScript>("Fight", new MiScript(
                        delegate
                        {
                            Game.PopScreen();
                            MessageScreen.Instance.Message = "Careful, don't forget about your team";
                            Game.PushScreen(MessageScreen.Instance);
                            return MessageScreen.Instance.EntrySequence();
                        })),
                    new KeyValuePair<string, MiScript>("Flee", new MiScript(
                        delegate
                        {
                            Game.PopScreen();
                            MessageScreen.Instance.Message = "You can't always run...";
                            Game.PushScreen(MessageScreen.Instance);
                            return MessageScreen.Instance.EntrySequence();
                        }))
                );

                Game.PushScreen(ChoiceScreen.Instance);
                return ChoiceScreen.Instance.EntrySequence();
            }
            return null;
        }
    }
}
