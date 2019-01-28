using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    AudioManager audioManager;

    [SerializeField]
    string sceneToLoad;
    [SerializeField]
    string pressButtonSound = "ButtonPress";

    private void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No Audio Manager found!");
        }
    }

    public void StartGame(){
        audioManager.PlaySound(pressButtonSound);
        if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
		    SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene \"" + sceneToLoad +"\" does not exist!");
        }
		
	}

    public void QuitGame()
    {
        audioManager.PlaySound(pressButtonSound);
        Application.Quit();
    }
}
