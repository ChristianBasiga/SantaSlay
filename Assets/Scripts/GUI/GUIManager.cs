using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SantaGame;

public class GUIManager : MonoBehaviour {

    public delegate void ButtonPressed();

    //events for these cause GameManager needs to do stuff when this happens
    //as well as SantaController, whatever else needs to be notified of these events    
    public event ButtonPressed PausePressed;
    public event Notifier DifficultyChanged;
    public event ButtonPressed QuitPressed;



    public Image loadingScreen;
    private bool doneLoading;

    public Text pointsLabel;
    public Text levelProgressLabel;
    public Text currentLevelLabel;

    SantaController santa;

    //Will be textures of all them cookies.
    private Sprite[] cookieHealthSprites;
    public SpriteRenderer healthSprite;



    void Awake()
    {
        loadingScreen.gameObject.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        doneLoading = true;

        if (scene.name == "TitleScreen")
        {

        }
        else if (scene.name == "Main")
        {

            //cookieHealthSprites = Resources.LoadAll<Sprite>("Cookies");

            //  healthSprite = player.GetComponent<SpriteRenderer>();

            //Tbh just different scripts would've looked alot cleaner than this
            //but hindsight, not erasing
            pointsLabel = GameObject.Find("PointsLabel").GetComponent<Text>();
            levelProgressLabel = GameObject.Find("LevelProgressLabel").GetComponent<Text>();
            currentLevelLabel = GameObject.Find("LevelLabel").GetComponent<Text>();

            santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();

            santa.PointsUpdated += (int newPoints) => { pointsLabel.text = "Points: " + newPoints.ToString(); };

            #region Assigning LevelManager Callbacks
            LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();

            lm.PassedHouse += (int currentProgress, int goal) => { levelProgressLabel.text = string.Format("Houses Passed: {0} / {1}", currentProgress, goal); };

            lm.ReachedEndOfLevel += () =>
            {
               
                doneLoading = false;
            };

            lm.LoadedLevel += () =>
            {
                doneLoading = true;
            };

            #endregion

        }


    }

    IEnumerator loadLoadingScreen()
    {
        //loadingScreen.gameObject.SetActive(true);
        //Here need to yield until done loading everything
        yield return new WaitUntil(() => doneLoading);
        //loadingScreen.gameObject.SetActive(false);
        
    }
    

    public void Pause()
    {
        PausePressed();
        //And could either add to call backs, but callbakcs should only need stuff from outsiders
        //I think that would be better design.
        //So the GUI stuff will be handled outside the event, just within this method.
        //ToDo:
        //Set active the Pause Menu
    }

    public void Quit()
    {
        if (QuitPressed != null)
            QuitPressed();
        //Nothing to do here for GUI.
        //Except maybe confirmation? GameManager itself will do the ApplicationQuit after it does everthing else need to do. In this case nothing else but amybe in future, saving.
    }

    public void DifficultyAltered(int newDiff)
    {
        //Cause need to pass in the new difficulty chosen, so that GameManager won't ahve to search for it.
        DifficultyChanged(newDiff);
    }

    //GameManager will just direcly call this
    //Hope keeps it's points. But it may not since would get destroyed
    public IEnumerator GameOver(bool didWin, string difficulty)
    {
        //DOPE IT DOES, OKAY MUCH EASIER YIPPY.
        //Debug.Log(pointsLabel.text);

        //Two frames, one frame for this to finish calling and another frame for scene to finish loading
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GameObject.Find("Level").GetComponent<Text>().text = currentLevelLabel.text;
        //This is called after scene change, but prob still considerd in last frame, which means IT might not work
        
        GameObject.Find("Difficulty").GetComponent<Text>().text = string.Format("Difficulty: {0}" ,difficulty);
        GameObject.Find("FinalPoints").GetComponent<Text>().text =  pointsLabel.text;

        string resultText = (didWin == true) ? "Congrats, yo. You a real one, santa helper" : "You suck, be nicer next year";
        GameObject.Find("EndingMessage").GetComponent<Text>().text = resultText;
        

    }
}
