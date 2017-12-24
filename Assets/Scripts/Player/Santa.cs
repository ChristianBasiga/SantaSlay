using System.Collections;
using System.Collections.Generic;


namespace SantaGame
{

    //Class cause need as reference in lambda, otherwise would be struct
    public delegate void Notifier(int val);
    public class Santa
    {
        //The GameManager will add to this a function to check if health is 0 and create GameOver instance
        //GUI manager will add to this to update GUI accordingly
        //Perhaps put this event in controller, since health updating is modifying data. Hmm
        //Maybe also move these to SantaController, since these being updated is controlling Santa, I believe.

      
        private int health;
        private int points;
        private float speed;

        public Santa(int health, int points, float speed)
        {
            this.health = health;
            this.points = points;
            this.speed = speed;
           
        }


        public int Health
        {

            get
            {
                return health;
            }
            set
            {
                if (value < 0)
                {
                    health = 0;
                }
                else
                    health = value;

            }
        }

        public float Speed
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

        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

      

        public void SwitchAmmo()
        {
            if (dropping == GameConstants.SantaAmmoType.COAL)
            {
                dropping = GameConstants.SantaAmmoType.PRESENT;
            }
            else
            {
                dropping = GameConstants.SantaAmmoType.COAL;
            }
        }

        public SantaGame.GameConstants.SantaAmmoType dropping;
    }
}