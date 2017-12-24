using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SantaGame;

public class GUIManager : MonoBehaviour {


    

    public Text pointsLabel;
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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
