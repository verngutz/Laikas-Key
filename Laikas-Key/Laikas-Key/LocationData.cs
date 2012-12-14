using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
using Choice = System.Collections.Generic.KeyValuePair<string, MiUtil.MiScript>;
using Microsoft.Xna.Framework;
namespace Laikas_Key
{
    class LocationData
    {
        public static void Init()
        {
            #region TEST_1
            TEST_1 = new LocationData();
            TEST_1.name = "Proslogion";
            TEST_1.map = new char[,]
            {
                {'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h'},
                {'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h'},
                {'h', 'h', 'h', 'h', 'r', 'h', 'h', 'r', 'h', 'r', 'r', 'h'},
                {'h', 'h', 'r', 'r', 'r', 'h', 'h', 'r', 'h', 'h', 'r', 'h'},
                {'h', 'r', 'r', 'h', 'h', 'h', 'h', 'r', 'h', 'h', 'r', 'h'},
                {'h', 'h', 'r', 'r', 'r', 'h', 'h', 'r', 'h', 'h', 'h', 'h'},
                {'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h', 'h'},
            };
            #endregion

            #region TEST_2
            TEST_2 = new LocationData();
            TEST_2.name = "Eudaimon";
            TEST_2.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'w', 'r'},
                {'r', 'g', 'r', 'r', 'g', 'g', 'r'},
                {'r', 'g', 'g', 't', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'g', 'q', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            TEST_2.townEntryX = 1;
            TEST_2.townEntryY = 1;
            TEST_2.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            TEST_2.events[new Point(3, 3)] =
                delegate
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
                    return null;
                };
            TEST_2.events[new Point(5, 1)] =
                delegate
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
                    return null;
                };
            TEST_2.events[new Point(5, 5)] =
                delegate
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
                    return null;
                };
            #endregion
        }

        public static LocationData TEST_1;
        public static LocationData TEST_2;

        public enum State { ALLY, ENEMY, NEUTRAL }
        public State ControllingFaction { set; get; }

        private string name;
        public string Name { get { return name; } }

        private int townEntryX;
        public int TownEntryX { get { return townEntryX; } }

        private int townEntryY;
        public int TownEntryY { get { return townEntryY; } }

        private TownScreen.AvatarDirection townEntryDirection;
        public TownScreen.AvatarDirection TownEntryDirection { get { return townEntryDirection; } }

        private char[,] map;
        public char[,] Map { get { return map; } }

        private Dictionary<Point, MiScript> events;
        public Dictionary<Point, MiScript> Events { get { return events; } }

        private LocationData() 
        {
            events = new Dictionary<Point, MiScript>();
        }
    }
}
