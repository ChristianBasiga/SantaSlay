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
        


        void Start()
        {
            poolManger = PoolManager.Instance;
            coalPrototype = Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString())) as SantaAmmo;
            presentPrototype = Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.PRESENT.ToString())) as SantaAmmo;

            //Only one pool for ammo, will use prototypes to switch between
            poolManger.AddPool(coalPrototype.ReuseID,coalProtoType,100);

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