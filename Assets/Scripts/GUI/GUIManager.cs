﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SantaGame;

public class GUIManager : MonoBehaviour {

    public delegate void ButtonPressed();

    //events for these cause GameManager needs to do stuff when this happens
    //as well as SantaController, whatever else needs to be notified of these events.
    public event ButtonPressed PausePressed;
    public event Notifier DifficultyChanged;
    public event ButtonPressed QuitPressed;



    public Image loadingScreen;
    public Text pointsLabel;
    public Text levelProgressLabel;
    public Text currentLevelLabel;

    SantaController santa;

    //Will be textures of all them cookies.
    private Sprite[] cookieHealthSprites;
    public SpriteRenderer healthSprite;



    void Awake()
    {
        //Or maybe instead of obstacles, when loses points also loses a cookie.
        //But could have obstacles too
        //cookieHealthSprites = Resources.LoadAll<Sprite>("Cookies");

        //  healthSprite = player.GetComponent<SpriteRenderer>();

        //Both MainScene and Titlescreen will have loading Screens.
        //MainScene will be after every level, Title will be when first entering game.
        if (loadingScreen != null)
            loadingScreen.gameObject.SetActive(false);
        else
        {
            Debug.Log("No loading screen referenced");
        }
        //Don't need to do all that if TitleScreen or GameOver, basicaly if  not MainScene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Main")
            return;

        santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>();

        santa.PointsUpdated += (int newPoints) => { pointsLabel.text = "Points: " + newPoints.ToString(); };


        LevelManager lm = GetComponent<LevelManager>();
        lm.PassedHouse += (int currentProgress, int goal) => { levelProgressLabel.text = string.Format("Houses Passed: {0} / {1}", currentProgress, goal); };



    }
    // Use this for initialization
    void Start () {
        /*santa.HealthUpdated += (int newHealth) => {

            healthSprite.sprite = cookieHealthSprites[newHealth];
        };
        */
        //Just realized think accidently removed the housesPassed/ num houses shit


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
}
