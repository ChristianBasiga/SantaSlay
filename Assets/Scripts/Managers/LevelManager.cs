using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will handle moving the backgrounds and foregrounds for Parallex effect and to simulate the player moving
/// </summary>
namespace SantaGame {

    public class LevelManager : MonoBehaviour {

        public Transform background;
        public Transform foreground;
        //I want to reuse the one in reusable sincree same signatu
        int currentLevel;
        public event Notifier levelChanged;
    // Use this for initialization
        void Start() {

        }

        public void NextLevel()
        {
            currentLevel += 1;
            //This will update all the base states of obstacles, etc.
            levelChanged(currentLevel);
        }

    }
}