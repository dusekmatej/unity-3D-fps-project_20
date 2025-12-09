using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference for Panel pause menu
    private bool isPaused = false;

    void Update()
    {
        // Activation using escape key 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f;         
        Cursor.lockState = CursorLockMode.Locked; // cursor lock
        Cursor.visible = false;     
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f;        // Stop gametime
        Cursor.lockState = CursorLockMode.None;  // cursor unlock
        Cursor.visible = true;      
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); 
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
    
    public Transform playerTransform;

   public void SaveGame()
{
   GameManager.Savegame();
}

}