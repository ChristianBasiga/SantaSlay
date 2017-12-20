using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    public Slider santaHP;
    public Text pointsLabel;
    Santa santa;


    void Awake()
    {
        santa = GameObject.FindGameObjectWithTag("Player").GetComponent<SantaController>().santa;
    }
	// Use this for initialization
	void Start () {

        //Todo: Test these to make sure call backs working right

        santaHP.maxValue = santa.Health;

        santa.healthUpdated += (int newHealth) => { santaHP.value = newHealth; };
        santa.pointsUpdated += (int newPoints) => { pointsLabel.text = "Scores: " + newHealth.ToString(); };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
