using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame
{

    public class House : Reusable
    {
        
        public event Notifier AmmoHit;

        public GameConstants.HouseState houseState;


        void Awake()
        {
        }

        void Start()
        {
            
        }

        
        //If this doesn't happen on same time as Ammo before it disappears, then I'll just have SantoAmmo do all this
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ammo"))
            {
                SantaAmmo ammoType = other.GetComponent<SantaAmmo>();
               
                AmmoHit(GameConstants.pointWorth[houseState][ammoType.type]);
            }

        }
    }
}