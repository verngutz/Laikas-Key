using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laikas_Key
{
    public class Character
    {
        
        private int hp;
        private int movement;
        private int attack;
        private int tradDamage;
        private int futurDamage;
        int traditionalist;
        int futurist;
        int damage;
        
        
        public Character(int health, int motion, int atk, int tradDmg, int futurDmg)
        {
            hp = health;
            movement = motion;
            attack = atk;
            //tradDamage = tradDmg;
            //futurDamage = futurDmg;
        }

        public int Damage()
        {
            double pTraditionalist = traditionalist * .01;
            double pFuturist = futurist *.01;
            damage = (int)((tradDamage * pTraditionalist) + (futurDamage * pFuturist)) * attack;
            return damage;
        }
        
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }

        public int Movement
        {
            get { return movement; }
            set { movement = value; }
        }

        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public int TradDamage
        {
            get { return tradDamage; }
            set { tradDamage = value; }
        }

        public int FuturDamage
        {
            get { return futurDamage; }
            set { futurDamage = value; }
        }
    }
}
