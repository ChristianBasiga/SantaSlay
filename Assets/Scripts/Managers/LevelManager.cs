using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle moving the backgrounds and foregrounds for Parallex effect and to simulate the player moving
/// </summary>
namespace SantaGame {

    //There will be multiple instances of this object made everytime make new level by GameManger
    public class LevelManager : MonoBehaviour {



        public Transform boundary;
        private Transform background;
        PoolManager poolManager;

        
        //GOtta think of better ways to form delegates after this. I shouldn't be making one for every single event.
        //Though in this case there has been none with this signature of two arguments
        public delegate void LevelProgressed(int progress, int goal);

        #region Obstacle Variables

        //Will just be along edge of top boundary, a spot within that. That's a simple equation, no need for this.
        //GameObject[] snowFlakeSpawnPoints;
        Obstacle snowflakePrefab;

        Obstacle starPrefab;
        
        //his is needed cause will be among houses, or will it? I could just retrieve all houses currently spawned then get ref to child
        //for spawning tbh.
        //GameObject[] wreethSpawnPoints;
        Obstacle wreethPrefab;


        #endregion


        #region House Related Varaibles
        //Same name event in House, but different purpose and signature, prob confusing but fuck for now.
        public event LevelProgressed PassedHouse;

        //I want to reuse the one in reusable sincree same signature
        int numHouses;
        //For GUI and for snowflake spawning
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


        public Transform Background
        {
            set
            {
                background = value;
                LoadedLevel();
            }
        }


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
            
            santa.GotStar += () => { timeLeftMultiplier = multiplierTime; };
            ReachedEndOfLevel();
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
            Vector3 starSpawnPoint = new Vector3();

            starSpawnPoint.y = boundary.position.y + boundary.localScale.y / 2;

            starSpawnPoint.x = boundary.position.x + boundary.localScale.x / 2;

            star.transform.position = starSpawnPoint;

            star.gameObject.SetActive(true);
           
        }

        //Exactly the same situation with this, just start off screen then come in, holy fuck okay number one priority is get that working
        void SpawnWreeths(int n)
        {
            //This one might actually be array of spawn points, reason is because they could come out of naughty houses to block coal or stop other people
            //from getting presents, so will dealw ith these spawns later. later being fucking today cause holy shit this should already be done, just been
            //lazy.

            //Scatter wreeths across all spawned houses that are naughty
            //Then they will be set active when enter house boundary of it
            GameObject[] spawnedHouses = GameObject.FindGameObjectsWithTag("House");
            int wreethsSpawned = 0;
            for (int i = 0; i < spawnedHouses.Length && wreethsSpawned < n; ++i)
            {

                House house = spawnedHouses[i].GetComponent<House>();

                if (house.houseState == GameConstants.HouseState.NAUGHTY)
                {
                    //Could make it just 50% chance naughty house has it or not, tbh, just to avoid complexity for now.
                    //Cause also determining naughty later. So it would add layer of complexity of determine how many wreaths out of how many naughty
                    //too. TODO: determine equation for determining chance

                    int rn = Random.Range(1, 101);

                    if (rn >= 50) continue;
                    

                    house.EnteredHouseBorder += () =>
                    {

                        Reusable wreeth = poolManager.Acquire(wreethPrefab.ReuseID);

                    //Getting wreeth spawn point and making wreeth be at that position
                        wreeth.gameObject.transform.position = house.gameObject.transform.GetChild(1).position;
                        wreeth.gameObject.SetActive(true);
                    };
                }
            }

          
            //Difference is there may be alot of them, so could make this Ienumerator so could add delay.
        }

        IEnumerator SpawnSnowFlakes()
        {
            float y = boundary.position.y + boundary.localScale.y / 2;

            //Could just spawn random up to max of some equation I'll come up with later
            //And then just let them fall slow, that way some flakes may start at top, but some may also already be in middle or something
            //by time Santa in. I would prefer to make it everytime enters new view, but I made it smoother transition to rest of level so can't do it like that
            //Well I still can, so anytime santa reaches boundary and they shift forward then, time to randomly spawn snowflakes again
            //Still need to swap it with Ienumerator that moves certain amount.

            //Edit: Finished this. Here what I could do. Make this a Coroutine and wait for random amount of seconds and spawn small interval of snowflakes
            //every iteration of loop and condition for loop will be while housesPassed < numHouses. Cause then no more reason for SnowFlakes
            //Yeah that should work.

            while (passedHouses < numHouses)
            {

                Reusable snowflake = poolManager.Acquire(snowflakePrefab.ReuseID);

                float minX = (boundary.position.x - boundary.localScale.x / 2) + snowflake.gameObject.transform.localScale.x;
                float maxX = (boundary.position.x + boundary.localScale.x / 2) - snowflake.gameObject.transform.localScale.x;

                snowflake.gameObject.transform.position = new Vector3(Random.Range(minX, maxX), y - snowflake.gameObject.transform.localScale.y, 0);
                snowflake.gameObject.SetActive(true);

                //wait till pass a house? fuck hmmm. or really just time lol, I won't have the random amount cause if 1 second apart
                //then they will spread fine and chance that they'll spawn in same position, so multiple within same frame
                //wasn't best idea in hindsight so this is better choice
                float waitTime = Random.Range(1.0f, 5.0f);

                yield return WaitForSeconds(waitTime);
            }
        }

        #region House related Methods
        public int NumberOfHouses
        {
            set
            {
                //Plus 1 cause will increment oncce hit boundary
                numHouses = value + 1;
                passedHouses = 0;

                //For GUI
                PassedHouse(passedHouses,numHouses);
                
                spawnHouses();
                SpawnWreeths(1);

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
            if (background == null)
            {
                Debug.Log("nuttin");
            }
            //Tbh, could just reach end of level when do this, but didn't want to remove it otherwise passedHoues only for GUI
            if (background != null && boundary.position.x - boundary.localScale.x / 2 >= background.position.x + boundary.localScale.x / 2)
            {
                if (ReachedEndOfLevel != null)
                {
                    ReachedEndOfLevel();
                    //GameManager will set the background in a callback attached to ReachedEndOfLevel event
                    background = null;
                }
            }

            if (timeLeftMultiplier > 0)
            {
                timeLeftMultiplier -= Time.deltaTime;
            }
        }

    }
}