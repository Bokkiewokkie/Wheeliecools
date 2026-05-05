using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string SceneName;

    void Start()
    {
        
    }

    public void loadScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
