using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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
        private SoundEffect arrow;
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
                LocationUI test_1 = new LocationUI(game, 150, 150, 64, 64, LocationData.TEST_1);
                LocationUI test_2 = new LocationUI(game, 150, 350, 64, 64, LocationData.courtyard);
                LocationUI test_3 = new LocationUI(game, 150, 550, 64, 64, LocationData.cafe);
                LocationUI test_4 = new LocationUI(game, 350, 150, 64, 64, LocationData.Erty);
                LocationUI test_5 = new LocationUI(game, 350, 350, 64, 64, LocationData.hall);
                LocationUI test_6 = new LocationUI(game, 350, 550, 64, 64, LocationData.HOME);
                LocationUI test_7 = new LocationUI(game, 550, 150, 64, 64, LocationData.mindclass);
                LocationUI test_8 = new LocationUI(game, 550, 350, 64, 64, LocationData.weaponclass);
                LocationUI test_9 = new LocationUI(game, 550, 550, 64, 64, LocationData.willclass);
                //LocationUI test_3 = new LocationUI(game, 650, 250, 128, 128, LocationData.TEST_3);
                LocationData.TEST_1.ControllingFaction = LocationData.State.ENEMY;

                //
                // Add Neighbors
                //
                test_1.Neighbors.Add(Controller.DOWN, test_2);
                test_1.Neighbors.Add(Controller.RIGHT, test_4);

                test_2.Neighbors.Add(Controller.UP, test_1);
                test_2.Neighbors.Add(Controller.DOWN, test_3);
                test_2.Neighbors.Add(Controller.RIGHT, test_5);

                test_3.Neighbors.Add(Controller.UP, test_2);
                test_3.Neighbors.Add(Controller.DOWN, test_4);
                test_3.Neighbors.Add(Controller.RIGHT, test_6);

                test_4.Neighbors.Add(Controller.UP, test_3);
                test_4.Neighbors.Add(Controller.DOWN, test_5);
                test_4.Neighbors.Add(Controller.RIGHT, test_7);

                test_5.Neighbors.Add(Controller.UP, test_4);
                test_5.Neighbors.Add(Controller.DOWN, test_6);
                test_5.Neighbors.Add(Controller.RIGHT, test_8);

                test_6.Neighbors.Add(Controller.UP, test_5);
                test_6.Neighbors.Add(Controller.DOWN, test_7);
                test_6.Neighbors.Add(Controller.RIGHT, test_9);

                test_7.Neighbors.Add(Controller.UP, test_6);
                test_7.Neighbors.Add(Controller.DOWN, test_8);
                test_7.Neighbors.Add(Controller.RIGHT, test_1);

                test_8.Neighbors.Add(Controller.UP, test_7);
                test_8.Neighbors.Add(Controller.DOWN, test_9);
                test_8.Neighbors.Add(Controller.RIGHT, test_2);

                test_9.Neighbors.Add(Controller.UP, test_8);
                test_9.Neighbors.Add(Controller.DOWN, test_1);
                test_9.Neighbors.Add(Controller.RIGHT, test_3);

                //
                // Add All Locations to Global List
                //
                allLocations = new List<LocationUI>();
                allLocations.Add(test_1);
                allLocations.Add(test_2);
                allLocations.Add(test_3);
                allLocations.Add(test_4);
                allLocations.Add(test_5);
                allLocations.Add(test_6);
                allLocations.Add(test_7);
                allLocations.Add(test_8);
                allLocations.Add(test_9);

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

        public void Activate()
        {
            Game.RemoveAllScreens();
            Game.PushScreen(Instance);
        }

        public override void LoadContent()
        {
            background.AddTexture(Game.Content.Load<Texture2D>("World View\\Map"), 0);
            foreach (LocationUI locationUI in allLocations)
            {
                locationUI.LoadContent();
            }
            cursor.AddTexture(Game.Content.Load<Texture2D>("Main Menu\\pointer"), 0);
            arrow = Game.Content.Load<SoundEffect>("Sounds\\crossbow");
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

        public IEnumerator<ulong> Upped() { return MoveCursor(Controller.UP); }
        public IEnumerator<ulong> Downed() { return MoveCursor(Controller.DOWN); }
        public IEnumerator<ulong> Lefted() { return MoveCursor(Controller.LEFT); }
        public IEnumerator<ulong> Righted() { return MoveCursor(Controller.RIGHT); }

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
            StartScreen.Instance.Activate();
            yield break;
        }

        private IEnumerator<ulong> MoveCursor(MiControl dir)
        {
            if (activeLocation.Neighbors.ContainsKey(dir))
            {
                activeLocation = activeLocation.Neighbors[dir];
                cursor.Position = new Point(activeLocation.ButtonBase.Position.X - 50, activeLocation.ButtonBase.Position.Y + 30);
                arrow.Play();
            }
            yield break;
        }
    }
}
