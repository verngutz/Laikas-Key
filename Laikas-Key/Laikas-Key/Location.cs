using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Location
    {
        public static Location TEST_1 = new Location("Prologion");
        public static Location TEST_2 = new Location("Eudaimon");

        public enum State { ALLY, ENEMY, NEUTRAL }
        public State ControllingFaction { set; get; }
        public string Name { set; get; }

        public Location(string name)
        {
            Name = name;
        }
    }
}
