using UnityEngine;
using TMPro;

public class ShopItemBuy : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI Name;
    public void OnBuy()
    {
        ShopManager.Instance.Purchase(Name.text);
    }
}
