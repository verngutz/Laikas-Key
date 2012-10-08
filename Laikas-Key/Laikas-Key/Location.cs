using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Location
    {
        public static Location TEST_1 = new Location("test_1");
        public static Location TEST_2 = new Location("test_2");
        public static Location TEST_3 = new Location("test_3");
        public static Location TEST_4 = new Location("test_4");
        public static Location TEST_5 = new Location("test_5");

        public enum State { ALLY, ENEMY, NEUTRAL }
        public State ControllingFaction { set; get; }
        public string Name { set; get; }

        public Location(string name)
        {
            Name = name;
        }
    }
}
