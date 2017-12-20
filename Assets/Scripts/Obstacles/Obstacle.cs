using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame {

    public class Obstacle : Reusable
    {

        //Don't need events for these since obstacle directly interacting with player unlike houses and ammo
        private float speedEffect;
        private int damage;

        public float SpeedEffect
        {
            get
            {
                return speedEffect;
            }
        }

        
        public int Damage
        {
            get
            {
                //Kinda messy here, but if dealt damage which si only time this is returned
                //Then dies and goes back to pool
                BackToPool();
                return damage;
            }
        }
    }
}