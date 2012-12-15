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
        public int MaxMovementPoints { get { return 3 + (int)(speed * 0.0005); } }

        public int CurrMovementPoints { set; get; }

        private int will;
        public int Will { get { return will; } }

        private int mind;
        public int Mind { get { return mind; } }

        private int power;
        public int Power { get { return power; } }

        private int vitality;
        public int Vitality { get { return vitality; } }
        public int MaxHealth { get { return vitality * 10; } }

        public int CurrHealth { set; get; }

        private List<Attack> knownAttacks;
        public List<Attack> KnownAttacks { get { return knownAttacks; } }

        private string name;
        public string Name { get { return name; } }
        
        public Character(string name, int speed, int will, int mind, int power, int vitality)
        {
            this.name = name;
            this.speed = speed;
            this.will = will;
            this.mind = mind;
            this.power = power;
            this.vitality = vitality;

            CurrHealth = MaxHealth;
            CurrMovementPoints = MaxMovementPoints;
            knownAttacks = new List<Attack>();
        }

        public bool IsDead()
        {
            return CurrHealth <= 0;
        }
    }
}
