using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SantaGame
{

    public class GameManager : MonoBehaviour
    {


        public static GameManager instance;

        SantaController santa;
        
        private int level;
        private int difficulty;

      
        //Wait since all one image, I can't have this difference. Hmm. Fuck. I'll talk to Kris bout this.
        //FUck this for now, it works now to get it spawning, wait spawning is not a fucking thing cause it's all drawn out
        //Okay, no I can make this work. Instead of spawning house prefab with picture, it'll have no render and just spawn empty game Object with collider and HOuse script on it
        //And that way I determine the spawn points by position. Okay this will still work
        Dictionary<int, float> NaughtyChances = new Dictionary<int, float>()
        {
            {1 , 10.0f },
            {2 , 25.0f },
            {3 , 60.0f },
        };

        public SantaAmmo ammoPrefab;
        public Transform boundary;

        public Obstacle birdPrefab;
        public Obstacle planePrefab;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();
            SceneManager.sceneLoaded += OnSceneLoaded;

            //For transitioning from TitleScreent o actual Game Scene. Levels will not be scene transitions.
            DontDestroyOnLoad(gameObject);
          
        }

        void Start()
        {
            level = 1;
            Debug.Log(difficulty);
        }


        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            GUIManager guiManager = GetComponent<GUIManager>();

            switch (scene.name)
            {
                case "TitleScreen":


                    //In titlescreen because they can decide on difficulty before entering game

                    guiManager.DifficultyChanged += (int newDiff) => { this.difficulty = newDiff; };
                    guiManager.QuitPressed += () => {

                        Application.Quit();

                    };

                    break;

                case "Main":


                    #region Santa Boundary and setting GameOver callback


                    santa.Width = boundary.localScale.x / 2;
                    santa.Height = boundary.localScale.y / 2;

                    santa.HealthUpdated += (int newHealth) =>
                    {
                        if (newHealth <= 0)
                        {
                            GameOver();
                        }
                    };

                    #endregion

                    #region Assigning LevelManager Callbacks

                    LevelManager levelManager = GetComponent<LevelManager>();

                    //When reach end of level, need to update num houses and everything for next one.
                    levelManager.ReachedEndOfLevel += () => {

                        //GUi manager should be doing this, but fuck it, it's public
                        guiManager.currentLevelLabel.text = "Level: " + level.ToString();
                        levelManager.NumberOfHouses = (int)(((difficulty / 2.0f) * (2.0f * level + 7)) + 1);

                        //Incrementing for next time need to update.
                        level += 1;
                    };

                    #endregion

                    #region Assignign GUIManager Callbacks
                    //Temp, cause don't need to use again outside of assigning callbacks

                    guiManager.PausePressed += () => {

                        santa.enabled = !santa.enabled;

                    };

                    #endregion 

                    InitAmmoPool();      
                    

                    break;


            }
        }

        //TO follow suit of LevelManager, I could even have SantaControler have PoolManager
        //Reference
        private void InitAmmoPool()
        {


            PoolManager poolManager = GetComponent<PoolManager>();

            ammoPrefab = ((GameObject)Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString()))).GetComponent<SantaAmmo>();
            ammoPrefab.ReuseID = 1;

            //Only one pool for ammo, will use prototypes to switch between
            poolManager.AddPool(ammoPrefab, 10);

            //Adding for taking out from pool and instantiating
            santa.SantaShot += (GameConstants.SantaAmmoType type) => {

                Reusable ammo = poolManager.Acquire(ammoPrefab.ReuseID);
                 
                SantaAmmo ammoInfo = ammo.GetComponent<SantaAmmo>();
                switch (type)
                {
                    //Prob like arrays where by reference until change then is copy
                    case GameConstants.SantaAmmoType.COAL:
                        ammoInfo.type = GameConstants.SantaAmmoType.COAL;
                        break;

                    case GameConstants.SantaAmmoType.PRESENT:
                        ammoInfo.type = GameConstants.SantaAmmoType.PRESENT;
                        break;
                }

                ammoInfo.HitHouse += santa.UpdatePoints;

                ammo.gameObject.transform.position = santa.transform.position;
                ammo.gameObject.SetActive(true);
            };

            santa.UpdatePoints(0);
        }


        void spawnObstacle()
        {

        }

        void GameOver()
        {

        }
    }
}