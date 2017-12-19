using System.Collections;
using System.Collections.Generic;


namespace SantaGame
{

    //Class cause need as reference in lambda, otherwise would be struct
    public class Santa
    {

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
        }

        public void updatePoints(int points)
        {
            if (points < 0)
            {
                this.points = 0;
            }
            else
                this.points = points;

        }

        public SantaGame.GameConstants.SantaAmmoType dropping;
    }
}