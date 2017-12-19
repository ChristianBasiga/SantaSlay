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
        public event Notifier healthUpdated;
        public event Notifier pointsUpdated;

        private int health;
        private int points;
        private int speed;

        public Santa(int health, int points, int speed)
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

                healthUpdated(health);
            }
        }

        public int Speed
        {

            get
            {
                return speed;
            }
            set
            {
                //Don't want it to be 0 either, cause santa should always be moving, background will be moving too for parrallax effect.
                if (value <= 5)
                {

                    //Minimum of 5
                    speed = 5;
                }
                else
                {
                    speed = value;
                }

            }
        }

        public int Points
        {
            get
            {
                return points;
            }
        }

        public void updatePoints(int points)
        {
            if (points < 0)
            {
                this.points = 0;
            }
            else
                this.points = points;

            pointsUpdated(points);
        }

        public SantaGame.GameConstants.SantaAmmoType dropping;
    }
}