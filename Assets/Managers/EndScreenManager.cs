using UnityEngine;
using TMPro;

public class EndScreenManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI ScoreField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        ScoreField.text += "Final Score\n";
        ScoreField.text += "Remaining Cash:" + InventoryManager.Instance.Funds.ToString() + "\n";
        ScoreField.text += "Completed Orders: " + ScoreManager.Instance.completedrequests.ToString();
    }

    public void ReturnToMenu()
    {
        SceneLoader.Instance.LoadScene();
    }

}
