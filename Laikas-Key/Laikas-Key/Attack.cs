using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Attack
    {
        int tradBase;
        int futurBase;

        public static Attack shootGun = new Attack("Shoot Gun", 10);
        public static Attack swingSword = new Attack("Sword Swing", 15);
        
        private Attack(String name, int baseDmg)
        {

        }    

        public int TradBase
        {
            get { return tradBase; }
        }

        public int FuturBase
        {
            get { return futurBase; }
        }
    }
}
