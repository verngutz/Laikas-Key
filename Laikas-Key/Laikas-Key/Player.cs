using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    static class Player
    {
        private static int money;
        private static List<Character> party = new List<Character>();
        public static List<Character> Party { get { return party; } }
    }
}
