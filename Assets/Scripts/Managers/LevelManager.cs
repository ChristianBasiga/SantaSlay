using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle moving the backgrounds and foregrounds for Parallex effect and to simulate the player moving
/// </summary>
namespace SantaGame {

    //There will be multiple instances of this object made everytime make new level by GameManger
    public class LevelManager : MonoBehaviour {

        public Transform background;
        public Transform foreground;
        //I want to reuse the one in reusable sincree same signature
        int numHouses;
        int passedHouses;


        //Will pass in next level and current difficulty by calling callback as soon as current houses went through 0.
        public event LevelChanged ReachedEndOfLevel;
        //At maximum would be 50%
        float naughtyChance;
    // Use this for initialization
        void Start() {
            passedHouses = 0;
        }

        public int NumberOfHouses
        {
            set
            {
                
                numHouses = value;
                passedHouses = 0;
                //Because only time this changes is when this is et
            
            }

        }

        void Update()
        {
            //Here it will be constantly checking housesPassed to see of equal to numHouses
            if (passedHouses == numHouses)
            {
                Debug.Log(numHouses);
                //GameManager Added callback to update numHouses and nice/naughty frequency.
                ReachedEndOfLevel();
            }
            Debug.Log("HOuses passed: " + passedHouses);
            //WIll be updated on house trigger, but for now just hit key for testing
            if (Input.GetKeyDown(KeyCode.L))
            {
                passedHouses += 1;
            }
        }

        public void PassedHouse()
        {
            //If passed house then increment. The houses will callback to this function on trigger exit of the player
           
        }

      


    }
}