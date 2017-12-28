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
        private float difficulty;

      
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

        LevelManager levelManager;

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

            level = 1;
            SceneManager.sceneLoaded += OnSceneLoaded;

            DontDestroyOnLoad(gameObject);

        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            GUIManager guiManager = GetComponent<GUIManager>();

            switch (scene.name)
            {
                case "TitleScreen":


                    //In titlescreen because they can decide on difficulty before entering game
                    //This is fine as this cause only one instance at this point
                    guiManager.DifficultyChanged += (int newDiff) => { instance.difficulty = (float)newDiff; };
                    guiManager.QuitPressed += () => {

                        Application.Quit();

                    };

                    break;

                case "Main":


                    #region Santa Boundary and setting GameOver callback

                    santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();
                    boundary = GameObject.Find("Boundary").GetComponent<Transform>();
                    santa.Width = boundary.localScale.x / 2;
                    santa.Height = boundary.localScale.y / 2;

                    santa.HealthUpdated += (int newHealth) =>
                    {
                        if (newHealth <= 0)
                        {
                            instance.GameOver();
                        }
                    };

                    #endregion

                    #region Assigning LevelManager Callbacks

                    levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

                    //When reach end of level, need to update num houses and everything for next one.
                    levelManager.ReachedEndOfLevel += () => {

                        //GUi manager should be doing this, but fuck it, it's public
                        guiManager.currentLevelLabel.text = "Level: " + instance.level.ToString();
                        levelManager.NumberOfHouses =  (int)(((instance.difficulty / 2) * (2.0f * instance.level + 7)) + 1);

                        instance.level += 1;
                    };


                    #endregion

                    #region Assignign GUIManager Callbacks
                    //Temp, cause don't need to use again outside of assigning callbacks

                    guiManager.PausePressed += () => {

                        instance.santa.enabled = !santa.enabled;

                    };

                    #endregion 

                    InitAmmoPool();

                    break;
                    

            }
            //So that don't add it and call twice
            SceneManager.sceneLoaded -= OnSceneLoaded;


        }

      

        //TO follow suit of LevelManager, I could even have SantaControler have PoolManager
        //Reference
        private void InitAmmoPool()
        {


            PoolManager poolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();

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

            //To initialize the GUI
            santa.UpdatePoints(0);
        }

        //For the button, need to add the neccessarry components
        public void Play()
        {
            //Depreacted: Decided to just have objects that ahve these aleady in mainscene
            gameObject.AddComponent(typeof(LevelManager));
            gameObject.AddComponent(typeof(PoolManager));

        }

        void GameOver()
        {


        }
    }
}