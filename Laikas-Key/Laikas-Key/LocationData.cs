using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiUtil;
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
            TEST_2.events[new Point(3, 3)] = Scripts.ChooseYourFate;
            TEST_2.events[new Point(5, 1)] = Scripts.AboutTheWar;
            TEST_2.events[new Point(5, 5)] = Scripts.FightOrFlee;
            #endregion

            #region TEST_3
            TEST_3 = new LocationData();
            TEST_3.name = "Town 3";
            TEST_3.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'w', 'r'},
                {'r', 'g', 'r', 'r', 'g', 'g', 'r'},
                {'r', 'g', 'g', 't', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'g', 'g', 'r'},
                {'g', 'g', 'r', 'g', 'g', 'q', 'r'},
                {'g', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            TEST_3.townEntryX = 0;
            TEST_3.townEntryY = 7;
            TEST_3.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            //TEST_3.events[new Point(3, 3)] = Scripts.ChooseYourFate;
            //TEST_3.events[new Point(5, 1)] = Scripts.AboutTheWar;
            //TEST_3.events[new Point(5, 5)] = Scripts.FightOrFlee;
            #endregion

            #region TEST_4
            TEST_4 = new LocationData();
            TEST_4.name = "Town 4";
            TEST_4.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'w', 'r'},
                {'r', 'g', 'r', 'r', 'g', 'g', 'r'},
                {'r', 'g', 'g', 't', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'g'},
                {'r', 'r', 'r', 'r', 'r', 'q', 'g'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'g'}
            };
            TEST_3.townEntryX = 7;
            TEST_3.townEntryY = 7;
            TEST_3.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            //TEST_3.events[new Point(3, 3)] = Scripts.ChooseYourFate;
            //TEST_3.events[new Point(5, 1)] = Scripts.AboutTheWar;
            //TEST_3.events[new Point(5, 5)] = Scripts.FightOrFlee;
            #endregion

            #region TEST_5
            TEST_5 = new LocationData();
            TEST_5.name = "Town 5";
            TEST_5.map = new char[,]
            {
                {'g', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'g', 'g', 'g', 'g', 'g', 'w', 'r'},
                {'r', 'g', 'r', 'r', 'g', 'g', 'r'},
                {'r', 'g', 'g', 't', 'r', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'r', 'g', 'r'},
                {'r', 'r', 'r', 'g', 'g', 'q', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            TEST_5.townEntryX = 0;
            TEST_5.townEntryY = 0;
            TEST_5.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            //TEST_3.events[new Point(3, 3)] = Scripts.ChooseYourFate;
            //TEST_3.events[new Point(5, 1)] = Scripts.AboutTheWar;
            //TEST_3.events[new Point(5, 5)] = Scripts.FightOrFlee;
            #endregion

            #region TEST_6
            TEST_6 = new LocationData();
            TEST_6.name = "Town 6";
            TEST_6.map = new char[,]
            {
                {'r', 'r', 'r', 'g', 'r', 'r', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'r', 'g', 'r'},
                {'r', 'g', 'w', 'g', 'r', 'g', 'r'},
                {'r', 'r', 'r', 't', 'r', 'q', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            TEST_6.townEntryX = 4;
            TEST_6.townEntryY = 0;
            TEST_6.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            //TEST_3.events[new Point(3, 3)] = Scripts.ChooseYourFate;
            //TEST_3.events[new Point(5, 1)] = Scripts.AboutTheWar;
            //TEST_3.events[new Point(5, 5)] = Scripts.FightOrFlee;
            #endregion







            #region HOME
            HOME = new LocationData();
            HOME.name = "My Home";
            HOME.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'w', 'r'},
                {'r', 'r', 'r', 'r', 'g', 'g', 'r'},
                {'r', 'r', 't', 'g', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            HOME.townEntryX = 1;
            HOME.townEntryY = 7;
            HOME.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            HOME.events[new Point(2, 3)] = Scripts.SchneiderMail;
            HOME.events[new Point(5, 1)] = Scripts.AndresMail;
            HOME.events[new Point(5, 7)] = Scripts.NextMap;
            
            #endregion

            #region cafe
            cafe = new LocationData();
            cafe.name = "Academe Cafe";
            cafe.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 't', 'w', 'g', 'r'},
                {'r', 'r', 'g', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'g', 'r', 'r', 'r'},
                {'r', 'g', 'g', 'g', 'r', 'r', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            cafe.townEntryX = 1;
            cafe.townEntryY = 7;
            cafe.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            cafe.events[new Point(3, 2)] = Scripts.FriendlyConversation;
            cafe.events[new Point(5, 7)] = Scripts.NextMap;
            #endregion

            #region hall
            hall = new LocationData();
            hall.name = "Academe Hallway";
            hall.map = new char[,]
            {
                {'r', 'r', 'r'},
                {'r', 'g', 'q'},
                {'r', 'g', 'r'},
                {'r', 'g', 'r'},
                {'r', 'g', 'w'},
                {'r', 'g', 'r'},
                {'r', 'g', 'r'},
                {'r', 'g', 't'},
                {'r', 'r', 'r'}
            };
            hall.townEntryX = 1;
            hall.townEntryY = 1;
            hall.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            hall.events[new Point(1, 7)] = Scripts.NextMap;

            #endregion

            #region weaponclass
            weaponclass = new LocationData();
            weaponclass.name = "Weapons Class";
            weaponclass.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 't', 'w', 'q', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            weaponclass.townEntryX = 1;
            weaponclass.townEntryY = 7;
            weaponclass.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            weaponclass.events[new Point(5, 7)] = Scripts.NextMap;
            #endregion

            #region willclass
            willclass = new LocationData();
            willclass.name = "Will Class";
            willclass.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 't', 'w', 'q', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            willclass.townEntryX = 1;
            willclass.townEntryY = 7;
            willclass.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            willclass.events[new Point(5, 7)] = Scripts.NextMap;

            #endregion

            #region mindclass
            mindclass = new LocationData();
            mindclass.name = "Mind Class";
            mindclass.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 't', 'w', 'q', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            mindclass.townEntryX = 1;
            mindclass.townEntryY = 7;
            mindclass.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            mindclass.events[new Point(5, 7)] = Scripts.NextMap;

            #endregion

            #region courtyard
            courtyard = new LocationData();
            courtyard.name = "Academe Courtyard";
            courtyard.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 'r', 'r', 'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 'r', 'r', 'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'r', 'r', 'g', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'g', 'g', 'w', 'q', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r', 'r'},
            };
            courtyard.townEntryX = 1;
            courtyard.townEntryY = 7;
            courtyard.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            courtyard.events[new Point(8,5)] = Scripts.CourtYard;
            courtyard.events[new Point(5, 7)] = Scripts.NextMap;

            #endregion

            #region Erty
            Erty = new LocationData();
            Erty.name = "Erty Pub";
            Erty.map = new char[,]
            {
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'},
                {'r', 'r', 't', 'w', 'q', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'r', 'r', 'r', 'g', 'r'},
                {'r', 'g', 'g', 'g', 'g', 'g', 'r'},
                {'r', 'r', 'r', 'r', 'r', 'r', 'r'}
            };
            Erty.townEntryX = 1;
            Erty.townEntryY = 7;
            Erty.townEntryDirection = TownScreen.AvatarDirection.DOWN;
            Erty.events[new Point(3,1)] = Scripts.PubShootOut;
            Erty.events[new Point(5, 7)] = Scripts.NextMap;
            #endregion
        }

        public static LocationData Erty;
        public static LocationData courtyard;
        public static LocationData weaponclass;
        public static LocationData willclass;
        public static LocationData mindclass;
        public static LocationData cafe;
        public static LocationData HOME;
        public static LocationData hall;
        public static LocationData TEST_1;
        public static LocationData TEST_2;
        public static LocationData TEST_3;
        public static LocationData TEST_4;
        public static LocationData TEST_5;
        public static LocationData TEST_6;

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
