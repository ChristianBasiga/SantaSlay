using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SantaGame;

public class GUIManager : MonoBehaviour {


    

    public Text pointsLabel;
    public Text levelProgressLabel;

    Santa santa;

    //Will be textures of all them cookies.
    private Sprite[] cookieHealthSprites;
    public SpriteRenderer healthSprite;



    void Awake()
    {
        //Or maybe instead of obstacles, when loses points also loses a cookie.
        //But could have obstacles too
        //cookieHealthSprites = Resources.LoadAll<Sprite>("Cookies");
      
      //  healthSprite = player.GetComponent<SpriteRenderer>();
    }
	// Use this for initialization
	void Start () {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        santa = player.GetComponent<SantaController>().santa;

        if (santa == null)
        {
            Debug.Log("something up");
        }


        /*santa.healthUpdated += (int newHealth) => {

            healthSprite.sprite = cookieHealthSprites[newHealth];
        };
        */
        //Rn only ever  updates to current hmm
        santa.pointsUpdated += (int newPoints) => { pointsLabel.text = "Scores: " + newPoints.ToString(); };

        //ToDo: Play corresponding change in facial expression if get wrong or right ammo hit
        //Hmmmm
        //I could reuse ammoHIt which was original intention but then that's mixing responsibilities cause GUI has no referencde to the houses and 
        //LevelManager would have to add that callback for GUI manager
        //Fuck it okay, just literally iterate through ALL houses in scene and assign tha call back here
        //Best way to not turn this into spaghetti code, more than it may already look


        LevelManager levelManager = GetComponent<LevelManager>();
        levelManager.PassedHouse += (int progress, int goal) =>
        {


            levelProgressLabel.text = string.Format("Houses Visited: {0} / {1}", progress, goal);

        };


        GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
        Debug.Log(houses.Length);
        //For adding callbacks to update santa face when fuckity the fuck up, yeahhh, won't work since not active. So basically NEEEDDDDDDDD to do add it during pool process but fuck
        foreach (GameObject house in houses)
        {


            house.GetComponent<House>().AmmoHit += (int pointGain) =>
            {
                Debug.Log("house hit");
                if (pointGain < 0)
                {
                    Debug.Log("Santa mad Yo");
                    //DIsplay santa mad
                }
                if (pointGain > 0)
                {
                    Debug.Log("Santa happy Yo");

                    //Display santa happeh
                }
            };
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
