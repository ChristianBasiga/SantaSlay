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
    public event Notifier HealthUpdated;
    public event Notifier PointsUpdated;

    private SantaAnimations santaAnimations;

    //For before adding it, this will be for Santa face, Points updated gets full points
    //But this one will recieve the points to add as argument, actually I'm stupid I made this change so that won't need event for it
    //Just change ti here cauese Santa Controller will have reference to Sprite of itself, I could make another class called SantaAnimations
    //That will handle it, which not bad idea., fuck it 'etls do that.
   // public event Notifier PointsWillUpdate;

    private Santa santa;
    public Transform boundary;
    
    //For delay between shots
    public float reloadTime;
    public float timeTillReload;

    //OnSceneLoaded(Scene scene, LoadSceneMode mode)
    void Start()
    {
        santa = new Santa(10, 0, 5);
        santaAnimations = GetComponent<SantaAnimations>();
        PointsUpdated(santa.Points);
    }

    void Update()
    {
        

        //vertical 
        float moveVertical = Input.GetAxis("Vertical");

        //vector direction
        Vector3 movement = new Vector3(1, moveVertical,0);

        Vector3 newPos = transform.position + (movement * santa.Speed * Time.deltaTime);


        //Okay 0 point of objects in unity is in center NOT topleft.
        //Wait the .y is already halfway point cause it's radius not diameter I believe, we'll find out
        //Okay it IS diameter so do want only half of it.

        float minY = boundary.position.y - boundary.localScale.y / 2;
        minY += transform.localScale.y / 2;

        float maxY = boundary.position.y + boundary.localScale.y / 2;
        maxY -= transform.localScale.y / 2;

        float minX = boundary.position.x - boundary.localScale.x / 2;
        minX += transform.localScale.x / 2;

        float maxX = boundary.position.x + boundary.localScale.x / 2;
        maxX -= transform.localScale.x / 2;

        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX );
        if (newPos.x > maxX - transform.localScale.x)
        {
            //
            //Then almost at edge of boundary and need to move boundary, prob move to i enumerator that goes until
            boundary.Translate(Vector3.right * Time.deltaTime * santa.Speed * 2);
        }

        transform.position = newPos;
        //Just k for testing, will be something else later
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Test after add that method in there
            santa.SwitchAmmo();
            santa.Health -= santa.Health;
            HealthUpdated(santa.Health);
        
        }
        //Else cause can't shoot and swap ammo at same time
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (timeTillReload <= 0)
            {
                santaAnimations.SantaShoot();
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
            santa.Health -= obstacleInfo.damage;
            santa.Speed *= obstacleInfo.speedEffect;

            //Originally had this event in Santa, so gotta add here now.
            HealthUpdated(santa.Health);

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

           // transform.Translate(-lastTranslation);
        }

    }


    #region Public Methods


    public void UpdatePoints(int points)
    {
        // santaAnimations.UpdateSantaFacial(points);

        //Cause could pass in -3 when got wrong

        if (santa.Points == 0)
        {
            santa.Health -= points;
            //If out of points and drop, then start losing health
            HealthUpdated(santa.Health);
        }

        if (points == 0)
            return;

        if (santa.Points + points < 0)
            santa.Points = 0;
        else
            santa.Points += points;

        if (PointsUpdated != null)
            PointsUpdated(santa.Points);
    }

    #endregion
}