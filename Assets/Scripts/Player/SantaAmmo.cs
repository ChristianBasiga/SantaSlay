using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantaGame
{
    public class SantaAmmo : Reusable
    {

        public GameConstants.SantaAmmoType type;
        //Just speed really rn, if want coal that lasts longer will do later
        float speed;
        public event Notifier HitHouse;
       
       
        void Start()
        {
             

        }
        void Update()
        {


        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("House"))
            {

                House house = other.GetComponent<House>();
                if (HitHouse != null)

                    HitHouse(GameConstants.pointWorth[house.houseState][type]);


                BackToPool();

            }
            else if (other.CompareTag("Wreath"))
            {
                //Because those block the ammo
                BackToPool();
            }

        }
    }
}