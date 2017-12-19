using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantaGame
{
    public class SantaAmmo : Reusable
    {

        public GameConstants.SantaAmmoType type;
        //Just speed really rn, if want coal that lasts longer will do later
        float speed;
       

        RigidBody2D rb;

       
       
        void Start()
        {
            poolID = 1;
             

        }
        void Update()
        {


        }
    }
}