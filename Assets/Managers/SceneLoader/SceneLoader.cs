using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public string SceneName;

    // Singleton design pattern, only 1 SceneLoader can exist at a time.
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            Debug.Log("SceneLoader already exists");
        }
        else
        {
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }
    }    

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
        switch(SceneName){
            case "Game":
                AudioManager.Instance.SwitchMusic(1);
                SceneName = "EndMenu";
                break;
            case "StartMenu":
                SceneName = "Game";
                break;
            case "EndMenu":
                AudioManager.Instance.SwitchMusic(0);
                SceneName = "StartMenu";
                break;
            default:
                Debug.Log("Tried to load invalid scene with Scene Loader.");
                break;
        }
    }

    public void LoadScene(string wawa){
        SceneName = wawa;
        LoadScene();
    }
}
