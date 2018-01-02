using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame {
    public class SnowFlake : Obstacle
    {

      
        public float amplitude;

        //Snowflake only one that actually effects santa directly(May change wreeth to do same again later)
        //May make this an event, but for now fuck it lol.
        public int damage;
        public float speedEffect;

        public float movSpeed
        {

            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public float patternSpeed
        {
            get
            {
                return pattSpeed;
            }
            set
            {
                pattSpeed = value;

            }
        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

            //Todo: Wreath prob sinwave instead, and this an arc motion
            Vector3 movement = transform.position + Vector3.up * -movSpeed;

            Vector3 waveAxis = Vector3.right * Mathf.Sin(Time.time * patternSpeed) * amplitude;

            transform.position = movement + waveAxis;

        }
    }
}