using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour

{

    // Public variables for speed, jump height and for checking if the game is over or paused.
    public float speed = 5;
    public float jumpHeight = 10.0f;
    public bool gameOver = false;
    public bool paused = false;

    // Camera game objects, the dirt particle and two sound effects, jump and crash.
    public GameObject firstPersonCamera;
    public GameObject thirdPersonCamera;
    public GameObject helicopterCamera;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    // Private floats that don't need to be accessed, by how much the character "crouches", the horizontal input for moving left and right
    // and whether the player is on the ground or is crouching.
    private float crouchFactor = 0.4f;
    private float horizontalInput;
    private bool isCrouching;
    private bool isGrounded = true;

    //  The scale of the player, this value gets edited when the player "crouches" and stands back up.
    private Vector3 playerScale;
    
    // The player's rigidbody (used in other scripts), the move object script and the audio source coming from the player for playing various sound effects.
    private Rigidbody playerRb;
    private MoveObject moveObjectScript;
    private AudioSource playerAudio;

    private void Start()

    {

        // Gets the components and the current scale of the player.
        playerRb = GetComponent<Rigidbody>();
        moveObjectScript = GameObject.Find("Helicopter").GetComponent<MoveObject>();
        playerScale = transform.localScale;
        playerAudio = GetComponent<AudioSource>();
        // The time scale is also set to 1, in case the player restarts the game from the restart game button while the game is paused, as otherwise the game would 
        // remain paused on restart.
        Time.timeScale = 1;

    }
    
    private void FixedUpdate()

    {

        // Gets the input in the fixed update to make it smoother.
        horizontalInput = Input.GetAxis("Horizontal");

    }

    private void Update()

    {

        // If the game isn't over:
        if (gameOver != true)

        {

            // Run the following methods.
            MovementCheck();
            CrouchCheck();
            ChangeCameras();

        }

        // If the player wants to pause the game:
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            
            // Call the pause game function.
            PauseGame();
            
        }

        // Regardless of what state the game is in, call the game over check to see if the game is over.
        GameOverCheck();

    }

    public void MovementCheck()

    {

        // Move the player left and right depending on the horizontal input gotten by holding A or D.
        transform.Translate(Vector3.right * (speed * horizontalInput * Time.deltaTime));

        // If the player presses the space bar or the W key:
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))

        {
            
            // and if the player is on the ground:
            if (isGrounded)

            {

                // and lastly if the player isn't crouching:
                if(!isCrouching)

                {

                    // Play the jumping sound effect.
                    playerAudio.PlayOneShot(jumpSound, 2.5f);
                    // Add upward force, i.e., jump.
                    playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                    // Stop playing the dirt particle.
                    dirtParticle.Stop();
                    // Set isGrounded to false, as the player is jumping.
                    isGrounded = false;

                }

            }

        }
        
    }

    public void CrouchCheck()

    {

        // If the player presses Left Ctrl, or the S key:
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.S))

        {

            // and if the player is grounded:
            if (isGrounded)

            {

                // If the player isn't crouching:
                if (!isCrouching)

                {

                    // Set crouching to true.
                    isCrouching = true;

                }

                // Alternatively, if the player is crouched:
                else

                {

                    // Set crouching to false;
                    isCrouching = false;

                }

            }

        }

        // If the player is currently crouching:
        if (isCrouching)

        {

            // Set the scale of player to be Y minus the crouch factor to make the player smaller.
            transform.localScale = new Vector3(playerScale.x, playerScale.y - crouchFactor, playerScale.z);

        }

        // If the player isn't currently crouching:
        else

        {

            // Set the player to their normal size.
            transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);

        }

    }

    public void GameOverCheck()

    {

        // If the player's Y position is less than 300, i.e., is below the building level, meaning they fell down a hole (with a little bit of wiggle room
        // before the game over is called to account for slower reaction times):
        if (transform.position.y < 300 && gameObject.CompareTag("Player"))

        {

            // Set game over to true.
            gameOver = true;

        }
        
        // If the helicopter's X value is the same as the player, or greater, i.e., it caught up to the player:
        if (moveObjectScript.helicopterX <= playerRb.transform.position.x)

        {

            // Set game over to true.
            gameOver = true;

        }

        // If the game is over:
        if (gameOver == true)

        {
            
            // Stop playing the dirt particle.
            dirtParticle.Stop();

        }

    }
    
    private void ChangeCameras()

    {

        // This method is responsible for which camera is actively being used.
        // If the player pressed 1 on the keyboard:
        if (Input.GetKeyDown(KeyCode.Alpha1))

        {

            // Enable the first person camera and disable the other cameras.
            firstPersonCamera.gameObject.SetActive(true);
            thirdPersonCamera.gameObject.SetActive(false);
            helicopterCamera.gameObject.SetActive(false);

        }

        // If the player pressed 2 on the keyboard:
        if (Input.GetKeyDown(KeyCode.Alpha2))

        {

            // Enable the third person camera and disable the other cameras.
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(true);
            helicopterCamera.gameObject.SetActive(false);

        }

        // If the player pressed 3 on the keyboard:
        if (Input.GetKeyDown(KeyCode.Alpha3))

        {

            // Enable the helicopter camera and disable the other cameras.
            firstPersonCamera.gameObject.SetActive(false);
            thirdPersonCamera.gameObject.SetActive(false);
            helicopterCamera.gameObject.SetActive(true);

        }

    }

    public void PauseGame()

    {

        // This method pauses and unpauses the game.
        // If the game isn't paused:
        if (!paused)

        {

            // Pause the game.
            paused = true;
            // Unlock the cursor, which allows the player to click on buttons.
            Cursor.lockState = CursorLockMode.None;
            // Set the time scale to zero, which pauses all functions using Time unless specified otherwise (there are no cases of this in my game).
            Time.timeScale = 0;

        }

        // Alternatively, if the game is paused:
        else if (paused)

        {

            // Unpause the game.
            paused = false;
            // Lock the cursor again so that it doesn't annoy the player when they move the mouse.
            Cursor.lockState = CursorLockMode.Locked;
            // Set the time scale to 1 to enable all the functions using Time again.
            Time.timeScale = 1;

        }
        
    }

    private void OnCollisionEnter(Collision collision)

    {

        // If the player collides with either the ground, or the ramp:
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Ramp"))

        {

            // Set grounded to true.
            isGrounded = true;
            // Continue playing the dirt particle, which is likely to be stopped as the player just landed.
            dirtParticle.Play();

        }

        // If the player collides with game objects tagged either room 1 or room 2:
        if (collision.gameObject.CompareTag("Room 1") || collision.gameObject.CompareTag("Room 2"))

        {

            // If the game object is room 1:
            if (collision.gameObject.CompareTag("Room 1"))

            {
                
                // Move the player to the right of the room they just hit.
                transform.position = new Vector3(transform.position.x, transform.position.y, -103.0f);
                
            }
            
            // And if the game object is room 2:
            else if (collision.gameObject.CompareTag("Room 2"))

            {
                
                // Move the player to the left of the room they just hit.
                transform.position = new Vector3(transform.position.x, transform.position.y, -109.0f);
                
            }

        }
        
        // If the player collides with either an obstacle, or the two rooms (i.e., they hit an obstacle that deals damage):
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Room 1") || collision.gameObject.CompareTag("Room 2"))

        {

            // Play the crashing sound effect.
            playerAudio.PlayOneShot(crashSound, 1.0f);
            
        }

    }

}