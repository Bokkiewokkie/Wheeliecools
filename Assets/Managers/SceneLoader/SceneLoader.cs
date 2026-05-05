using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void Start()
    {
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
        switch(SceneName){
            case "Game":
                AudioManager.Instance.SwitchMusic(1);
                break;
            case "StartMenu":
                AudioManager.Instance.SwitchMusic(0);
                break;
            default:
                Debug.Log("Tried to load invalid scene with Scene Loader.");
                break;
        }
    }
}
