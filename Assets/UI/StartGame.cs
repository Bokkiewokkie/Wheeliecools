using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartGameReal()
    {
        SceneLoader.Instance.LoadScene("Game");
    }

}
