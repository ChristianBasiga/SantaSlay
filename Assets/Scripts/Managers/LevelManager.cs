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
         PoolManager poolManager;

        
        //GOtta think of better ways to form delegates after this. I shouldn't be making one for every single event.
        //Though in this case there has been none with this signature of two arguments.
        public delegate void LevelProgressed(int progress, int goal);

        //Same name event in House, but different purpose and signature, prob confusing but fuck for now.
        public event LevelProgressed PassedHouse;

        //I want to reuse the one in reusable sincree same signature
        int numHouses;
        int passedHouses;
        House housePrefab;

        //All possible spawn points
        private List<Vector3> houseSpawnPoints;

        //Will pass in next level and current difficulty by calling callback as soon as current houses went through 0.
        //GUI manager needs to add it's own callback here to load the loading screen while next level getting set up.
        //Event for End of Level and time to load next.
        public event LevelChanged ReachedEndOfLevel;

        //This event for when Level is Loaded
        public event LevelChanged LoadedLevel;


        //At maximum would be 50%
        float naughtyChance;

        //Multiplier Variables
        //All public for testing purposes
        public float multiplierTime = 2.0f;
        public float timeLeftMultiplier = 0;




        // Use this for initialization
        void Start()
        {

     
            SantaController santa = GameObject.Find("Santa").GetComponent<SantaController>();

            housePrefab = ((GameObject)Resources.Load("Prefabs/House")).GetComponent<House>();
            //May actually just change to static oncstants as will start to get More hectic as more pools added.
            housePrefab.ReuseID = 2;
            poolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
            //No more than 5 seeing at a time
            poolManager.AddPool(housePrefab, 30);
            passedHouses = 0;
            numHouses = 0;

            santa.BirdHit += () => { timeLeftMultiplier = multiplierTime; };

        }

       

        public int NumberOfHouses
        {
            set
            {
                numHouses = value;
                passedHouses = 0;

                //For GUI
                PassedHouse(passedHouses,numHouses);

                spawnHouses();

                LoadedLevel();
            }

        }

        private void spawnHouses()
        {
          
            for (int i = 0; i < numHouses; ++i)
            {
                //In here will place position of house in array of positions
                Reusable house = poolManager.Acquire(housePrefab.ReuseID);

                //Maybe move event to Controller instead of What's supposed to be just data.

                
                //Need to create the house spawn points first.
               // house.gameObject.transform.position = houseSpawnPoints[i];

                //Randomly make nice or naughty, since won't depend on level anymore

                //so automatically incremented
                house.GetComponent<House>().PassedHouse += () => {

                    passedHouses += 1;


                    //For GUI
                    this.PassedHouse(passedHouses,numHouses);

                };
                house.gameObject.SetActive(true);
            }
        }


        public float NaughtyChance
        {
            set
            {
                naughtyChance = value;
            }
        }

        void Update()
        {
            //Here it will be constantly checking housesPassed to see of equal to numHouses
            if (passedHouses == numHouses)
            {
                if (ReachedEndOfLevel != null)
                    ReachedEndOfLevel();
            }

            if (timeLeftMultiplier > 0)
            {
                timeLeftMultiplier -= Time.deltaTime;
            }
        }

    }
}