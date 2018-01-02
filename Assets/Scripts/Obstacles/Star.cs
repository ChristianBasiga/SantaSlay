using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantaGame
{
    public class Star : Obstacle
    {


        public float movSpeed
        {

            get
            {
                return 1.0f;
            }
            set
            {

            }
        }

        public float patternSpeed
        {
            get
            {
                return 1.0f;
            }
            set
            {


            }
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            Vector3 direction = Time.deltaTime * ((Vector3.up * -patternSpeed) + (Vector3.right * -movSpeed));

            transform.localScale += new Vector3(1, 1) * patternSpeed * Time.deltaTime;

            transform.position += direction;
        }

       
    }
}