using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public void Play()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("Scenes/TestTutorialLevel");
        // SaveManager.LoadGame();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P)) SaveManager.SaveGame();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
