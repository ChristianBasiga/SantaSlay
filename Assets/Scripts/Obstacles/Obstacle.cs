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

    public class Obstacle : Reusable
    {
        public float speed;
        public float pattSpeed;

        
        //This would be something all derived classes will have.
        //May not need to be virtual cause don't think going to do more on the obstacles
        //cause santa controller handling when hit player, though I could add that lol
        //but for now leave as is just to finish, note what I learned, and move on.
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                BackToPool();
        }


        //If out of view of camera, then just disappear
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Boundary"))
            {
                BackToPool();
            }
        }
    }
}