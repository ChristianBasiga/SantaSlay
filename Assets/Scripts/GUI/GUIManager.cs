using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SantaGame;

public class GUIManager : MonoBehaviour {


    

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
	

}
