using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public string mousePressSoundName = "ButtonPress";

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError("Game Master: No Audio Manager found in the scene!");
    }

    public void Quit()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void Retry()
    {
        //Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMousePress()
    {
        if (audioManager == null) audioManager = AudioManager.instance;
        audioManager.PlaySound(mousePressSoundName);
    }

}
