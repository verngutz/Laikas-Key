using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Laikas_Key
{
    class WorldScreen : MiScreen
    {
        public static WorldScreen Instance { set; get; }

        private class LocationUI
        {
            private MiGame game;
            public Dictionary<MiControl, LocationUI> Neighbors { set; get; }
            
            public MiAnimatingComponent ButtonBase
            {
                get
                {
                    switch (location.ControllingFaction)
                    {
                        case Location.State.ALLY:
                            return allyButtonBase;
                        case Location.State.ENEMY:
                            return enemyButtonBase;
                        case Location.State.NEUTRAL:
                            return neutralButtonBase;
                    }
                    return null;
                }
            }

            private MiAnimatingComponent allyButtonBase;
            private MiAnimatingComponent enemyButtonBase;
            private MiAnimatingComponent neutralButtonBase;

            private Location location;
            public string Label { get { return location.Name; } }

            public LocationUI(MiGame game, int x, int y, Location location)
            {
                this.game = game;
                Neighbors = new Dictionary<MiControl, LocationUI>();
                allyButtonBase = new MiAnimatingComponent(game, x, y);
                enemyButtonBase = new MiAnimatingComponent(game, x, y);
                neutralButtonBase = new MiAnimatingComponent(game, x, y);
                this.location = location;
            }

            public void LoadContent()
            {
                allyButtonBase.AddTexture(game.Content.Load<Texture2D>("buttonAlly"), 0);
                enemyButtonBase.AddTexture(game.Content.Load<Texture2D>("buttonEnemy"), 0);
                neutralButtonBase.AddTexture(game.Content.Load<Texture2D>("button"), 0);
            }
        }

        private LocationUI activeLocation;
        private List<LocationUI> allLocations;

        private TownScreen townScreen;
        private BattleScreen battleScreen;

        private MiAnimatingComponent cursor;

        public WorldScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                //
                // Create UI for Locations
                //
                LocationUI test_1 = new LocationUI(game, 100, 100, Locations.TEST_1);
                LocationUI test_2 = new LocationUI(game, 100, 400, Locations.TEST_2);
                LocationUI test_3 = new LocationUI(game, 400, 100, Locations.TEST_3);
                LocationUI test_4 = new LocationUI(game, 400, 400, Locations.TEST_4);

                //
                // Add Neighbors
                //
                test_1.Neighbors.Add(Controller.DOWN, test_2);
                test_1.Neighbors.Add(Controller.RIGHT, test_3);

                test_2.Neighbors.Add(Controller.UP, test_1);
                test_2.Neighbors.Add(Controller.RIGHT, test_4);

                test_3.Neighbors.Add(Controller.LEFT, test_1);
                test_3.Neighbors.Add(Controller.DOWN, test_4);

                test_4.Neighbors.Add(Controller.LEFT, test_2);
                test_4.Neighbors.Add(Controller.UP, test_3);

                //
                // Add All Locations to Global List
                //
                allLocations = new List<LocationUI>();
                allLocations.Add(test_1);
                allLocations.Add(test_2);
                allLocations.Add(test_3);
                allLocations.Add(test_4);

                //
                // Default Active Location
                //
                activeLocation = test_1;

                //
                // Cursor
                //
                cursor = new MiAnimatingComponent(game, activeLocation.ButtonBase.Position.X, activeLocation.ButtonBase.Position.Y);
            }
            else
            {
                throw new Exception("World Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            foreach (LocationUI locationUI in allLocations)
            {
                locationUI.LoadContent();
            }
            cursor.AddTexture(Game.Content.Load<Texture2D>("buttonHover"), 0);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (LocationUI locationUI in allLocations)
            {
                locationUI.ButtonBase.Draw(gameTime);
                Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), locationUI.Label, locationUI.ButtonBase.Position + new Vector2(0, 20), Color.White);
            }
            cursor.Draw(gameTime);
        }

        public override IEnumerator<ulong> Upped()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.UP))
            {
                activeLocation = activeLocation.Neighbors[Controller.UP];
                cursor.Position = activeLocation.ButtonBase.Position;
            }
            yield break;
        }

        public override IEnumerator<ulong> Downed()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.DOWN))
            {
                activeLocation = activeLocation.Neighbors[Controller.DOWN];
                cursor.Position = activeLocation.ButtonBase.Position;
            }
            yield break;
        }

        public override IEnumerator<ulong> Lefted()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.LEFT))
            {
                activeLocation = activeLocation.Neighbors[Controller.LEFT];
                cursor.Position = activeLocation.ButtonBase.Position;
            }
            yield break;
        }

        public override IEnumerator<ulong> Righted()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.RIGHT))
            {
                activeLocation = activeLocation.Neighbors[Controller.RIGHT];
                cursor.Position = activeLocation.ButtonBase.Position;
            }
            yield break;
        }
    }
}
