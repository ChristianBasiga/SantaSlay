using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SantaGame;

public class SantaController : MonoBehaviour
{
    public delegate void AmmoDelegate(GameConstants.SantaAmmoType ammoType);
    public event AmmoDelegate SantaShot;

    private Santa santa;
    //rigid component 
    private Rigidbody2D r2d;
    //collider
    Collider col;

    //For delay between shots
    public float reloadTime;
    public float timeTillReload;
    
    //damaged
    bool = damaged;
    //damage sound
    AudioSource ???
    
    //HUD health update (can update this when HUD is made, just putting this for placeholder)
    public Slider healthSlider;
    
    //player current health
    public int currentHealth; 
    
    
    //dead
    bool = isDead;
   

  

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

    void Update()
    {

        //Just k for testing, will be something else later
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Test after add that method in there
            santa.SwitchAmmo();

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
    
    void OnTriggerEnter(Collider other)
      {
        if(other.CompareTag("Bird")){
        
        //mutliplier???
       GameManager.instance.ScoreMultiplier???();
    
      }
       else if(other.CompareTag("Obstacle"))
      {
       
       
        // damage flag
        damaged = true;
        
        //damageSound
       

        // Reduce health by amount
        currentHealth -= amount;

        // Set health to current value
        healthSlider.value = currentHealth;


        // If health reaches 0
        if(currentHealth <= 0 && !isDead)
        {
            // le dead
            Death ();
            
            //game over
            GameManager.instance.GameOver();        
            
        
        //slow movement
                           
      
        //place Obstacle back to pool
        <obstacleName>.BackToPool();
        
                
    
      }

    }

}
