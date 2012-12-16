using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    static class EnemyGenerator
    {
        public static List<Character> Generate()
        {
            Character grunt1 = new Character("Grunt 1", 5, 5, 5, 5, 5);
            grunt1.KnownAttacks.Add(Attack.shootGun);
            grunt1.KnownAttacks.Add(Attack.swingSword);
            Character grunt2 = new Character("Grunt 2", 5, 5, 5, 5, 5);
            grunt2.KnownAttacks.Add(Attack.shootGun);
            grunt2.KnownAttacks.Add(Attack.swingSword);
            Character grunt3 = new Character("Grunt 3", 5, 5, 5, 5, 5);
            grunt3.KnownAttacks.Add(Attack.shootGun);
            grunt3.KnownAttacks.Add(Attack.swingSword);
            return new List<Character>() { grunt1, grunt2, grunt3 };
        }
    }
}
