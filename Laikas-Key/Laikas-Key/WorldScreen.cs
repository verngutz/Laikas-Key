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
                        case LocationData.State.ALLY:
                            return allyButtonBase;
                        case LocationData.State.ENEMY:
                            return enemyButtonBase;
                        case LocationData.State.NEUTRAL:
                            return neutralButtonBase;
                    }
                    return null;
                }
            }

            private MiAnimatingComponent allyButtonBase;
            private MiAnimatingComponent enemyButtonBase;
            private MiAnimatingComponent neutralButtonBase;

            private LocationData location;
            public LocationData LocationData { get { return location; } }
            public string Label { get { return location.Name; } }
            public LocationData.State ControllingFaction { get { return location.ControllingFaction; } }

            public LocationUI(MiGame game, int x, int y, int width, int height, LocationData location)
            {
                this.game = game;
                Neighbors = new Dictionary<MiControl, LocationUI>();
                allyButtonBase = new MiAnimatingComponent(game, x, y, width, height);
                enemyButtonBase = new MiAnimatingComponent(game, x, y, width, height);
                neutralButtonBase = new MiAnimatingComponent(game, x, y, width, height);
                this.location = location;
            }

            public void LoadContent()
            {
                allyButtonBase.AddTexture(game.Content.Load<Texture2D>("World View\\FriendNode_v1"), 0);
                enemyButtonBase.AddTexture(game.Content.Load<Texture2D>("World View\\EnemyNode_v1"), 0);
                neutralButtonBase.AddTexture(game.Content.Load<Texture2D>("button"), 0);
            }
        }

        private MiAnimatingComponent background;
        private LocationUI activeLocation;
        private List<LocationUI> allLocations;

        private MiAnimatingComponent cursor;

        public WorldScreen(MiGame game)
            : base(game)
        {
            if (Instance == null)
            {
                background = new MiAnimatingComponent(game, 0, 0, 1280, 800);
                //
                // Create UI for Locations
                //
                LocationUI test_1 = new LocationUI(game, 650, 250, 128, 128, LocationData.TEST_1);
                LocationUI test_2 = new LocationUI(game, 450, 450, 128, 128, LocationData.TEST_2);

                LocationData.TEST_1.ControllingFaction = LocationData.State.ENEMY;

                //
                // Add Neighbors
                //
                test_1.Neighbors.Add(Controller.DOWN, test_2);
                test_2.Neighbors.Add(Controller.UP, test_1);

                //
                // Add All Locations to Global List
                //
                allLocations = new List<LocationUI>();
                allLocations.Add(test_1);
                allLocations.Add(test_2);
                //
                // Default Active Location
                //
                activeLocation = test_1;

                //
                // Cursor
                //
                cursor = new MiAnimatingComponent(game, activeLocation.ButtonBase.Position.X -50, activeLocation.ButtonBase.Position.Y + 30, 33, 35);

                //
                // Responses to Input
                //
                inputResponses[Controller.UP] = Upped;
                inputResponses[Controller.DOWN] = Downed;
                inputResponses[Controller.LEFT] = Lefted;
                inputResponses[Controller.RIGHT] = Righted;
                inputResponses[Controller.A] = EnterLocation;
                inputResponses[Controller.START] = Escape;
            }
            else
            {
                throw new Exception("World Screen Already Initialized");
            }
        }

        public override void LoadContent()
        {
            background.AddTexture(Game.Content.Load<Texture2D>("World View\\Map"), 0);
            foreach (LocationUI locationUI in allLocations)
            {
                locationUI.LoadContent();
            }
            cursor.AddTexture(Game.Content.Load<Texture2D>("Main Menu\\pointer"), 0);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(gameTime);
            foreach (LocationUI locationUI in allLocations)
            {
                locationUI.ButtonBase.Draw(gameTime);
                Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>("Fonts\\Default"), locationUI.Label, new Vector2(locationUI.ButtonBase.Position.X, locationUI.ButtonBase.Position.Y + 20), Color.White);
            }
            cursor.Draw(gameTime);
        }

        public IEnumerator<ulong> Upped()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.UP))
            {
                activeLocation = activeLocation.Neighbors[Controller.UP];
                cursor.Position = new Point(activeLocation.ButtonBase.Position.X - 50, activeLocation.ButtonBase.Position.Y + 30);
            }
            yield break;
        }

        public IEnumerator<ulong> Downed()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.DOWN))
            {
                activeLocation = activeLocation.Neighbors[Controller.DOWN];
                cursor.Position = new Point(activeLocation.ButtonBase.Position.X - 50, activeLocation.ButtonBase.Position.Y + 30);
            }
            yield break;
        }

        public IEnumerator<ulong> Lefted()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.LEFT))
            {
                activeLocation = activeLocation.Neighbors[Controller.LEFT];
                cursor.Position = activeLocation.ButtonBase.Position;
            }
            yield break;
        }

        public IEnumerator<ulong> Righted()
        {
            if (activeLocation.Neighbors.ContainsKey(Controller.RIGHT))
            {
                activeLocation = activeLocation.Neighbors[Controller.RIGHT];
                cursor.Position = activeLocation.ButtonBase.Position;
            }
            yield break;
        }

        public IEnumerator<ulong> EnterLocation()
        {
            Game.RemoveAllScreens();
            switch (activeLocation.ControllingFaction)
            {
                case LocationData.State.ALLY:
                    TownScreen.Instance.Activate(activeLocation.LocationData);
                    break;
                case LocationData.State.ENEMY:
                    BattleScreen.Instance.Activate(activeLocation.LocationData);
                    break;
            }
            yield break;
        }

        public IEnumerator<ulong> Escape()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(StartScreen.Instance);
            yield break;
        }

        public static void Activate()
        {
            Instance.Game.RemoveAllScreens();
            Instance.Game.PushScreen(Instance);
        }
    }
}
