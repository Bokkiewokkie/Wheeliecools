using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image Filling;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI PriceTag;

    public void OnSpawn(Color color, string NameGiven, float PriceGiven)
    {
        Filling.color = color;
        Name.text = NameGiven;
        PriceTag.text = "$" + PriceGiven.ToString();
    }
}
