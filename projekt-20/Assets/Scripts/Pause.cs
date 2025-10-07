using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Pro naèítání scén

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference na Panel pause menu
    private bool isPaused = false;

    void Update()
    {
        // Aktivace/deaktivace pause menu klávesou Escape
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
        pauseMenuUI.SetActive(false); // Skryj pause menu
        Time.timeScale = 1f;         // Obnov bìh hry
        Cursor.lockState = CursorLockMode.Locked; // Uzamkni kurzor
        Cursor.visible = false;     // Skryj kurzor
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Zobraz pause menu
        Time.timeScale = 0f;        // Zastav èas ve høe
        Cursor.lockState = CursorLockMode.None;  // Uvolni kurzor
        Cursor.visible = true;      // Zobraz kurzor
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Ukonèení aplikace
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Obnov èas (v hlavním menu èas bìží normálnì)
        SceneManager.LoadScene("MainMenu"); // Naèti scénu hlavního menu
    }
    public SaveSystem saveSystem;
    public Transform playerTransform;

   public void SaveGame()
{
    if (playerTransform != null)
    {
        saveSystem.SaveGame(playerTransform);
    }
    else
    {
        Debug.LogError("Transform hrace neni nastaven");
    }
}

}