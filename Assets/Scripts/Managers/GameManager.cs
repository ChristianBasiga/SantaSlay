using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantaGame
{

    public class GameManager : MonoBehaviour
    {

        //GameManager will have a SantaController reference.
        //Will handle calling to spawn Houses
        //Will handle calling to spawn Obstacles
        // Use this for initialization

        
        SantaController santa;

        PoolManager poolManager;

        LevelManager levelManager;

        //Make inner class strictly for the lambda to have closure with reference to level and all that.
      
        int level;
        float difficulty;
        //Wait since all one image, I can't have this difference. Hmm. Fuck. I'll talk to Kris bout this.
        //FUck this for now, it works now to get it spawning, wait spawning is not a fucking thing cause it's all drawn out
        //Okay, no I can make this work. Instead of spawning house prefab with picture, it'll have no render and just spawn empty game Object with collider and HOuse script on it
        //And that way I determine the spawn points by position. Okay this will still work
        Dictionary<int, float> NaughtyChances = new Dictionary<int, float>()
        {
            {1 , 10.0f },
            {2 , 25.0f },
            {3 , 60.0f },
        };

        public SantaAmmo ammoPrefab;
        public Transform boundary;

        public Obstacle birdPrefab;
        public Obstacle planePrefab;


        void Awake()
        {

          
            santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();
            poolManager = GetComponent<PoolManager>();

            levelManager = GetComponent<LevelManager>();
            

            ammoPrefab = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString()))).GetComponent<SantaAmmo>();
            ammoPrefab.ReuseID = 1;
            InitAmmoPool();

            //Could prob do neater, but at this point just get set up, nly change to make is make enum for diff kinda, but eh. Not needed and at that point mightaswell just
            //not have the derivations but need it for different updates and added functionality of Bird with multiplier, but we'll see. I'll put more thought into this later
            /* //Want more just done at this point so can start asking someone for art part.
             birdPrefab = ((GameObject)Resources.Load("Prefabs/Obstacles/Bird")).GetComponent<Obstacle>();
             birdPrefab.ReuseID = 3;
             poolManager.AddPool(birdPrefab, 4);

             planePrefab = ((GameObject)Resources.Load("Prefabs/Obstacles/Plane")).GetComponent<Obstacle>();
             poolManager.AddPool(planePrefab, 3);
             birdPrefab.ReuseID = 4;
             #endregion*/
        }




        void Start()
        {
            level = 1;
            difficulty = 1.0f;



            santa.Width = boundary.localScale.x / 2;
            santa.Height = boundary.localScale.y / 2;

            santa.santa.healthUpdated += (int newHealth) =>
            {
                if (newHealth <= 0)
                {
                    GameOver();
                }
            };


            

            levelManager.ReachedEndOfLevel += () => {


                //Rightt, cause primitives are value not reference types so always the same so closure doesn't apply here, unless make small inner class to hold these values lol.
                this.levelManager.NumberOfHouses = (int)(((difficulty / 2) * (2.0f * level + 7)) + 1);
                level += 1;
            };




            levelManager.ReachedEndOfLevel += () =>
            {

                //The number of houses spawned was perfect in terms of deriving an equation for it 
                //But chance of Naughty, it's hardest at uniform distribution caues never know
                //And I can't think of an equation that would go up, down, and finish at center. Unless maybe sine wave but not really
                //Easiest, in terms of just getting done cause people won't care bout this detail, just  adictionary might suffice for this
                //and in that case


            };


           
        }

        //TO follow suit of LevelManager, I could even have SantaControler have PoolManager
        //Reference
        private void InitAmmoPool()
        {

          
            //Only one pool for ammo, will use prototypes to switch between
            poolManager.AddPool(ammoPrefab, 10);

            //Adding for taking out from pool and instantiating
            santa.SantaShot += (GameConstants.SantaAmmoType type) => {

                Reusable ammo = poolManager.Acquire(ammoPrefab.ReuseID);
                 
                SantaAmmo ammoInfo = ammo.GetComponent<SantaAmmo>();
                switch (type)
                {
                    //Prob like arrays where by reference until change then is copy
                    case GameConstants.SantaAmmoType.COAL:
                        ammoInfo.type = GameConstants.SantaAmmoType.COAL;
                        break;

                    case GameConstants.SantaAmmoType.PRESENT:
                        ammoInfo.type = GameConstants.SantaAmmoType.PRESENT;
                        break;
                }

                ammo.gameObject.transform.position = santa.transform.position;
                ammo.gameObject.SetActive(true);
            };
        }
        

        //For buttons to call.
        public void Pause()
        {

        }

        public void Play()
        {

        }


        void spawnObstacle()
        {

        }

        void GameOver()
        {

        }
    }
}