using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MiUtil
{
    public class MiTileEngine : MiDrawableComponent
    {
        private class MiTileType : MiDrawableComponent
        {
            private string texturePath;
            private Texture2D texture;
            public Texture2D Texture { get { return texture; } }
            private bool passable;
            public bool Passable { get { return passable; } }
            public MiTileType(MiGame game, string texturePath, bool passable)
                : base(game)
            {
                this.texturePath = texturePath;
                this.passable = passable;
            }

            public override void LoadContent()
            {
                texture = Game.Content.Load<Texture2D>(texturePath);
            }
        }

        private class MiTileSprite : MiDrawableComponent
        {
            private MiAnimatingComponent tileGraphic;

            public MiTileSprite(MiGame game, Texture2D texture, int x, int y, int width, int height)
                : base(game)
            {
                tileGraphic = new MiAnimatingComponent(game, x, y, width,height);
                tileGraphic.AddTexture(texture, 0);
            }

            public void Move(int dx, int dy, int dt)
            {
                tileGraphic.XPositionOverTime.Keys.Add(new CurveKey(tileGraphic.Position.X + dx, dt));
                tileGraphic.YPositionOverTime.Keys.Add(new CurveKey(tileGraphic.Position.Y + dy, dt));
            }

            public override void Draw(GameTime gameTime)
            {
                tileGraphic.Draw(gameTime);
            }
        }

        private int tileWidth;
        public int TileWidth { get { return tileWidth; } }

        private int tileHeight;
        public int TileHeight { get { return tileHeight; } }

        private Dictionary<char, MiTileType> tiles;

        private MiAnimatingComponent[,] mapGraphics;
        public MiAnimatingComponent[,] MapGraphics { get { return mapGraphics; } }

        private bool[,] mapPassability;
        public bool[,] MapPassability { get { return mapPassability; } }

        public MiTileEngine(MiGame game, int tileWidth, int tileHeight)
            : base(game)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            tiles = new Dictionary<char, MiTileType>();
        }

        public void AddTileType(char index, string texturePath, bool passable)
        {
            tiles[index] = new MiTileType(Game, texturePath, passable);
        }

        public void LoadMap(char[,] charmap, int init_dx, int init_dy)
        {
            mapGraphics = new MiAnimatingComponent[charmap.GetLength(0), charmap.GetLength(1)];
            mapPassability = new bool[charmap.GetLength(0), charmap.GetLength(1)];
            for (int row = 0; row < charmap.GetLength(0); row++)
            {
                for (int col = 0; col < charmap.GetLength(1); col++)
                {
                    mapGraphics[row, col] = new MiAnimatingComponent(Game, col * tileWidth + init_dx, row * tileHeight + init_dy, tileWidth, tileHeight);
                    mapGraphics[row, col].AddTexture(tiles[charmap[row, col]].Texture, 0);
                    mapPassability[row, col] = tiles[charmap[row, col]].Passable;
                }
            }
        }

        public override void LoadContent()
        {
            foreach (MiTileType tile in tiles.Values)
            {
                tile.LoadContent();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(MiAnimatingComponent tile in mapGraphics)
            {
                tile.Draw(gameTime);
            }
        }
    }
}
