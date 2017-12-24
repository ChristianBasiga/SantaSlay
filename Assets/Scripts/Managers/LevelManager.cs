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

        //I want to reuse the one in reusable sincree same signature
        int numHouses;
        int passedHouses;
        House housePrefab;

        //All possible spawn points
        private List<Vector3> houseSpawnPoints;

        //Will pass in next level and current difficulty by calling callback as soon as current houses went through 0.
        public event LevelChanged ReachedEndOfLevel;
        //At maximum would be 50%
        float naughtyChance;

        //Multiplier Variables
        //All public for testing purposes
        public float multiplierTime = 2.0f;
        public float timeLeftMultiplier = 0;

        SantaController santa;

        void Awake()
        {
            //ToDo here: Find all stuff with position Tag
            //And fill up the list of Vector 3s with those positions
            santa = GameObject.Find("Santa").GetComponent<SantaController>();
        }


    // Use this for initialization
        void Start() {

            passedHouses = 0;
            numHouses = 0;
            poolManager = GetComponent<PoolManager>();

            santa.BirdHit += () => { timeLeftMultiplier = multiplierTime; };
            housePrefab = ((GameObject)Resources.Load("Prefabs/House")).GetComponent<House>();
            //May actually just change to static oncstants as will start to get More hectic as more pools added.
            housePrefab.ReuseID = 2;
            //Could reuse notifier delegate had in MOdel, instead of making new one, but won't effect stuff in here
            housePrefab.AmmoHit += AddSantaPoints;
            //No more than 5 seeing at a time
            poolManager.AddPool(housePrefab, 30);
        }


        //Forward function for multiplier, so we can do more to points given before updating points
        private void AddSantaPoints(int points)
        {

            if (timeLeftMultiplier > 0)
            {

                //Just doubling for now
                points *= 2;
                Debug.Log("Hello");
            }
            Debug.Log("Points:" + points);

            //Then updates actual points;
            santa.santa.UpdatePoints(points);

        }

        public int NumberOfHouses
        {
            set
            {
                numHouses = value;
                passedHouses = 0;
                //Because only time this changes is when time to go to next level.

                //Actually better than iterating through all objects
                //woudl be to add callback to LevelChanged that has the House automatically brign itself back to pool, and I'll add when spawn it
                //So only loop needed is just respawning everything into place
                //Scratch, could just do it on TriggerExit, alot better
                spawnHouses();
            }

        }

        private void spawnHouses()
        {
            //If always make random requires nested loop, with list that gets bigger
            //The positions themselves don't need to be chosen at random. The house status is.
            //Can just change state randomly and place in each position
            
            //Sucks that search for santa everytime new level, but fuck it. I could make it local to class but hmm
            //Anyway their query should optomized to find it quick on unique identifiers like names. Should be.
            //So not really that slow, as in not iterating though ALL objects in scene to find Santa name, or shouldn't be
          
            for (int i = 0; i < numHouses; ++i)
            {
                //In here will place position of house in array of positions
                Reusable house = poolManager.Acquire(housePrefab.ReuseID);
                //Maybe move event to Controller instead of What's supposed to be just data.
                       
                house.GetComponent<House>().AmmoHit += (int points) => { santa.santa.UpdatePoints(points); };
                //Need to create the house spawn points first.
               // house.gameObject.transform.position = houseSpawnPoints[i];

                //Randomly make nice or naughty, since won't depend on level anymore

                //so automatically incremented
                house.GetComponent<House>().PassedHouse += () => { passedHouses += 1; };
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
                //GameManager Added callback to update numHouses and nice/naughty frequency.
                ReachedEndOfLevel();
            }
            Debug.Log("Passed Houses: " + passedHouses);
            //WIll be updated on house trigger, but for now just hit key for testing, works
            if (Input.GetKeyDown(KeyCode.L))
            {
                passedHouses += 1;
            }

            if (timeLeftMultiplier > 0)
            {
                timeLeftMultiplier -= Time.deltaTime;
            }
        }

    }
}