using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Location
    {
        public enum State { ALLY, ENEMY, NEUTRAL }
        public State ControllingFaction { set; get; }
        public string Name { set; get; }

        public Location(string name)
        {
            Name = name;
        }
    }
}
