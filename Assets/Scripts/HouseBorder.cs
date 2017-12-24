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

        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("yoyo");
            //Then passed this house
            if (other.CompareTag("Player"))
            {
                Debug.Log("dfgdg");
                house.DidPassHouse();
            }

        }
    }
}