using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantaGame
{
    public static class GameConstants
    {


        public enum SantaAmmoType
        {
            COAL,
            PRESENT
        }

        //might put these in house and santa scripts instead but for now fine here
        //reason I say that is only in those scripts am I really using them?
        //well actualy spawner and playercontroller will.
        public enum HouseState
        {
            NAUGHTY,
            NICE
        }

        public static readonly Dictionary<HouseState, Dictionary<SantaAmmoType, int>> pointWorth = new Dictionary<HouseState, Dictionary<SantaAmmoType, int>>()
        {
            {HouseState.NAUGHTY, new Dictionary<SantaAmmoType, int>(){{SantaAmmoType.COAL , 3 }, {SantaAmmoType.PRESENT , -3} } },
            {HouseState.NICE, new Dictionary<SantaAmmoType, int>(){{SantaAmmoType.COAL, -3}, {SantaAmmoType.PRESENT, 3 } } }
        };

       
       
    }
}