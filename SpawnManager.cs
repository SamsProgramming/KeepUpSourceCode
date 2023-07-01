using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour

{
    
    // The spawn position, i.e, the center of the furthest back building when its position gets reset.
    public Vector3 spawnPos = new Vector3(-150, 300, -105);
    // The obstacle prefabs:
    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;
    public GameObject obstacle4;
    public GameObject obstacle5;
    public GameObject obstacle6;
    // Sound effect used when the difficulty gets increased.
    public AudioClip difficultyIncreaseSound;

    // 3 parameters affected by difficulty increasing, public as they are being called by the move object script.
    public float newMoveForwardSpeed;
    public float newHelicopterSpeed;
    public float newHelicopterHitPenalty;
    
    // Calling the move object script.
    private MoveObject moveObjectScript;
    // The audio source attached to the spawn manager empty object.
    private AudioSource playerAudio;
    
    // Int used to increase two out of three parameters a certain number of times, the last parameters gets increased forever.
    private int diffCount = 0;
    
    void Start()
    
    {
        
        // Invoke Repeating method called at the start. Invoke repeating will wait 60 seconds, and thereafter will repeat the IncreaseDifficulty method every 60 seconds.
        InvokeRepeating("IncreaseDifficulty", 60.0f, 60.0f);
        
        // Gets the move object script from the helicopter, as the helicopter is the only object that has one instance of it in the scene that has MoveObject
        // attached to it.
        moveObjectScript = GameObject.Find("Helicopter").GetComponent<MoveObject>();
        // Gets the audio source from the spawn manager empty object.
        playerAudio = GetComponent<AudioSource>();

        // Gets the values of the three variables at the start so that when they are increased they correspond to the value assigned at the start.
        newMoveForwardSpeed = moveObjectScript.moveForwardSpeed;
        newHelicopterSpeed = moveObjectScript.helicopterSpeed;
        newHelicopterHitPenalty = moveObjectScript.helicopterHitPenalty;

    }
    
    private void IncreaseDifficulty()

    {

        // This method is called every minute and is responsible for increasing the difficulty over time.
        // Plays the difficulty increase sound effect to notify the player that the game just got harder. Lowered volume to not make it too loud.
        playerAudio.PlayOneShot(difficultyIncreaseSound, 0.25f);
        // Increase the difficulty counter.
        diffCount++;

        // Hit penalty increase, starts at 10, and increases by 5. This is done 3 times, i.e., the hit penalty values are 10, 15, 20 and 25. 
        // If diffCount is 3 or less, i.e., if it's increased 3 times or less.
        if (diffCount <= 3)

        {

            // Add 5 to the helicopter hit penalty.
            newHelicopterHitPenalty = newHelicopterHitPenalty + 5;

        }

        // Helicopter speed increase, starts at 5, and increases by 1. This is done 5 times, i.e., the helicopter speed values are 5, 6, 7, 8, 9 and 10. 
        // If diffCount is 5 or less, i.e., if it's increased 5 times or less.
        if (diffCount <= 5)

        {

            // Add 1 to helicopter speed, which is further modified by the formula in the move object script.
            newHelicopterSpeed++;

        }
        
        // The speed of the buildings is increased by 1 no matter what, hopefully ramping up in speed until the player makes a mistake and loses the game.
        newMoveForwardSpeed++;

    }
    
    public void CreateObstacle()

    {

        // This method spawns an obstacle on the building furthest away from the player.
        // Generates a random number from 1 to 6 which determines which obstacle will be chosen.
        int obstacleChoice = Random.Range(1, 7);

        // If the obstacle choice is 1:
        if (obstacleChoice == 1)

        {
            
            // Spawns obstacle 1: the crates.
            Instantiate(obstacle1, spawnPos, obstacle1.transform.rotation);

        }

        // If the obstacle choice is 2:
        else if (obstacleChoice == 2)

        {

            // Spawns obstacle 2: the left room.
            Instantiate(obstacle2, spawnPos, obstacle2.transform.rotation);

        }

        // If the obstacle choice is 3:
        else if (obstacleChoice == 3)

        {

            // Spawns obstacle 3: the right room.
            Instantiate(obstacle3, spawnPos, obstacle3.transform.rotation);

        }

        // If the obstacle choice is 4:
        else if (obstacleChoice == 4)

        {

            // Spawns obstacle 4: the alternate crates.
            Instantiate(obstacle4, spawnPos, obstacle4.transform.rotation);

        }

        // If the obstacle choice is 5:
        else if (obstacleChoice == 5)

        {

            // Spawns obstacle 5: the crouching wall.
            Instantiate(obstacle5, spawnPos, obstacle5.transform.rotation);

        }

        // If the obstacle choice is 6:
        else if (obstacleChoice == 6)

        {

            // Spawns obstacle 6: the ramp (or harder crates if the player can't get on the ramp).
            Instantiate(obstacle6, spawnPos, obstacle6.transform.rotation);

        }

    }
    
}