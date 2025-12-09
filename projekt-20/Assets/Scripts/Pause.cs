using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Pro na��t�n� sc�n

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference na Panel pause menu
    private bool isPaused = false;

    void Update()
    {
        // Aktivace/deaktivace pause menu klavesou Escape
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
        Time.timeScale = 1f;         // Obnov beh hry
        Cursor.lockState = CursorLockMode.Locked; // Uzamkni kurzor
        Cursor.visible = false;     // Skryj kurzor
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Zobraz pause menu
        Time.timeScale = 0f;        // Zastav cas ve hre
        Cursor.lockState = CursorLockMode.None;  // Uvolni kurzor
        Cursor.visible = true;      // Zobraz kurzor
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Ukonceni aplikace
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Obnov cas (v hlavnim menu cas bude normalni)
        SceneManager.LoadScene("MainMenu"); // Nacti scenu hlavniho menu
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