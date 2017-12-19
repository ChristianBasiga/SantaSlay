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

        public SantaAmmo ammoPrefab;
        House housePrefab;
        public Bird birdPrefab;
        public Plane planePrefab;

        void Awake()
        {
            santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();
            poolManager = GetComponent<PoolManager>();




            ammoPrefab = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString()))).GetComponent<SantaAmmo>();
            ammoPrefab.ReuseID = 1;

            housePrefab = ((GameObject)Resources.Load("Prefabs/House")).GetComponent<House>();
            //May actually just change to static oncstants as will start to get More hectic as more pools added.
            housePrefab.ReuseID = 2;

            #region Spawning obstacles
            //Could prob do neater, but at this point just get set up, nly change to make is make enum for diff kinda, but eh. Not needed and at that point mightaswell just
            //not have the derivations but need it for different updates and added functionality of Bird with multiplier, but we'll see. I'll put more thought into this later
            //Want more just done at this point so can start asking someone for art part.
            birdPrefab = ((GameObject)Resources.Load("Prefabs/Obstacles/Bird")).GetComponent<Bird>();
            birdPrefab.ReuseID = 3;
            poolManager.AddPool(birdPrefab, 4);

            planePrefab = ((GameObject)Resources.Load("Prefabs/Obstacles/Plane")).GetComponent<Plane>();
            poolManager.AddPool(planePrefab, 3);
            birdPrefab.ReuseID = 4;
            #endregion
        }


        void Start()
        {
            
            InitAmmoPool();
            InitHousePool();
        }


        private void InitHousePool()
        {
          
            //Could reuse notifier delegate had in MOdel, instead of making new one, but won't effect stuff in here
            housePrefab.AmmoHit += (int points) => { santa.santa.UpdatePoints(points); };
            //No more than 5 seeing at a time
            poolManager.AddPool(housePrefab, 5);
        }


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
        // Update is called once per frame
        void Update()
        {
            //Getting input just for testing
            if (Input.GetKeyDown(KeyCode.A))
            {

                //Where this is put will depend on level design
                spawnHouse();
            }

        }


        private void spawnHouse()
        {
            Reusable house = poolManager.Acquire(housePrefab.ReuseID);
            house.GetComponent<House>().AmmoHit += (int points) => { santa.santa.UpdatePoints(points); };

            house.gameObject.SetActive(true);
        }


        void spawnObstacle()
        {

        }
    }
}