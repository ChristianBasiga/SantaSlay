using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SantaGame;

namespace SantaGame
{
    public class SantaController : MonoBehaviour
    {

        Santa santa;

        //May move this to GameConstants
        Dictionary<string, int> pointValues;
        public float reloadTime;
        float timeTillReloaded;
        //Replace this with speed field in Santa class later
        public float speed;
        // Use this for initialization
        void Start()
        {
            timeTillReloaded = 0;
            santa = new Santa();
        }

        // Update is called once per frame
        void Update()
        {

            //Just set translation, physics not needed for the on rails part, but will be needed
            //for obstacles falling
            transform.Translate(transform.right * speed * Time.deltaTime);


        }

        //Here will instantiate correct prefab
        void Shoot()
        {

        }
    }
}