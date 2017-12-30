using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame {

    //Different kind of Obstacles that could drop:
    /*
     * Snowflakes(slows santa down)
     * Christmas reefs(Going towards slay in straight line) (Slow santa down and damage)
     * Star(multiplier)
     * 
     
   */
    public delegate Transform ObstacleMoveFunction(Transform intial,float speed, float pattSpeed);

    public class Obstacle : Reusable
    {
        //Really only needs to be in this scope, and it will be decided at start what it will have, based on the name of prefab
        public static readonly Dictionary<string, ObstacleMoveFunction> ObstacleActions = new Dictionary<string, ObstacleMoveFunction>()
        {
            //Okay since very special movement, inheritance might be okay after all
            {"Snowflake", (Transform init, float movSpeed, float patternSpeed) => {

                Vector2 movement = init.position + Vector2.up * -movSpeed;
                float amplitude = 0.8f;
                Vector2 waveAxis = Vector2.right * Mathf.Sin(Time.time * patternSpeed ) * amplitude;

                init.position =  movement + waveAxis;

            } },
            { "Star" , (Transform init, float movSpeed, float patternSpeed) => {

                //Will just be diagonal movement, but not y = x.

                //Tbh instead of returning since passing transform now, I could just straight up change the position too.
                Vector2 direction = (Vector2.up * -patternSpeed) + (Vector2.right * -movSpeed);

                init.localScale += new Vector2(1,1) * patternSpeed;

                init.position += direction;

            } },
            { "Wreeth" , (Transform init, float speed) => {

                //Idk yet.

            } }

            //ToDo: Rest of obstacles


        };

        //Don't need events for these since obstacle directly interacting with player unlike houses and ammo
        public float speedEffect;
        public int damage;
        public float speed;
        public float pattSpeed;

        ObstacleMoveFunction move;

        public void SetMovement(ObstacleMoveFunction move)
        {
            this.move = move;
        }


        void Update()
        {
            if (move != null)
                move(transform.position, Time.deltaTime * speed,pattSpeed * Time.deltaTime);
        }


        
        //This would be something all derived classes will have.
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                BackToPool();
        }
    }
}