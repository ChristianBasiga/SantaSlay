using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SantaGame
{


    public delegate void LevelChanged();

    public class House : Reusable
    {
        
        //Makes sense to be LevelChanged delegate since passing house means level updated in way? Fuck it
        //reusing cause same signature lol.
        public event LevelChanged PassedHouse;
        public event LevelChanged EnteredHouseBorder;
        public GameConstants.HouseState houseState;


        void Awake()
        {
        }

        void Start()
        {
            
        }
        
        //For House Border to call
        public void DidPassHouse()
        {

            PassedHouse();
            BackToPool();
        }

        public void DidEnterHouse()
        {
            DidEnterHouse();
        }
       
    }
}