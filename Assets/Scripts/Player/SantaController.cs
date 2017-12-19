using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SantaGame;

public class SantaController : MonoBehaviour
{

    private Santa santa;
    //rigid component 
    private Rigidbody2D r2d;


    public GameObject coalPrototype;
    public GameObject presentPrototype;

    // Use this for initialization
    ReuseablePool<SantaAmmo> bulletPool;
    void Start()
    {
        //get component
        r2d = GetComponent<Rigidbody2D>();

        //Retrieves the prefabs for coal and present ammo
        //These will act as prototypes to reduce Disk I/O and complicating Pool
        coalPrototype = Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString()));
        presentPrototype = Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.PRESENT.ToString()));

        //Starts off bullet buffer with 200
        bulletPool = new ReuseablePool<SantaAmmo>(200);
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

    void Update()
    {

        //Just k for testing, will be something else later
        if (Input.GetKeyDown(KeyCode.X))
        {
            santa.switchAmmo();

        }
        //Else cause can't shoot and swap ammo at same time
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }

    }


    void Shoot()
    {
        


    }


}