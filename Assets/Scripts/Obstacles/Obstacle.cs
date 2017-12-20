using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame {

    public class Obstacle : Reusable
    {

        //Don't need events for these since obstacle directly interacting with player unlike houses and ammo
        public float speedEffect;
        public int damage;
    }
}