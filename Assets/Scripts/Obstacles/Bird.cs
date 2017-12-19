using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantaGame
{
    public class Bird : Obstacle
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public float spawnRate
        {
            set
            {

            }
            get
            {
                //Spawn rate will be set by level, and returned by recalculaing for level, prob won't make too complicated
                //Tbh.
                return 1.0f; 
            }
        }
    }
}