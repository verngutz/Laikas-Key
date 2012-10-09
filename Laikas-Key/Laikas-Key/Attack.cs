using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Attack
    {
        public static Attack shootGun = new Attack("Shoot Gun", 0, 3, 4, 1, 0);
        public static Attack swingSword = new Attack("Sword Swing", 3, 0, 1, 2, 1);

        private string name;
        public string Name { get { return name; } }

        private int traditionalBaseDamage;
        public int TraditionalBaseDamage { get { return traditionalBaseDamage; } }

        private int futuristBaseDamage;
        public int FuturistBaseDamage { get { return futuristBaseDamage; } }

        private int range;
        public int Range { get { return range; } }

        private int movementCost;
        public int MovementCost { get { return movementCost; } }

        private int aoe;
        public int AOE { get { return aoe; } }
        
        private Attack(String name, int traditionalBaseDamage, int futuristBaseDamage, int range, int movementCost, int aoe)
        {
            this.name = name;
            this.traditionalBaseDamage = traditionalBaseDamage;
            this.futuristBaseDamage = futuristBaseDamage;
            this.range = range;
            this.movementCost = movementCost;
            this.aoe = aoe;
        }
    }
}
