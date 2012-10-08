using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Attack
    {
        public static Attack shootGun = new Attack("Shoot Gun", 10);
        public static Attack swingSword = new Attack("Sword Swing", 15);

        private int traditionalBaseDamage;
        public int TraditionalBaseDamage { get { return traditionalBaseDamage; } }

        private int futuristBaseDamage;
        public int FuturistBaseDamage { get { return futuristBaseDamage; } }

        
        private Attack(String name, int baseDmg)
        {

        }
    }
}
