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
        public SantaAmmo coalPrototype;
        public SantaAmmo presentPrototype;
        House housePrefab;
        void Awake()
        {
            santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();
            poolManager = GetComponent<PoolManager>();
        }


        void Start()
        {
            
            InitAmmoPool();
            InitHousePool();
        }


        private void InitHousePool()
        {
            housePrefab = ((GameObject)Resources.Load("Prefabs/House")).GetComponent<House>();
            housePrefab.ReuseID = 2;
            //Could reuse notifier delegate had in MOdel, instead of making new one, but won't effect stuff in here
            housePrefab.AmmoHit += (int points) => { santa.santa.UpdatePoints(points); };
            //No more than 5 seeing at a time
            poolManager.AddPool(housePrefab.ReuseID, housePrefab, 5);
        }


        private void InitAmmoPool()
        {

            coalPrototype = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString()))).GetComponent<SantaAmmo>();
            presentPrototype = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.PRESENT.ToString()))).GetComponent<SantaAmmo>();

            coalPrototype.ReuseID = 1;
            //Only one pool for ammo, will use prototypes to switch between
            poolManager.AddPool(coalPrototype.ReuseID, coalPrototype, 10);

            //Adding for taking out from pool and instantiating
            santa.SantaShot += (GameConstants.SantaAmmoType type) => {

                Reusable ammo = poolManager.Acquire(coalPrototype.ReuseID);
                 
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
                Reusable house = poolManager.Acquire(housePrefab.ReuseID);
                house.GetComponent<House>().AmmoHit += (int points) => { santa.santa.UpdatePoints(points); };

                house.gameObject.SetActive(true);
                //Where this is put will depend on level design
            }

        }


        void spawnHouse()
        {

        }


        void spawnObstacle()
        {

        }
    }
}