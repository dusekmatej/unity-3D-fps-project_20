using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public SaveSystem saveSystem; // Odkaz na SaveSystem

    public void ContinueGame()
    {
        // Pokud existuje ulo�en� hra, na�ti pozici hr��e
        if (PlayerPrefs.HasKey("HasSave") && PlayerPrefs.GetInt("HasSave") == 1)
        {
            // Na��t�n� pozice hr��e
            if (saveSystem != null)
            {
                //saveSystem.LoadGame(); // Na��st pozici
                SceneManager.LoadScene("SampleScene"); // P�echod do hern� sc�ny (GameScene)
            }
            else
            {
                Debug.LogError("SaveSystem nen� p�ipojen. Ujist�te se, �e je spr�vn� propojen.");
            }
        }
        else
        {
            Debug.LogError("��dn� ulo�en� hra nen� k dispozici.");
        }
    }

    public void NewGame()
    {
        // Pokud za��n�te novou hru, sma�te ulo�en� data
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("SampleScene"); // P�echod na novou hru
    }
}
