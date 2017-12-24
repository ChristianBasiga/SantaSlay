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

    //Will be set by level or game manager
    float width;
    float height;

    public float Width
    {

        set { width = value; }
    }

    public float Height
    {
        set { height = value; }

    }
   

    //For delay between shots
    public float reloadTime;
    public float timeTillReload;

    //For keeping within boundaries, level manager will just auto push it or can just do here.


    


    
    public Santa santa {

        get {
            return _santa;
        }
    }
    void Start()
    {

        _santa = new Santa(10, 0, 5);
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        //vertical 
        float moveVertical = Input.GetAxis("Vertical");

        //vector direction
        Vector3 movement = new Vector3(moveHorizontal, moveVertical,0);

        Vector3 newPos = transform.position + (movement * _santa.Speed * 0.1f);

        //Stay within same position, unity has up for down so don't ned to be engative
        newPos.y = Mathf.Clamp(newPos.y, -height + (transform.localScale.y), height - (transform.localScale.y));
        newPos.x = Mathf.Clamp(newPos.x, -width + (transform.localScale.x), width - (transform.localScale.x));

        transform.position = newPos;
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
            _santa.Health -= obstacleInfo.damage;
            _santa.Speed *= obstacleInfo.speedEffect;

            //Inheritence may not actually be required for this, just composition with different instnaces of obstacles, will remove those classes
            //Only change is stats and those could be public properties set before hand in the prefabs
            if (other.gameObject.name.Contains("Bird")) {

                //This one may be worth just setting a public function on GameManager
                //But for now just calls the callback that activates multiplier
                BirdHit();
            }

        }
        else if (other.CompareTag("Boundary"))
        {
            Debug.Log("Hello");
           // transform.Translate(-lastTranslation);
        }

    }
}