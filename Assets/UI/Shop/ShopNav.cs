using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopNav : MonoBehaviour
{
    public float Funds = 0; //Our cash
    [SerializeField] private Canvas ShopWindow;
    [SerializeField] private Canvas CheckoutWindow;
    [SerializeField] private Canvas NavigationButtons;
    [SerializeField] private Button ShopButton;
    [SerializeField] private Button CheckoutButton;
    [SerializeField] private TextMeshProUGUI FundTracker;

    void Update()
    {
        FundTracker.text = "Funds: $" + InventoryManager.Instance.Funds.ToString();
    }

    public void EnterShop()
    {
        NavigationButtons.enabled = true; //Should always enter in the shop
        CheckoutWindow.enabled = false;
        ShopWindow.enabled = true;
        ShopButton.interactable = false;
        CheckoutButton.interactable = true;
    }

    public void EnterCheckout()
    {
        ShopWindow.enabled = false;
        CheckoutWindow.enabled = true;
        CheckoutButton.interactable = false;
        ShopButton.interactable = true;
    }

    public void ExitShop()
    {
        NavigationButtons.enabled = false;
        ShopWindow.enabled = false;
        CheckoutWindow.enabled = false;
    }
}
