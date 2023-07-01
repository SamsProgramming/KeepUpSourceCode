using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour

{

    // Player UI Elements.
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI restartPrompt;
    public Image blackScreen;
    public Canvas playerUI;
    
    // Pause Menu Elements.
    public Canvas pauseMenu;
    public TextMeshProUGUI highScoreText;
    
    // Other Scripts.
    private PlayerController playerControllerScript;
    private MoveObject moveObjectScript;

    // Variables for getting the current time and the score.
    private float startTime;
    private float currentTime;
    private int score;

    void Start()
    
    {

        // Gets the scripts attached to other objects in the scene and gets the starting time of the game.
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        moveObjectScript = GameObject.Find("Helicopter").GetComponent<MoveObject>();
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()

    {

        // Gets the current time by subtracting the Time.time (i.e., time since the scene was loaded) from the start time.
        currentTime = Time.time - startTime;

        RunTimer();
        UIManagement();

        // If the enter key is pressed:
        if (Input.GetKeyDown(KeyCode.Return))

        {
            
            // Call the restart game function.
            RestartGame();
            
        }

        // Every frame update the high score text to be "High Score: " and the current high score. This appears if the game is paused.
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();

    }

    private void RunTimer()

    {

        // If the game isn't over, i.e., run the timer until the game ends and then pause it.
        if (playerControllerScript.gameOver == false)

        {

            // Get the minutes since the start of the game by dividing the current time by 60 and set it to a string for the timer.
            string minutes = ((int)currentTime / 60).ToString();
            // Get the seconds since the start of the game (minus the minutes) by getting the modulus 60 of current time and set it to a string for the timer.
            string seconds = (currentTime % 60).ToString("f2");

            // If the current seconds are a single digit, i.e., 0-9, print an extra zero to make it a standard time format.
            if ((currentTime % 60) < 10)

            {

                timerText.text = minutes + ":0" + seconds;

            }

            //  If the seconds are a double digit leave it as is.
            else

            {

                timerText.text = minutes + ":" + seconds;

            }

            // Call the score and achievements function.
            ScoreAndAchievements();

        }

    }

    private void UIManagement()

    {

        // If the game is over.
        if (playerControllerScript.gameOver == true)

        {

            // Set the game over text to be active when the game is over.
            gameOverText.gameObject.SetActive(true);
            // Set the restart prompt text to be active when the game is over.
            restartPrompt.gameObject.SetActive(true);

        }

        // Call the pause game method if the game is over, allowing the player to pause it do perform actions such as return to menu using a button
        // or quit the game directly as soon as the game is over.
        PauseGame();

    }

    private void PauseGame()

    {
        
        // If the game is paused:
        if (playerControllerScript.paused)

        {

            // Enable the pause menu canvas,
            pauseMenu.gameObject.SetActive(true);
            // and disable the player UI canvas to hide elements such as the timer to make the pause menu clearer.
            playerUI.gameObject.SetActive(false);

        }

        // If the game isn't paused, i.e., the game was paused and gets unpaused:
        else if (!playerControllerScript.paused)

        {

            // Disable the pause menu canvas,
            pauseMenu.gameObject.SetActive(false);
            // and enable the player UI again.
            playerUI.gameObject.SetActive(true);

        }
        
    }
    
    public void RestartGame()

    {

        // Reload the Game scene. This is triggered by either pressing Enter or pressing the restart game button in the pause menu.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ReturnToMenu()

    {

        // Load the main menu scene.
        SceneManager.LoadScene("Main Menu");

    }

    public void QuitGame()

    {

        // Quit the application, only works in the built version, doesn't work in the editor when using play mode.
        Application.Quit();

    }

    private void OnCollisionEnter(Collision collision)
    
    {

        // If the player collides with the road (the player has the UI script attached to it):
        if (collision.gameObject.CompareTag("Road"))

        {
            
            // Enable the blackscreen image when the player hits the road, symbolising the player falling several hundred meters to their death.
            blackScreen.gameObject.SetActive(true);

        }

    }

    private void ScoreAndAchievements()

    {
 
        // Score:
        // Set the score to the score variable, the score is determined by the amount of seconds survived since the start of the game (paused if the game is paused).
        score = (int) currentTime;

        // If the score is higher than the value of high score stored in player prefs:
        if (score > PlayerPrefs.GetInt("HighScore"))

        {
                
            // Set the high score to be the current score.
            PlayerPrefs.SetInt("HighScore", score);

        }

        // Achievements:
        // If the score is 30 or more and the first achievement's int value isn't 1 (same logic as using bools,
        // using this workaround as player prefs doesn't have bools, only ints, floats and strings):
        if (score >= 30 && PlayerPrefs.GetInt("Achievement1") != 1)

        {

            // Set the Achievement1's int value to be 1 rather than zero, i.e., is true.
            PlayerPrefs.SetInt("Achievement1", 1);

        }
        
        // If the score is 60 or more and achievement 2 is 0 (same code as above except for different achievement):
        if (score >= 60 && PlayerPrefs.GetInt("Achievement2") != 1)

        {

            // Set Achievement2's value to be 1.
            PlayerPrefs.SetInt("Achievement2", 1);

        }
        
        // If the score is 120 or more and achievement 3 is 0
        if (score >= 120 && PlayerPrefs.GetInt("Achievement3") != 1)

        {

            // Set Achievement3's value to be 1.
            PlayerPrefs.SetInt("Achievement3", 1);

        }

        // If the score is 180 or more and achievement 4 is 0
        if (score >= 180 && PlayerPrefs.GetInt("Achievement4") != 1)

        {

            // Set Achievement4's value to be 1.
            PlayerPrefs.SetInt("Achievement4", 1);

        }
        
        // If the score is over 30 AND the helicopter hasn't move much (allowing a little bit of slowdown but not getting hit by an obstacle)
        // and achievement 5's value is 0:
        if (score >= 30 && moveObjectScript.helicopterX > 99 && PlayerPrefs.GetInt("Achievement5") != 1)

        {

            // Set Achievement5's value to be 1.
            PlayerPrefs.SetInt("Achievement5", 1);

        }

    }

}