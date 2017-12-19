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
        PoolManager poolManger;
        public SantaAmmo coalPrototype;
        public SantaAmmo presentPrototype;
        
        void Awake()
        {
            santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();
            poolManger = GameObject.Find("PoolManager").GetComponent<PoolManager>();
        }


        void Start()
        {
            coalPrototype = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString()))).GetComponent<SantaAmmo>();
            presentPrototype = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.PRESENT.ToString()))).GetComponent<SantaAmmo>();

           
            //Only one pool for ammo, will use prototypes to switch between
            poolManger.AddPool(coalPrototype.ReuseID,coalPrototype,10);

            //Adding for taking out from pool and instantiating
            santa.SantaShot += (GameConstants.SantaAmmoType type) => {

                Reusable ammo = poolManger.Acquire(coalPrototype.ReuseID);

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

        }


        void spawnHouse()
        {

        }


        void spawnObstacle()
        {

        }
    }
}