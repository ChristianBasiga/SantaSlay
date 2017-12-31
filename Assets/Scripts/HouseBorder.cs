using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame
{
    public class HouseBorder : MonoBehaviour
    {

        House house;

        void Awake()
        {

            house = transform.parent.GetComponent<House>();
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                house.DidEnterHouse();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            //Then passed this house
            if (other.CompareTag("Player"))
            {

                house.DidPassHouse();
            }

        }
    }
}