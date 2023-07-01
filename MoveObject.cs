using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveObject : MonoBehaviour

{

    // Move forward speed, i.e., the speed of the buildings and obstacles coming towards the player.
    public float moveForwardSpeed = 5;
    // Three variables affecting difficulty.
    public float helicopterSpeed = 5;
    public float helicopterX = 100;
    public float helicopterHitPenalty = 10;
    // Bool to indicate whether the player can take damage or not.
    public bool hitGracePeriod = false;

    // Slowdown and normal speed are private as they aren't used in other scripts unlike move forward speed.
    private float slowdownSpeed;
    private float normalSpeed;

    // Components that are gotten at start.
    private PlayerController playerControllerScript;
    private SpawnManager spawnManagerScript;
    private Rigidbody heliRb;
    private Rigidbody playerRb;

    void Start()
    
    {

        // Getting scripts and rigidbodies from objects in the scene.
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        heliRb = GameObject.Find("Helicopter").GetComponent<Rigidbody>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();

    }

    void Update()
    
    {

        // Runs the game loop inside an if statement to make sure everything stops if the game is over.
        if (playerControllerScript.gameOver == false)


        {

            MoveObjects();
            ChangeGround();
            ChangeBackground();
            DestroyObstacle();
            SlowdownGame();
            ApplyDifficulty();

        }

    }
    
    // MoveObjects moves the buildings and obstacles, as well as the helicopter if the left mouse button is held down.
    private void MoveObjects()

    {
        
        // If the object has the helicopter tag, and the left mouse button is held down:
        if (gameObject.CompareTag("Helicopter") && Input.GetMouseButton(0))

        {

            // The helicopter is moved forward, the whole equation is multiplied by helicopter speed and then divided by 5. The helicopter speed starts at 5,
            // but is increased by 1 every minute. 
            transform.Translate(Vector3.forward * Time.deltaTime * (helicopterSpeed / 5));

        }
            
        // If the object isn't a helicopter, i.e., building, obstacle, ramp etc:
        else if (!gameObject.CompareTag("Helicopter"))
            
        {
            
            // Move the object right, which moves the buildings against the player due to the rotation of the scene. 
            transform.Translate(Vector3.right * Time.deltaTime * moveForwardSpeed);
                
        }
        
        // Moves the helicopter value based on the helicopterX values, which is used for one of the two game over checks.
        helicopterX = transform.position.x;

    }

    // This method is used to move the background apartments, not the ones the player runs along. It is almost the same as the foreground building movement
    // but it also changes the height and the position of the building to add some variety to the level. 
    private void ChangeBackground()

    {
        
        // If the building's X value is 150 or less and the game object has the background tag:
        if (transform.position.x >= 150 && gameObject.CompareTag("Background"))

        {

            // Gets the scale of the building.
            Vector3 buildingScale = transform.localScale;
            
            // Transforms the scale of the building, keeping the X and Z constant but changes the Y value, which can be any number between 250 and 350. 
            transform.localScale = new Vector3(buildingScale.x, Random.Range(250, 350), buildingScale.z);
            // Changes the Y position of the object, and from testing having the Y value be half of the total scale means it sits perfectly on the ground.
            // This line also changes the X to -150, which sets it back to the start of the buildings that head towards the player.
            transform.position = new Vector3(-150, (transform.localScale.y / 2), transform.position.z);

        }
        
    }

    // Changes the buildings that the player runs along, simplified version of the above method. 
    private void ChangeGround()

    {

        // If the building's X value is 150 or less and the game object has the ground tag.
        if (transform.position.x >= 150 && gameObject.CompareTag("Ground"))

        {

            // Sends the building back to -150 on the X axis, i.e, becomes the building furthest from the player.
            transform.position = new Vector3(-150, transform.position.y, transform.position.z);
            spawnManagerScript.CreateObstacle();

        }
        
    }

    // Destroy Obstacles method, responsible for destroying obstacles that are out of play to help memory management.
    private void DestroyObstacle()

    {

        // If the position is less than or equal to 150, and the tag is either obstacle, room 1, room 2 or ramp:
        if (transform.position.x >= 150 && gameObject.CompareTag("Obstacle") || transform.position.x >= 150 && gameObject.CompareTag("Room 1")
            || transform.position.x >= 150 && gameObject.CompareTag("Room 2") || transform.position.x >= 150 && gameObject.CompareTag("Ramp"))

        {
            
            // Then destroy the object.
            Destroy(gameObject);
            
        }
        
    }

    private void SlowdownGame()

    {
        
        // If left click is held down:
        if (Input.GetMouseButtonDown(0))

        {

            // The normal speed variable (temporary) gets the speed at which the objects move,
            normalSpeed = moveForwardSpeed;
            // slow down speed (also temporary) becomes half of the speed at which the objects move,
            slowdownSpeed = moveForwardSpeed / 2;
            // and finally move forward speed gets set to the slowdown speed.
            moveForwardSpeed = slowdownSpeed;

        }

        // Once left click is released:
        else if (Input.GetMouseButtonUp(0))

        {

            // Move forward speed gets set back to its original speed which is held in move forward speed.
            moveForwardSpeed = normalSpeed;

        }
        
    }

    private void ApplyDifficulty()

    {

        // This function gets the values from spawn manager's increase difficulty function and applies them to the values that are in this script.
        // If left click isn't held down:
        if (!Input.GetMouseButton(0))

        {
            
            // Make the move forward speed the speed in spawn manager.
            // Done this way as this value would always override the value for slowing down, meaning that the game never slowed down when holding left click.
            moveForwardSpeed = spawnManagerScript.newMoveForwardSpeed;
            
        }
        
        // The rest of the values are always updated as nothing overrides them.
        helicopterSpeed = spawnManagerScript.newHelicopterSpeed;
        helicopterHitPenalty = spawnManagerScript.newHelicopterHitPenalty;

    }

    private void HelicopterObstacleHit()

    {
        
        // If the grace period is false, i.e., the player didn't take damage recently:
        if (hitGracePeriod == false)

        {

            // Make the helicopter move forward based on the hit penalty value.
            helicopterX = heliRb.transform.position.x - helicopterHitPenalty;
            // Also move the helicopter's model so that the player can see the helicopter getting closer. This also makes the helicopter
            // whirling sound effect louder as the sound effect is coming from the helicopter.
            heliRb.transform.position = new Vector3(helicopterX, heliRb.transform.position.y, heliRb.transform.position.z);

            // Hit Grace Period is set to true, meaning the player can't take damage while it's active.
            hitGracePeriod = true;
            // Starts the Grace Period co-routine, at the end of which the grace period is disabled. 
            StartCoroutine(GracePeriodRoutine());

        }

    }

    private void OnCollisionEnter(Collision collision)

    {

        // If the game object's tag is obstacle:
        if (gameObject.CompareTag("Obstacle"))

        {
            
            // and if the game object collides with the player:
            if (collision.gameObject.CompareTag("Player"))

            {

                // Move the obstacle back by 2.5 on the X axis, meaning it no longer collides with the player.
                transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z);
                // Call the helicopter obstacle hit method.
                HelicopterObstacleHit();

            }
            
        }

        // If the game object is tagged either with room 1 or room 2:
        if (gameObject.CompareTag("Room 1") || gameObject.CompareTag("Room 2"))

        {

            // and if the game object collides with the player:
            if (collision.gameObject.CompareTag("Player"))

            {

                // Call the helicopter obstacle hit method.
                HelicopterObstacleHit();

            }

        }

    }

    // The co-routine for the grace period.
    IEnumerator GracePeriodRoutine()

    {

        // While this co-routine is running, the player can't take more damage, allowing the player to have some wiggle room if they get caught up in obstacles.
        // This also prevents a bug where the player would take damage two frames in a row sometimes.
        // Wait for one second...
        yield return new WaitForSeconds(1);
        /// ...and set the grace period to false, meaning the player can take damage again.
        hitGracePeriod = false;

    }

}