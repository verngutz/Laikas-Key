using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    class Character
    {
        private int speed;
        public int Speed { get { return speed; } }
        public int MovementPoints { get { return speed; } }

        private int will;
        public int Will { get { return will; } }

        private int mind;
        public int Mind { get { return mind; } }

        private int power;
        public int Power { get { return power; } }

        private int vitality;
        public int Vitality { get { return vitality; } }
        public int MaxHealth { get { return vitality; } }

        private int currHealth;
        public int CurrHealth { get { return currHealth; } }

        private List<Attack> knownAttacks;
        public List<Attack> KnownAttacks { get { return knownAttacks; } }
        
        public Character(int speed, int will, int mind, int power, int vitality)
        {
            this.speed = speed;
            this.will = will;
            this.mind = mind;
            this.power = power;
            this.vitality = vitality;

            currHealth = MaxHealth;
            knownAttacks = new List<Attack>();
        }

        public bool IsDead()
        {
            return currHealth <= 0;
        }
    }
}
