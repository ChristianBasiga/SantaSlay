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
            //Wait cause of closure, don't need to pass it in lol it holds reference to those vars, rather it should if know my shit.
            passedHouses = 0;
        }

        public int NumberOfHouses
        {
            set
            {
                numHouses = value;
                levelChanged();

            }

        }

        void Update()
        {

            //Here it will be constantly checking housesPassed to see of equal to numHouses
            if (passedHouses == numHouses)
            {

            }
        }

        public void PassedHouse()
        {
            //If passed house then increment. The houses will callback to this function on trigger exit of the player
           
        }

        //Todo here, is make sure pool back to full.
        //Decrease num houses spawned
        private void levelChanged()
        {

        }

        public void NextLevel()
        {
            currentLevel += 1;
            //This will update all the base states of obstacles, etc.
            levelChanged(currentLevel);
        }

    }
}