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
        ReusablePool<House> housePool;
        ReusablePool<Obstacle> obstaclePool;

        void Start()
        {
            //I spawn different amounts per level
            //Perhaps no need for pool for this since Game design
            //may want to present naughty to nice houses in specific patterns
            housePool = new ReusablePool<House>(20);


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