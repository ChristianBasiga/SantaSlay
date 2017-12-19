using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame
{

    public class House : Reusable
    {
        
        public event Notifier AmmoHit;

        private GameConstants.HouseState houseState;

        void Start()
        {
            int rand = Random.Range(5, 25);

            if (rand % 2 == 0){
                houseState = GameConstants.HouseState.NAUGHTY;
            }
            else
            {
                houseState = GameConstants.HouseState.NICE;
            }
        }

        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ammo"))
            {
                GameConstants.SantaAmmoType ammoType = GetComponent<GameConstants.SantaAmmoType>();

                AmmoHit(GameConstants.pointWorth[houseState][ammoType]);
            }

        }
    }
}