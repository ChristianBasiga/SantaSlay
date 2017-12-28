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
    public delegate Vector2 ObstacleMoveFunction(Vector2 intial,float speed);

    public class Obstacle : Reusable
    {

        //Don't need events for these since obstacle directly interacting with player unlike houses and ammo
        public float speedEffect;
        public int damage;
        public int speed;

        ObstacleMoveFunction move;

        public void SetMovement(ObstacleMoveFunction move)
        {
            this.move = move;
        }


        void Update()
        {
            transform.position = move(transform.position, Time.deltaTime * speed);
        }


        
        //This would be something all derived classes will have.
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                BackToPool();
        }
    }
}