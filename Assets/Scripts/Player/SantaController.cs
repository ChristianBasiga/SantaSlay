using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SantaGame;

public class SantaController : MonoBehaviour
{
    public delegate void SantaHit();

    //This one may not be worth putting as event when really just setting a property, no other callbacks will be assigned to it other than that
    //nothing else special happens when HitBird, but incase we do I'll leave as is just to get done
    public event SantaHit BirdHit;

    public delegate void AmmoDelegate(GameConstants.SantaAmmoType ammoType);
    public event AmmoDelegate SantaShot;

    private Santa _santa;
    //rigid component 
    private Rigidbody2D r2d;


    //For delay between shots
    public float reloadTime;
    public float timeTillReload;

    


    
    public Santa santa {

        get {
            return _santa;
        }
    }
    void Start()
    {
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
        //r2d.AddForce(movement * _santa.Speed);
        transform.Translate(movement * _santa.Speed * Time.deltaTime);
    }
    void Update()
    {

        //Just k for testing, will be something else later
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Test after add that method in there
            _santa.SwitchAmmo();
            //Yup points being updated

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
        Debug.Log(_santa.Points);

    }

    void Shoot()
    {

        timeTillReload = reloadTime;

        SantaShot(santa.dropping);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //Obstacle handles itself dying and goign back to pool
        if (other.CompareTag("Obstacle"))
        {
            Obstacle obstacleInfo = other.GetComponent<Obstacle>();
            _santa.Health -= obstacleInfo.Damage;
            _santa.Speed *= obstacleInfo.SpeedEffect;

            //Inheritence may not actually be required for this, just composition with different instnaces of obstacles, will remove those classes
            //Only change is stats and those could be public properties set before hand in the prefabs
            if (other.gameObject.name.Contains("Bird")) {

                //This one may be worth just setting a public function on GameManager
                //But for now just calls the callback that activates multiplier
                BirdHit();
            }

        }

    }
}