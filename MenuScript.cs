using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour

{

    public TextMeshProUGUI keepUpText;
    public Image buttonBackground;
    public Canvas howToPlay;
    public Canvas about;
    public Canvas achievements;
    public GameObject helicopter;
    public TextMeshProUGUI highScoreText;
    public Image checkmark1;
    public Image checkmark2;
    public Image checkmark3;
    public Image checkmark4;
    public Image checkmark5;
    
    void Start()

    {

        // Set time scale to 1 if the player is coming back from a paused game scene.
        Time.timeScale = 1;
        // Update high score to be current high score.
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();

    }

    void Update()
    
    {

        // If the player presses escape:
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            
            // Go back to main menu.
            BackToMainMenu();
            
        }

    }

    public void StartGame()

    {

        // If the start game button is pressed, load the game scene.
        SceneManager.LoadScene("Game");

    }

    public void HowToPlay()

    {

        // Disable all elements.
        keepUpText.gameObject.SetActive(false);
        buttonBackground.gameObject.SetActive(false);
        helicopter.gameObject.SetActive(false);
        
        // Set how to play to true.
        howToPlay.gameObject.SetActive(true);

    }

    public void About()

    {

        // Disable all elements.
        keepUpText.gameObject.SetActive(false);
        buttonBackground.gameObject.SetActive(false);
        helicopter.gameObject.SetActive(false);
        
        // Set about to true.
        about.gameObject.SetActive(true);
        
    }

    public void Achievements()

    {

        // Disable all elements.
        keepUpText.gameObject.SetActive(false);
        buttonBackground.gameObject.SetActive(false);
        helicopter.gameObject.SetActive(false);

        // If any of the achievement playerprefs are equal to one, display a check mark beside them.
        // This is done for all 5 achievements.
        if (PlayerPrefs.GetInt("Achievement1") == 1)

        {

            checkmark1.gameObject.SetActive(true);

        }
        
        if (PlayerPrefs.GetInt("Achievement2") == 1)

        {

            checkmark2.gameObject.SetActive(true);

        }
        
        if (PlayerPrefs.GetInt("Achievement3") == 1)

        {

            checkmark3.gameObject.SetActive(true);

        }
        
        if (PlayerPrefs.GetInt("Achievement4") == 1)

        {

            checkmark4.gameObject.SetActive(true);

        }
        
        if (PlayerPrefs.GetInt("Achievement5") == 1)

        {

            checkmark5.gameObject.SetActive(true);

        }
        
        // Lastly, set achievements to true.
        achievements.gameObject.SetActive(true);
        
    }
    
    public void QuitGame()

    {

        // Quit game if the button is pressed.
        Application.Quit();

    }

    public void BackToMainMenu()

    {

        // Disable any of the elements that can be active.
        howToPlay.gameObject.SetActive(false);
        about.gameObject.SetActive(false);
        achievements.gameObject.SetActive(false);
        
        // Display all the main menu objects.
        keepUpText.gameObject.SetActive(true);
        buttonBackground.gameObject.SetActive(true);
        helicopter.gameObject.SetActive(true);

    }

    public void ResetProgress()

    {
        
        // If the reset button is pressed, set the high score and all achievements to be zero so that the player can start over again.
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.SetInt("Achievement1", 0);
        PlayerPrefs.SetInt("Achievement2", 0);
        PlayerPrefs.SetInt("Achievement3", 0);
        PlayerPrefs.SetInt("Achievement4", 0);
        PlayerPrefs.SetInt("Achievement5", 0);
        
    }
    
}