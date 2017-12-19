using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SantaGame;

public class SantaController : MonoBehaviour
{

    private Santa santa;
    //rigid component 
    private Rigidbody2D r2d;
    // Use this for initialization
    void Start()
    {
        //get component
        r2d = GetComponent<Rigidbody2D>();
        santa = new Santa(10, 0, 5);
    }
    void FixedUpdate()
    {
        //This is fine, the foreground will be moving so will look like always moving as well
        //horizontal
        float moveHorizontal = Input.GetAxis("Horizontal");


        //vertical 
        float moveVertical = Input.GetAxis("Vertical");

        //vector direction
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //movement 
        r2d.AddForce(movement * santa.Speed);
    }
}