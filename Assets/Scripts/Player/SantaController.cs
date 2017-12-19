using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SantaGame;

public class SantaController : MonoBehaviour
{
    public delegate void AmmoDelegate(GameConstants.SantaAmmoType ammoType);
    public event AmmoDelegate SantaShot;

    private Santa _santa;
    //rigid component 
    private Rigidbody2D r2d;

    //For delay between shots
    public float reloadTime;
    public float timeTillReload;



    public Santa santa{

        get{
            return _santa;
        }
    }
    void Start()
    {
        //get component
        r2d = GetComponent<Rigidbody2D>();

   

        
       
        _santa = new Santa(10, 0, 5);
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
        r2d.AddForce(movement * _santa.Speed);


    }

    void Update()
    {

        //Just k for testing, will be something else later
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Test after add that method in there
            _santa.SwitchAmmo();
            //Yup points being updated
            Debug.Log(_santa.Points);

        }
        //Else cause can't shoot and swap ammo at same time
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (timeTillReload <= 0)
            {
                Shoot();
            }
        }

        if (timeTillReload > 0)
        {
            timeTillReload -= Time.deltaTime;
        }       
    }

    void Shoot()
    {
        
        timeTillReload = reloadTime;
      
        SantaShot(santa.dropping);

    }

}