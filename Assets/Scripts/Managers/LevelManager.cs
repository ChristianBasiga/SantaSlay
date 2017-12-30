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
        //Though in this case there has been none with this signature of two arguments
        public delegate void LevelProgressed(int progress, int goal);

        #region Obstacle Variables

        GameObject[] snowFlakeSpawnPoints;
        Obstacle snowflakePrefab;

        Obstacle starPrefab;
        Obstacle wreethPrefab;


        #endregion

        #region House Related Varaibles
        //Same name event in House, but different purpose and signature, prob confusing but fuck for now.
        public event LevelProgressed PassedHouse;

        //I want to reuse the one in reusable sincree same signature
        int numHouses;
        int passedHouses;

        House housePrefab;

        //All possible spawn points
        private List<Vector3> houseSpawnPoints;

    #endregion

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


            //Wait, wait. I only need obstacles pool cause that's these all are. Specific instnaces means they can act as prototypes.
            //Simplicity sake, I'ma just have them ahve different pools, fuck it. Otherwise would need to copy everything, sprite, transform, etc. over
            //Not that need seperate pools for them


            //Cause all share same pool.
            snowflakePrefab = ((GameObject)Resources.Load("Prefabs/Snowflake")).GetComponent<Obstacle>();
            snowflakePrefab.ReuseID = 3;
            starPrefab = ((GameObject)Resources.Load("Prefabs/Star")).GetComponent<Obstacle>();
            starPrefab.ReuseID = 4;
            wreethPrefab = ((GameObject)Resources.Load("Prefabs/Wreeth")).GetComponent<Obstacle>();
            wreethPrefab.ReuseID = 5;

            
            
            poolManager.AddPool(snowflakePrefab, 5);
            //Very rare for there to be more than 1 star, tbh this is waste of pool for this, might as well just instantiate it
            //cause it's not as bad as houses, and snowflakes etc. But will leave as is for now, just incase
            poolManager.AddPool(starPrefab, 2);

            poolManager.AddPool(wreethPrefab, 5);

            santa.BirdHit += () => { timeLeftMultiplier = multiplierTime; };

        }


       
        void SpawnStar()
        {
            //ToDo: spawn store, and place in correct position
            //To make "correct position" easier I could do initial plan of just having moving backgrounds
            //instead of actually moving santa outside of up and down. and that would mean
            //getting rid of speed factor(well can still use for up/down movement though) but yeah would make things
            //ALOT easier of level itself was just translating. Also then wouldn't need to have position var for star, cause could just leave it at prefabs position meaning this:
            //This is literally All I need to do, if I did that.
            Reusable star = poolManager.Acquire(starPrefab.ReuseID);
            star.gameObject.SetActive(true);
           
        }

        //Exactly the same situation with this, just start off screen then come in, holy fuck okay number one priority is get that working
        void SpawnWreeths(int n)
        {
            Reusable wreeth = poolManager.Acquire(wreethPrefab.ReuseID);
            wreeth.gameObject.SetActive(true);
            //Difference is there may be alot of them, so could make this Ienumerator so could add delay.
        }

        void SpawnSnowFlakes(int n)
        {
            //Need spawn points for snowflakes
            for (int i = 0; i < snowFlakeSpawnPoints.Length; ++i)
            {
                Reusable snowflake = poolManager.Acquire(snowflakePrefab.ReuseID);

                snowflake.gameObject.transform.position = snowFlakeSpawnPoints[i].transform.position;
                snowflake.gameObject.SetActive(true);
            }

        }

        #region House related Methods
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

        #endregion

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