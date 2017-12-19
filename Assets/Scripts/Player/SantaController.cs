using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SantaGame;

public class SantaController : MonoBehaviour
{

    private Santa santa;
    //rigid component 
    private Rigidbody2D r2d;


    public SantaAmmo coalPrototype;
    public SantaAmmo presentPrototype;

    // Use this for initialization
    ReusablePool<SantaAmmo> ammoPool;
    void Start()
    {
        //get component
        r2d = GetComponent<Rigidbody2D>();

        //Retrieves the prefabs for coal and present ammo
        //These will act as prototypes to reduce Disk I/O and complicating Pool
        coalPrototype = Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.COAL.ToString())) as SantaAmmo;
        presentPrototype = Resources.Load(string.Format("Prefabs/Ammo/{0}", GameConstants.SantaAmmoType.PRESENT.ToString())) as SantaAmmo;

        //Starts off bullet buffer with 200
        ammoPool = new ReusablePool<SantaAmmo>(200);
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
            //Test after add that method in there
           // santa.switchAmmo();

        }
        //Else cause can't shoot and swap ammo at same time
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }

    }


    void Shoot()
    {
        SantaAmmo ammo = ammoPool.Acquire();

        //Need to make it so if goes over buffer pool just instantiate new one

        //Copies correspodning prototype to bullet shot
        switch (santa.dropping)
        {
            case GameConstants.SantaAmmoType.PRESENT:
                ammo = presentPrototype;
                break;
            case GameConstants.SantaAmmoType.COAL:
                ammo = coalPrototype;
                break;
        }


        //Spawns the ammo
        Instantiate(ammo, transform);

    }


}