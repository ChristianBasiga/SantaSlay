using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaPlayer : MonoBehaviour
{


    //speed
    public float speed;

    //rigid component 
    private Rigidbody2D r2d;

    //collider component
    Collider col;

    // Use this for initialization
    void Start()
    {
        //get component
        r2d = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        //horizontal
        float moveHorizontal = Input.GetAxis("Horizontal");

        //vertical
        float moveVertical = Input.GetAxis("Vertical");

        //vector movement
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //movement 
        r2d.AddForce(movement * speed);

        
}


    